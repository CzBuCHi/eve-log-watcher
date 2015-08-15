﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using eve_log_watcher.cva_kos_api;
using eve_log_watcher.server;
using EveAI.Live;
using EveAI.Live.Character;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eve_log_watcher
{
    public partial class FormCva : Form
    {
        private static readonly FormCva _Form = new FormCva();
        public readonly DataTable DataTable;

        private FormCva() {
            InitializeComponent();
            DataTable = new DataTable();
            DataTable.Columns.Add("PilotName", typeof (string));
            DataTable.Columns.Add("PilotKos", typeof (bool));
            DataTable.Columns.Add("CorporationName", typeof (string));
            DataTable.Columns.Add("CorporationKos", typeof (bool));
            DataTable.Columns.Add("AllianceName", typeof (string));
            DataTable.Columns.Add("AllianceKos", typeof (bool));
            DataTable.Columns.Add("Kos", typeof (bool));
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool CanBeClosed { get; set; }

        private long _CurrentSystemId;
        public static FormCva ShowMe(long currentSystemId) {
            _Form._CurrentSystemId = currentSystemId;
            _Form.Text = @"Loading ...";
            try {
                FillDataTable();
            } catch (Exception exc) {
                MessageBox.Show(@"Error during filling datatable: " + exc.Message);
                return null;
            }
            _Form.Show();
            return _Form;
        }

        private void AfterFillDataTable(int kosCount) {
            labelLoading.Visible = false;
            dataGridCva.Visible = true;

            Text = @"KOS: " + kosCount;

            _Form.dataGridCva.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            _Form.dataGridCva.DataSource = _Form.DataTable.DefaultView;
            _Form.dataGridCva.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            dataGridCva.Width = dataGridCva.Columns.Cast<DataGridViewColumn>().Sum(c => (c.Visible ? c.Width : 0) + c.DividerWidth) + 1;
            dataGridCva.Height = dataGridCva.ColumnHeadersHeight + dataGridCva.Rows.Count*(dataGridCva.RowTemplate.Height + dataGridCva.RowTemplate.DividerHeight);


            string[] kos = _Form.DataTable.Rows.Cast<DataRow>().Where(row => (bool) row["Kos"]).Select(row => (string) row["PilotName"]).ToArray();
            if (kos.Length > 0) {
                var data = new {
                    SystemId = _Form._CurrentSystemId,
                    Kos = kos
                };
                EveIntelServerConnector.SendMessage(JObject.FromObject(data).ToString(Formatting.None));
            }
        }

        private void dataGridCva_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            Color red = Color.FromArgb(250, 172, 163);
            Color redSelected = Color.FromArgb(250, 127, 113);

            foreach (DataGridViewRow row in dataGridCva.Rows) {
                if ((bool) row.Cells[colPilotKos.Name].Value) {
                    row.Cells[colPilotName.Name].Style.BackColor = red;
                    row.Cells[colPilotName.Name].Style.SelectionBackColor = redSelected;
                }
                if ((bool) row.Cells[colCorporationKos.Name].Value) {
                    row.Cells[colCorporationName.Name].Style.BackColor = red;
                    row.Cells[colCorporationName.Name].Style.SelectionBackColor = redSelected;
                }
                if ((bool) row.Cells[colAllianceKos.Name].Value) {
                    row.Cells[colAllianceName.Name].Style.BackColor = red;
                    row.Cells[colAllianceName.Name].Style.SelectionBackColor = redSelected;
                }
            }
            if (dataGridCva.SortedColumn == null) {
                DataGridViewColumn column = dataGridCva.Columns[colKos.Name];
                Debug.Assert(column != null);
                dataGridCva.Sort(column, ListSortDirection.Descending);
            }
        }

        private void FormCva_FormClosing(object sender, FormClosingEventArgs e) {
            if (!CanBeClosed) {
                e.Cancel = true;
                Hide();
            }
        }

        #region FillDataTable

        private class FillDataTableArg
        {
            public DataTable Table { get; set; }
            public List<string> Names { get; set; }
        }

        private static bool _FillingDataTable;

        private static void FillDataTable() {
            if (_FillingDataTable) {
                return;
            }
            _FillingDataTable = true;
            _Form.labelLoading.Visible = true;
            _Form.dataGridCva.Visible = false;
            _Form.DataTable.Rows.Clear();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += FillDataTable;
            worker.RunWorkerCompleted += (sender, args) => {
                if (args.Error != null) {
                    MessageBox.Show(args.Error.Message);
                    return;
                }

                _Form.AfterFillDataTable((int) args.Result);
                _FillingDataTable = false;
            };

            List<string> names = Clipboard.GetText().Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            worker.RunWorkerAsync(new FillDataTableArg {
                Table = _Form.DataTable,
                Names = names
            });
        }

        private static void FillDataTable(object sender, DoWorkEventArgs args) {
#if DEBUG
            FillDataTableArg arg = (FillDataTableArg)args.Argument;
            int sum = 0;
            for (int i = 0; i < 10; i++) {
                bool charKos = i % 3 == 2;
                bool corporationKos = i % 5 == 4;
                bool allianceKos = i % 7 == 6;
                bool kos = charKos || corporationKos || allianceKos;
                DataRow dataRow = arg.Table.NewRow();
                dataRow[0] = "character #" + i;
                dataRow[1] = charKos;
                dataRow[2] = "Corporation #" + i;
                dataRow[3] = corporationKos;
                dataRow[4] = "Alliance #" + i;
                dataRow[5] = allianceKos;
                dataRow[6] = kos;
                arg.Table.Rows.Add(dataRow);
                if (kos) {
                    ++sum;
                }
            }
            args.Result = sum;
#else
            args.Result = KosChecker.FillDataTable((FillDataTableArg) args.Argument);
#endif
        }

        private static class KosChecker
        {
            private static EveApi _Api;
            private static readonly Dictionary<long, bool> _KosCharacters = new Dictionary<long, bool>();
            private static readonly Dictionary<long, bool> _KosCorporations = new Dictionary<long, bool>();
            private static readonly Dictionary<long, bool> _KosAlliances = new Dictionary<long, bool>();

            public static int FillDataTable(FillDataTableArg arg) {
                if (_Api == null) {
                    _Api = new EveApi("eve-log-watcher", 4488664, "GvRnlFE0G2r3yX2OzHzzldNEN0BSx9DGJQgebKgfuzRO8vmrV8WVY3u3lqLadAZ8");
                }

                Dictionary<string, long> dict = _Api.ConvertNamesToIDs(arg.Names);

                int kosCounter = 0;
                foreach (KeyValuePair<string, long> pair in dict) {
                    if (pair.Value == 0) {
                        continue;
                    }

                    _Api.Authentication.CharacterID = pair.Value;
                    CharacterInfo info = _Api.GetCharacterInfo();
                    if (info.CharacterID == 0) {
                        continue;
                    }

                    DataRow dataRow = arg.Table.NewRow();
                    if (FillDataRow(dataRow, info)) {
                        kosCounter++;
                    }
                    arg.Table.Rows.Add(dataRow);
                    Thread.Sleep(50);
                }

                return kosCounter;
            }

            private static bool FillDataRow(DataRow dataRow, CharacterInfo characterInfo) {
                bool pilotKos = IsKos(characterInfo.CharacterID, characterInfo.Name, IdType.Character);
                bool corporationKos;
                bool allianceKos;
                if (IsNpcCorp(characterInfo.CorporationID)) {
                    corporationKos = false;
                    allianceKos = false;
                } else {
                    corporationKos = IsKos(characterInfo.CorporationID, characterInfo.CorporationName, IdType.Corporation);
                    allianceKos = (characterInfo.AllianceID != -1) && IsKos(characterInfo.AllianceID, characterInfo.AllianceName, IdType.Alliance);
                }
                bool kos = pilotKos || corporationKos || allianceKos;

                dataRow[0] = characterInfo.Name;
                dataRow[1] = pilotKos;
                dataRow[2] = characterInfo.CorporationName;
                dataRow[3] = corporationKos;
                dataRow[4] = characterInfo.AllianceName;
                dataRow[5] = allianceKos;
                dataRow[6] = kos;

                return kos;
            }

            private static bool IsKos(long id, string name, IdType type) {
                bool kos = false;

                CvaCharacterInfo characterInfo = null;
                CvaCorporationInfo corpInfo = null;
                CvaAllianceInfo allianceInfo = null;

                switch (type) {
                    case IdType.Character: {
                        if (!_KosCharacters.TryGetValue(id, out kos)) {
                            characterInfo = CvaClient.GetCharacterInfo(id, name);
                            kos = characterInfo?.Kos == true;
                        }
                        break;
                    }
                    case IdType.Corporation: {
                        if (!_KosCorporations.TryGetValue(id, out kos)) {
                            corpInfo = CvaClient.GetCorpInfo(id, name);
                            kos = corpInfo?.Kos == true;
                        }
                        break;
                    }
                    case IdType.Alliance: {
                        if (!_KosAlliances.TryGetValue(id, out kos)) {
                            allianceInfo = CvaClient.GetAllianceInfo(id, name);
                            kos = allianceInfo?.Kos == true;
                        }
                        break;
                    }
                }

                if (characterInfo != null && !_KosCharacters.ContainsKey(characterInfo.EveId)) {
                    _KosCharacters.Add(characterInfo.EveId, characterInfo.Kos);
                    corpInfo = characterInfo.Corp;
                }

                if (corpInfo != null && !_KosCorporations.ContainsKey(corpInfo.EveId)) {
                    _KosCorporations.Add(corpInfo.EveId, corpInfo.Kos);
                    allianceInfo = corpInfo.Alliance;
                }

                if (allianceInfo != null && !_KosAlliances.ContainsKey(allianceInfo.EveId)) {
                    _KosAlliances.Add(allianceInfo.EveId, allianceInfo.Kos);
                }

                return kos;
            }

            private static bool IsNpcCorp(long id) {
                return id >= 1000002 && id <= 1000182;
            }
        }

        private enum IdType
        {
            Character,
            Corporation,
            Alliance
        }

        #endregion
    }
}