using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using eve_log_watcher.cva_kos_api;
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

        public static FormCva ShowMe() {
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

            Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
            int titleHeight = screenRectangle.Top - Top;

            dataGridCva.Width = dataGridCva.Columns.Cast<DataGridViewColumn>().Sum(c => (c.Visible ? c.Width : 0) + c.DividerWidth)+1;
            dataGridCva.Height = dataGridCva.ColumnHeadersHeight + dataGridCva.Rows.Count * (dataGridCva.RowTemplate.Height + dataGridCva.RowTemplate.DividerHeight);
        }

        private void dataGridCva_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            Color red = Color.FromArgb(250, 172, 163);
            Color redSelected = Color.FromArgb(250, 127, 113);

            foreach (DataGridViewRow row in dataGridCva.Rows) {
                if (row.Cells[colPilotKos.Name].Value as bool? == true) {
                    row.Cells[colPilotName.Name].Style.BackColor = red;
                    row.Cells[colPilotName.Name].Style.SelectionBackColor = redSelected;
                }
                if (row.Cells[colCorporationKos.Name].Value as bool? == true) {
                    row.Cells[colCorporationName.Name].Style.BackColor = red;
                    row.Cells[colCorporationName.Name].Style.SelectionBackColor = redSelected;
                }
                if (row.Cells[colAllianceKos.Name].Value as bool? == true) {
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
#if DESIGN
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
            private static readonly Dictionary<string, CharacterInfo> _KnownCharacters = new Dictionary<string, CharacterInfo>();

            public static int FillDataTable(FillDataTableArg arg) {
                if (_Api == null) {
                    _Api = new EveApi("eve-log-watcher", 4488664, "GvRnlFE0G2r3yX2OzHzzldNEN0BSx9DGJQgebKgfuzRO8vmrV8WVY3u3lqLadAZ8");
                }

                List<CharacterInfo> characters = new List<CharacterInfo>();
                List<string> unknowns = new List<string>();
                foreach (string name in arg.Names) {
                    CharacterInfo characterInfo;
                    if (!_KnownCharacters.TryGetValue(name, out characterInfo)) {
                        unknowns.Add(name);
                    } else {
                        characters.Add(characterInfo);
                    }
                }
                if (unknowns.Count > 0) {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://kos.cva-eve.org/api/?c=json&type=multi&q=" + string.Join(",", unknowns));
                    JObject jObject;
                    using (WebResponse response = request.GetResponse()) {
                        using (Stream stream = response.GetResponseStream()) {
                            Debug.Assert(stream != null);
                            using (StreamReader reader = new StreamReader(stream)) {
                                using (JsonTextReader jReader = new JsonTextReader(reader)) {
                                    jObject = JObject.Load(jReader);
                                }
                            }
                        }
                    }

                    JToken jMessage = jObject["message"];
                    if (jMessage.Value<string>() != "OK") {
                        throw new Exception("error message from kos.cva-eve.org: " + jMessage.Value<string>());
                    }

                    JArray jResults = (JArray)jObject["results"];
                    foreach (JToken jResult in jResults) {
                        CharacterInfo info = new CharacterInfo(jResult);
                        _KnownCharacters.Add(info.PilotName, info);
                        characters.Add(info);
                    }

                    unknowns = unknowns.Where(o => !_KnownCharacters.ContainsKey(o)).ToList();
                }

                int kosCounter = 0;
                foreach (CharacterInfo characterInfo in characters) {
                    DataRow dataRow = arg.Table.NewRow();
                    characterInfo.FillDataRow(dataRow);
                    if (characterInfo.Kos) {
                        kosCounter++;
                    }
                    arg.Table.Rows.Add(dataRow);

                }

                foreach (string unknown in unknowns) {
                    DataRow dataRow = arg.Table.NewRow();
                    dataRow[0] = unknown;
                    dataRow[2] = "???";
                    arg.Table.Rows.Add(dataRow);
                }

                return kosCounter;
            }
        }

        private class CharacterInfo
        {
            public CharacterInfo(JToken jObject) {
                PilotName = jObject["label"].Value<string>();
                PilotKos = jObject["kos"].Value<bool>();

                JToken jCorp = jObject["corp"];
                CorporationName = jCorp["label"].Value<string>();
                CorporationKos = jCorp["kos"].Value<bool>();

                JToken jAlliace = jCorp["alliance"];
                if (jAlliace != null) {
                    AllianceName = jAlliace["label"].Value<string>();
                    AllianceKos = jAlliace["kos"].Value<bool>();
                }
            }

            public string PilotName { get;  }
            public string CorporationName { get;  }
            public string AllianceName { get;  }

            public bool PilotKos { get;  }
            public bool CorporationKos { get;  }
            public bool AllianceKos { get;  }

            public bool Kos => PilotKos || CorporationKos || AllianceKos;

            public void FillDataRow(DataRow dataRow) {
                dataRow[0] = PilotName;
                dataRow[1] = PilotKos;
                dataRow[2] = CorporationName;
                dataRow[3] = CorporationKos;
                dataRow[4] = AllianceName;
                dataRow[5] = AllianceKos;
                dataRow[6] = Kos;
            }
        }

        #endregion
    }
}