using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using eve_log_watcher.Properties;

namespace eve_log_watcher.controls
{
    public sealed class LogWatcher : Timer
    {
        private static readonly string _LogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"EVE\logs\Chatlogs");
        private long _LastLogSize;
        private FileInfo _LogFile;

        public LogWatcher() {
            Interval = 250;
            Tick += OnTick;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public string LogName { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public ISynchronizeInvoke SynchronizingObject { get; set; }

        public static string[] GetLogNames() {
            try {
                IEnumerable<string> q = from filePath in Directory.EnumerateFiles(_LogsPath, "*.txt")
                                        let fileName = Path.GetFileName(filePath)
                                        let match = Regex.Match(fileName, @"(.*?)_\d+_\d+\.txt$", RegexOptions.Compiled | RegexOptions.IgnoreCase)
                                        where match.Success
                                        select match.Groups[1].Value;


                if (!string.IsNullOrEmpty(Settings.Default.intelLogName)) {
                    q = q.Union(new[] { Settings.Default.intelLogName });
                }

                return q.Distinct().ToArray();
            } catch{
                MessageBox.Show("Unable to find EVE log files. Please run EVE first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return new string[0];
            }
        }

        private void OnTick(object sender, EventArgs eventArgs) {
            FileInfo info = new FileInfo(_LogFile.FullName);
            if (info.Length == _LastLogSize) {
                return;
            }

            Enabled = false;
            try {
                lock (this) {
                    string newData;
                    try {
                        using (FileStream stream = new FileStream(_LogFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                            using (StreamReader reader = new StreamReader(stream, Encoding.Unicode)) {
                                stream.Seek(_LastLogSize, SeekOrigin.Begin);
                                newData = reader.ReadToEnd();
                            }
                        }
                    } catch {
                        return;
                    }
                    _LastLogSize = info.Length;
                    string[] lines = newData.Split(new[] {
                        Environment.NewLine
                    }, StringSplitOptions.RemoveEmptyEntries);
                    lines = lines.Select(line => line.Trim()).Where(line => line.Length > 0).ToArray();
                    if (lines.Length == 0) {
                        return;
                    }
                    OnProcessNewData(lines);
                }
            } finally {
                Enabled = true;
            }
        }

        public bool StartWatch() {
            if (Enabled) {
                return true;
            }

            IOrderedEnumerable<FileInfo> q = from f in new DirectoryInfo(_LogsPath).EnumerateFiles(LogName + "*.txt")
                                             orderby f.LastWriteTime descending
                                             select f;

            _LogFile = q.FirstOrDefault();
            if (_LogFile == null) {
                return false;
            }

            _LastLogSize = _LogFile.Length;
            Enabled = true;
            return true;
        }

        public void StopWatch() {
            Enabled = false;
        }

        public event EventHandler<ProcessNewDataEventArgs> ProcessNewData;

        private void OnProcessNewData(string[] lines) {
            ProcessNewData?.Invoke(this, new ProcessNewDataEventArgs(lines));
        }
    }

    public class ProcessNewDataEventArgs : EventArgs
    {
        public ProcessNewDataEventArgs(string[] lines) {
            Lines = lines;
        }

        public string[] Lines { get; }
    }
}
