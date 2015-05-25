using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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

        private class FillDataTableArg
        {
            public DataTable Table { get; set; }
            public string Names { get; set; }
        }

        private static bool _FillingDataTable = false;
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

            string names = Clipboard.GetText().Replace("\r\n", "%0D%0A");
            worker.RunWorkerAsync(new FillDataTableArg { Table = _Form._DataTable, Names = names });
        }

        private static void FillDataTable(object sender, DoWorkEventArgs args) {
            FillDataTableArg arg = (FillDataTableArg) args.Argument;

            // request
            WebRequest request = WebRequest.CreateHttp("http://local.cva-eve.org/index.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            
            using (Stream stream = request.GetRequestStream()) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write("searchRows=" + arg.Names);
                }
            }

            // reads whole html
            string html;
            using (WebResponse response = request.GetResponse()) {
                using (Stream stream = response.GetResponseStream()) {
                    Debug.Assert(stream != null);
                    using (StreamReader reader = new StreamReader(stream)) {
                        html = reader.ReadToEnd();
                    }
                }
            }

            // extract table
            int start = html.IndexOf("<div class=\"col-xs-9\">", StringComparison.Ordinal) + 22;
            while (char.IsWhiteSpace(html[start])) {
                ++start;
            }
            int end = html.IndexOf("</table>", start, StringComparison.Ordinal) + 8;
            string table = html.Substring(start, end - start).Replace("&nbsp;", " ");

            // removes attributes except class
            table = Regex.Replace(table, @"<(\w*)\s*([^>]*)>", match => {
                if (!string.IsNullOrEmpty(match.Groups[2].Value)) {
                    return "<" + match.Groups[1].Value + " " + Regex.Replace(match.Groups[2].Value, "\\w*(?<!class)=\"[^\"]*\"", "") + ">";
                }
                return match.Value;
            });

            // fixes < /tag> --> </tag>
            table = Regex.Replace(table, @"<\s*/", "</");

            // removes <img> tags
            table = Regex.Replace(table, "<img[^>]*>", "");

            // add missing </tr>
            table = Regex.Replace(table, "<tr[^>]*>(?:(?!</?tr>|</tbody>|</table>).)*?(?=<tr[^>]*>|</tbody>|</table>)", "$&</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // add missing </td>
            table = Regex.Replace(table, "<td[^>]*>(?:(?!</?td>|</tr>|</tbody>|</table>).)*?(?=<td[^>]*>|</tr>|</tbody>|</table>)", "$&</td>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            XElement xTable = XElement.Parse(table);

            XElement[] xTrs = xTable.Elements("tr").Skip(1).ToArray();

            arg.Table.Rows.Clear();

            int kosCounter = 0;
            foreach (XElement xTr in xTrs) {
                XElement[] xTds = xTr.Elements("td").ToArray();
                if (xTds.Length == 4) {
                    bool pilotKos = GetIsKos(xTds[1]);
                    bool corporationKos = GetIsKos(xTds[2]);
                    bool allianceKos = GetIsKos(xTds[3]);
                    bool isKos = pilotKos || corporationKos || allianceKos;
                    if (isKos) {
                        ++kosCounter;
                    }
                    arg.Table.Rows.Add(GetValue(xTds[1]), pilotKos, GetValue(xTds[2]), corporationKos, GetValue(xTds[3]), allianceKos, isKos);
                }
            }
            args.Result = kosCounter;
        }

        private void AfterFillDataTable(int kosCount) {
            labelLoading.Visible = false;
            dataGridCva.Visible = true;

            Text = @"KOS: " + kosCount;
            _Form.dataGridCva.DataSource = _Form._DataTable;
            
            Height = Math.Min(dataGridCva.Rows.Count * (dataGridCva.RowTemplate.Height + dataGridCva.RowTemplate.DividerHeight), 500) + 70;
            Width = Math.Max(dataGridCva.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Sum(c => c.Width + c.DividerWidth), 300) + 40;
        }

        private static string GetValue(XElement xElement) {
            if (xElement.HasElements) {
                return string.Concat(xElement.DescendantNodes().Where(node => node.NodeType == XmlNodeType.Text));
            }
            return xElement.Value;
        }

        private static bool GetIsKos(XElement xElement) {
            XElement xButton = xElement.Element("button");
            if (xButton == null) {
                return false;
            }
            return xButton.Attribute("class").Value.IndexOf("btn-danger", StringComparison.Ordinal) != -1;
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
    }
}
