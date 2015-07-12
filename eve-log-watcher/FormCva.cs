using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using eve_log_watcher.cva_kos_api;
using EveAI.Live;
using EveAI.Live.Character;

namespace eve_log_watcher
{
    public partial class FormCva : Form
    {
        private static readonly FormCva _Form = new FormCva();
        private readonly DataTable _DataTable;

        private FormCva() {
            InitializeComponent();
            _DataTable = new DataTable();
            _DataTable.Columns.Add("PilotName", typeof (string));
            _DataTable.Columns.Add("PilotKos", typeof (bool));
            _DataTable.Columns.Add("CorporationName", typeof (string));
            _DataTable.Columns.Add("CorporationKos", typeof (bool));
            _DataTable.Columns.Add("AllianceName", typeof (string));
            _DataTable.Columns.Add("AllianceKos", typeof (bool));
            _DataTable.Columns.Add("Kos", typeof (bool));

            dataGridCva.DataSource = _DataTable;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool CanBeClosed { get; set; }

        public static FormCva ShowMe() {
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
            _Form.dataGridCva.DataSource = _Form._DataTable;

            Height = Math.Min(dataGridCva.Rows.Count*(dataGridCva.RowTemplate.Height + dataGridCva.RowTemplate.DividerHeight), 500) + 70;
            Width = Math.Max(dataGridCva.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Sum(c => c.Width + c.DividerWidth), 300) + 40;
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

            DataGridViewColumn column = dataGridCva.Columns[colKos.Name];
            Debug.Assert(column != null);
            dataGridCva.Sort(column, ListSortDirection.Descending);
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
            _Form._DataTable.Rows.Clear();
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
                Table = _Form._DataTable,
                Names = names
            });
        }

        private static void FillDataTable(object sender, DoWorkEventArgs args) {
            args.Result = KosChecker.FillDataTable((FillDataTableArg) args.Argument);
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
                    allianceKos = (characterInfo.AllianceID == -1) ? false : IsKos(characterInfo.AllianceID, characterInfo.AllianceName, IdType.Alliance);
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
                            kos = characterInfo.Kos;
                        }
                        break;
                    }
                    case IdType.Corporation: {
                        if (!_KosCorporations.TryGetValue(id, out kos)) {
                            corpInfo = CvaClient.GetCorpInfo(id, name);
                            kos = corpInfo.Kos;
                        }
                        break;
                    }
                    case IdType.Alliance: {
                        if (!_KosAlliances.TryGetValue(id, out kos)) {
                            allianceInfo = CvaClient.GetAllianceInfo(id, name);
                            kos = allianceInfo.Kos;
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