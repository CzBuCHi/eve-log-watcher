using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;

namespace eve_log_watcher.controls
{
    public partial class Map : UserControl
    {
        private readonly Dictionary<string, Node> _Nodes = new Dictionary<string, Node>();
        private readonly Dictionary<string, DateTime> _RedInfo = new Dictionary<string, DateTime>();
        private string _CurrentSystemName;

        public Map() {
            InitializeComponent();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public int RedShowDuration { get; set; } = 10;

        public string CurrentSystemName {
            // ReSharper disable once UnusedMember.Global
            get { return _CurrentSystemName; }
            set {
                if (_CurrentSystemName == value) {
                    return;
                }
                _CurrentSystemName = value;
                UpdateGraph();
            }
        }

        public Dictionary<string, DateTime> RedInfo {
            get {
                lock (_RedInfo) {
                    return _RedInfo;
                }
            }
        }

        public void UpdateNodes() {
            gViewer.SuspendLayout();
            try {
                lock (_Nodes) {
                    foreach (Node node in _Nodes.Values) {
                        node.Attr.FillColor = Color.Transparent;
                    }
                    foreach (KeyValuePair<string, DateTime> pair in RedInfo) {
                        Node node;
                        if (_Nodes.TryGetValue(pair.Key, out node)) {
                            node.Attr.FillColor = Color.LightPink;
                        }
                    }
                }
            } finally {
                gViewer.ResumeLayout(true);
                gViewer.Invalidate();
            }
        }

        private void UpdateGraph() {
            Func<IEnumerable<SystemInfo>, IEnumerable<SystemInfo>> getConnected = s => from o in s
                                                                                       from o2 in from o3 in DbHelper.DataContext.SolarSystemJumps
                                                                                                  join o4 in DbHelper.DataContext.SolarSystems on o3.ToSolarsystemId equals o4.Id
                                                                                                  where o3.FromSolarsystemId == o.Id
                                                                                                  select o4
                                                                                       select new SystemInfo { Id = o2.Id, PrevId = o.Id, Name = o2.SolarsystemName };

            IEnumerable<SystemInfo> current = from o in DbHelper.DataContext.SolarSystems
                                              where o.SolarsystemName == _CurrentSystemName
                                              select new SystemInfo { Id = o.Id, Name = o.SolarsystemName };

            current = current.ToArray();
            SystemInfo[] oneJump = getConnected(current).ToArray();
            SystemInfo[] twoJumps = getConnected(oneJump).ToArray();
            SystemInfo[] treeJumps = getConnected(twoJumps).ToArray();
            SystemInfo[] fourJumps = getConnected(treeJumps).ToArray();
            SystemInfo[] fiveJumps = getConnected(fourJumps).ToArray();

            Dictionary<int, SystemInfo> infos = current.Union(oneJump).Union(twoJumps).Union(treeJumps).Union(fourJumps).Union(fiveJumps).ToDictionary(o => o.Id);

            SystemInfo[][] q = { (SystemInfo[]) current, oneJump, twoJumps, treeJumps, fourJumps, fiveJumps };

            gViewer.Graph = null;
            Graph graph = new Graph("graph");

            Dictionary<string, HashSet<string>> dict = new Dictionary<string, HashSet<string>>();

            lock (_Nodes) {
                _Nodes.Clear();

                foreach (SystemInfo[] qq in q) {
                    foreach (SystemInfo info in qq) {
                        if (!_Nodes.ContainsKey(info.Name)) {
                            Node node = graph.AddNode(info.Name);
                            _Nodes.Add(info.Name, node);
                        }

                        if (info.PrevId == null) {
                            continue;
                        }

                        SystemInfo prev = infos[info.PrevId.Value];

                        HashSet<string> list;
                        if (!dict.TryGetValue(prev.Name, out list)) {
                            list = new HashSet<string>();
                            dict[prev.Name] = list;
                        }
                        if (!list.Contains(info.Name)) {
                            list.Add(info.Name);
                        }
                    }
                }
            }

            HashSet<string> edges = new HashSet<string>();
            foreach (KeyValuePair<string, HashSet<string>> pair in dict) {
                foreach (string item in pair.Value) {
                    if (edges.Contains(item + "-" + pair.Key)) {
                        continue;
                    }

                    edges.Add(pair.Key + "-" + item);
                    Edge edge = graph.AddEdge(pair.Key, item);
                    edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                }
            }

            gViewer.Graph = graph;
            Height = (int) (Width * gViewer.GraphHeight / gViewer.GraphWidth);
            UpdateNodes();
        }

        private void Map_SizeChanged(object sender, EventArgs e) {
            if (gViewer.Graph != null) {
                Height = (int) (Width * gViewer.GraphHeight / gViewer.GraphWidth);
            }
        }

        private void timerCleaner_Tick(object sender, EventArgs e) {
            string[] trash;
            lock (_RedInfo) {
                DateTime now = DateTime.Now;
                TimeSpan delta = TimeSpan.FromSeconds(RedShowDuration);
                IEnumerable<string> q = from pair in _RedInfo
                                        where now - pair.Value > delta
                                        select pair.Key;

                trash = q.ToArray();

                foreach (string key in trash) {
                    _RedInfo.Remove(key);
                }
            }
            if (trash.Length > 0) {
                UpdateNodes();
            }
        }

        [DebuggerDisplay("{Name}")]
        private class SystemInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? PrevId { get; set; }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) {
                    return false;
                }
                if (ReferenceEquals(this, obj)) {
                    return true;
                }
                SystemInfo second = obj as SystemInfo;
                return second != null && Id == second.Id;
            }

            public override int GetHashCode() {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return Id;
            }
        }
    }
}
