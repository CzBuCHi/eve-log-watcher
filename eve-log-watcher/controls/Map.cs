﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using eve_log_watcher.model;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;

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
        [DefaultValue(10)]
        public int RedShowDuration { get; set; } = 10;

        [DefaultValue(20)]
        public int MaxVisibleSystems { get; set; } = 20;

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
                        node.Attr.FillColor = node.LabelText == CurrentSystemName ? Color.GreenYellow : Color.Transparent;
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

        public void UpdateGraph() {
            Dictionary<int, SystemInfo> infos;
            SystemInfo[][] q;
            GetSystems(out infos, out q);

            gViewer.Graph = null;
            Graph graph = new Graph("graph") {
                LayoutAlgorithmSettings = new MdsLayoutSettings {
                    EdgeRoutingSettings = new EdgeRoutingSettings {
                        EdgeRoutingMode = EdgeRoutingMode.Spline,
                        RouteMultiEdgesAsBundles = false,
                        BendPenalty = 50,
                        IncrementalRoutingThreshold = 10,
                    },
                    IterationsWithMajorization = 10,
                    AdjustScale = true
                }
            };

            Dictionary<string, HashSet<string>> dict = new Dictionary<string, HashSet<string>>();

            lock (_Nodes) {
                _Nodes.Clear();
                foreach (SystemInfo info in q.SelectMany(o => o)) {
                    
                    if (!_Nodes.ContainsKey(info.Name)) {
                        Node node = graph.AddNode(info.Name);
                        node.IsVisible = _Nodes.Count < MaxVisibleSystems;
                        _Nodes.Add(info.Name, node);
                    }

                    if (info.Parent == null) {
                        continue;
                    }

                    HashSet<string> list;
                    if (!dict.TryGetValue(info.Parent.Name, out list)) {
                        list = new HashSet<string>();
                        dict[info.Parent.Name] = list;
                    }
                    if (!list.Contains(info.Name)) {
                        list.Add(info.Name);
                    }

                    foreach (SystemInfo child in info.List) {
                        if (!dict.TryGetValue(info.Name, out list)) {
                            list = new HashSet<string>();
                            dict[info.Name] = list;
                        }
                        if (!list.Contains(child.Name) && _Nodes.ContainsKey(child.Name)) {
                            list.Add(child.Name);
                        }
                    }
                }
            }

            int fakeNodeIndex = 0;
            HashSet<string> edges = new HashSet<string>();
            foreach (KeyValuePair<string, HashSet<string>> pair in dict) {
                foreach (string item in pair.Value) {
                    if (edges.Contains(item + "-" + pair.Key)) {
                        continue;
                    }

                    lock (_Nodes) {
                        if (_Nodes[pair.Key].IsVisible) {
                            string targetId = item;
                            if (!_Nodes[item].IsVisible) {
                                Node node = graph.AddNode((++fakeNodeIndex).ToString());
                                node.IsVisible = false;
                                targetId = node.Id;
                            }

                            edges.Add(pair.Key + "-" + item);
                            Edge edge = graph.AddEdge(pair.Key, targetId);
                            edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                        }
                    }
                }
            }

            gViewer.Graph = graph;
            Height = (int) (Width*gViewer.GraphHeight/gViewer.GraphWidth);
            UpdateNodes();
        }

        private IEnumerable<SystemInfo> GetConnected(IEnumerable<SystemInfo> prev) {
            foreach (SystemInfo o in prev) {
                IQueryable<SolarSystem> q = from o3 in DbHelper.DataContext.SolarSystemJumps
                                            join o4 in DbHelper.DataContext.SolarSystems on o3.ToSolarsystemId equals o4.Id
                                            where o3.FromSolarsystemId == o.Id
                                            select o4;

                foreach (SolarSystem o2 in q) {
                    SystemInfo info = new SystemInfo {
                        Id = o2.Id,
                        Parent = o,
                        Name = o2.SolarSystemName
                    };
                    o.List.Add(info);
                    yield return info;
                }
            }
        }

        private void GetSystems(out Dictionary<int, SystemInfo> infos, out SystemInfo[][] systemsByJumps) {
            IEnumerable<SystemInfo> current = from o in DbHelper.DataContext.SolarSystems
                                              where o.SolarSystemName == _CurrentSystemName
                                              select new SystemInfo {
                                                  Id = o.Id,
                                                  Name = o.SolarSystemName
                                              };

            current = current.ToArray();

            List<SystemInfo[]> jumps = new List<SystemInfo[]> {(SystemInfo[]) current};
            SystemInfo[] total = current.ToArray();
            int overflow = 0;
            while (overflow < 2) {
                SystemInfo[] next = GetConnected(current).Distinct().ToArray();
                total = total.Union(next).ToArray();
                jumps.Add(next);
                current = next;

                if (total.Length > MaxVisibleSystems) {
                    ++overflow;
                }
            }

            infos = total.ToDictionary(o => o.Id);
            systemsByJumps = jumps.ToArray();
        }

        private void Map_SizeChanged(object sender, EventArgs e) {
            if (gViewer.Graph != null) {
                Height = (int) (Width*gViewer.GraphHeight/gViewer.GraphWidth);
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

        private void gViewer_Load(object sender, EventArgs e) {
        }

        [DebuggerDisplay("{Name}")]
        private class SystemInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public SystemInfo Parent { get; set; }
            public List<SystemInfo> List { get; } = new List<SystemInfo>();

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
