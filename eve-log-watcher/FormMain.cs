using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using eve_log_watcher.controls;
using eve_log_watcher.model;
using eve_log_watcher.Properties;

namespace eve_log_watcher
{
    public partial class FormMain : Form
    {
        private readonly DataTable _CitadelLogs;
        private readonly KeyboardHook _Hook = new KeyboardHook();
        private FormCva _FormCva;
        private bool _ComboBoxSystemsSetValue;

        public FormMain() {
            InitializeComponent();

            Rectangle bounds = Screen.AllScreens.Last().Bounds;
            Top = bounds.Top;
            Left = bounds.Left;
            Width = bounds.Width;


            _CitadelLogs = new DataTable();
            _CitadelLogs.Columns.Add("Text", typeof (string));
            _CitadelLogs.Columns.Add("System", typeof (string));

            dataGridIntel.DataSource = _CitadelLogs;

            _Hook.RegisterHotKey(eve_log_watcher.ModifierKeys.Control, Keys.F);
            _Hook.KeyPressed += HookOnKeyPressed;

            logWatcherIntel.LogName = Settings.Default.intelLogName;

            comboLogs.DataSource = LogWatcher.GetLogNames();
            comboLogs.SelectedItem = Settings.Default.intelLogName;
            comboLogs.SelectedIndexChanged += comboLogs_SelectedIndexChanged;

            _ComboBoxSystemsSetValue = true;
            comboBoxSystems.DataSource = DbHelper.DataContext.SolarSystems.ToArray();
            _ComboBoxSystemsSetValue = false;
            if (Settings.Default.currentSystemId == 0) {
                labelCurentSystem.Visible = false;
                labelWarning.Visible = true;                
            } else {
                map.CurrentSystemName = DbHelper.DataContext.SolarSystems.Where(o => o.Id == Settings.Default.currentSystemId).Select(o => o.SolarSystemName).FirstOrDefault();
                labelCurentSystem.Text = map.CurrentSystemName;
                labelCurentSystem.Visible = true;
                labelWarning.Visible = false;
                _ComboBoxSystemsSetValue = true;
                comboBoxSystems.SelectedText = map.CurrentSystemName;
                _ComboBoxSystemsSetValue = false;
            }
        }

        private void HookOnKeyPressed(object sender, KeyPressedEventArgs keyPressedEventArgs) {
            _FormCva = FormCva.ShowMe();
            if (_FormCva != null) {
                _FormCva.Top = Bottom;
                _FormCva.Left = Left;
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            buttonStop_Click(null, null);
            Settings.Default.Save();
            if (_FormCva != null) {
                _FormCva.CanBeClosed = true;
                _FormCva.Close();
            }
            base.OnClosing(e);
        }

        private void logWatcherLocal_ProcessNewData(object sender, ProcessNewDataEventArgs e) {
            string currentSystem = null;
            foreach (string line in e.Lines) {
                int index = line.IndexOf("Channel changed to Local", StringComparison.OrdinalIgnoreCase);
                if (index != -1) {
                    index = line.TrimEnd().LastIndexOf(' ');
                    currentSystem = line.Substring(index + 1);
                }
            }
            if (currentSystem != null) {
                IQueryable<int?> q = from o in DbHelper.DataContext.SolarSystems
                                     where o.SolarSystemName == currentSystem
                                     select (int?) o.Id;

                int? currentSystemId = q.FirstOrDefault();
                if (currentSystemId != null) {
                    Settings.Default.currentSystemId = currentSystemId.Value;
                    map.CurrentSystemName = currentSystem;
                    labelCurentSystem.Text = map.CurrentSystemName;
                    _ComboBoxSystemsSetValue = true;
                    comboBoxSystems.SelectedText = map.CurrentSystemName;
                    _ComboBoxSystemsSetValue = false; 
                    labelCurentSystem.Visible = true;
                    labelWarning.Visible = false;
                }                
            }
        }

        private void logWatcherIntel_ProcessNewData(object sender, ProcessNewDataEventArgs e) {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += WorkerOnDoWork;
            worker.RunWorkerCompleted += (o, args) => {
                dataGridIntel.Refresh();
                dataGridIntel.ClearSelection();
                DataGridViewRow row = dataGridIntel.Rows.OfType<DataGridViewRow>().LastOrDefault();
                if (row != null) {
                    row.Selected = true;
                    dataGridIntel.FirstDisplayedScrollingRowIndex = row.Index;
                }
                List<string> result = (List<string>) args.Result;
                DateTime now = DateTime.Now;
                foreach (string item in result) {
                    map.RedInfo[item] = now;
                }
                map.UpdateNodes();
            };
            worker.RunWorkerAsync(new LogWatcherIntelProcessNewDataArg {
                Lines = e.Lines,
                Logs = _CitadelLogs
            });
        }

        private void buttonStart_Click(object sender, EventArgs e) {
            if (logWatcherIntel.LogName == null) {
                MessageBox.Show(@"Please select Intel channel...");
                return;
            }
            if (!logWatcherIntel.StartWatch() || !logWatcherLocal.StartWatch()) {
                MessageBox.Show(@"Missing log files - start EVE to create them.");
                return;
            }

            buttonStart.Enabled = false;
            comboLogs.Enabled = false;
            buttonStop.Enabled = true;
            buttonRefresh.Enabled = false;
        }

        private void buttonStop_Click(object sender, EventArgs e) {
            logWatcherIntel.StopWatch();
            logWatcherLocal.StopWatch();

            buttonStop.Enabled = false;
            comboLogs.Enabled = true;
            buttonStart.Enabled = true;
            buttonRefresh.Enabled = true;
        }

        private static void WorkerOnDoWork(object sender, DoWorkEventArgs args) {
            LogWatcherIntelProcessNewDataArg arg = (LogWatcherIntelProcessNewDataArg) args.Argument;

            List<string> systems = new List<string>();
            foreach (string line in arg.Lines) {
                if (line.IndexOf("EVE System", StringComparison.OrdinalIgnoreCase) != -1) {
                    continue;
                }
                int index = line.IndexOf('>');
                string text = line.Substring(index + 1).TrimStart();
                string[] solarSystems = DbHelper.EnumerateSolarsystemsInText(text).ToArray();
                arg.Logs.Rows.Add(text, string.Join(", ", solarSystems));

                systems.AddRange(solarSystems);
            }
            args.Result = systems;
        }

        private void timerTime_Tick(object sender, EventArgs e) {
            labelTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void comboLogs_SelectedIndexChanged(object sender, EventArgs e) {
            Settings.Default.intelLogName = (string) comboLogs.SelectedItem;
            logWatcherIntel.LogName = Settings.Default.intelLogName;
        }

        private class LogWatcherIntelProcessNewDataArg
        {
            public string[] Lines { get; set; }
            public DataTable Logs { get; set; }
        }

        private void buttonRefresh_Click(object sender, EventArgs e) {
            string selected = (string) comboLogs.SelectedItem;
            string[] logNames = LogWatcher.GetLogNames();
            comboLogs.DataSource = logNames;
            if (logNames.Contains(selected)) {
                comboLogs.SelectedItem = selected;
            }
        }

        private void map_SizeChanged(object sender, EventArgs e) {
            ClientSize = new Size(ClientSize.Width, map.Height + panelTop.Height);
        }

        private void comboBoxSystems_SelectedIndexChanged(object sender, EventArgs e) {
            if (_ComboBoxSystemsSetValue || comboBoxSystems.SelectedIndex == -1) {
                return;
            }
            SolarSystem solarSystem = (SolarSystem) comboBoxSystems.SelectedItem;
            Settings.Default.currentSystemId = solarSystem.Id;

            map.CurrentSystemName = solarSystem.SolarSystemName;
            labelCurentSystem.Text = solarSystem.SolarSystemName;
            labelCurentSystem.Visible = true;
            labelWarning.Visible = false;
        }
    }
}
