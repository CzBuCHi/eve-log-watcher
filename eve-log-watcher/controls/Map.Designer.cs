namespace eve_log_watcher.controls
{
    partial class Map
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.timerCleaner = new System.Windows.Forms.Timer(this.components);
            this.gViewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.SuspendLayout();
            // 
            // timerCleaner
            // 
            this.timerCleaner.Enabled = true;
            this.timerCleaner.Interval = 250;
            this.timerCleaner.Tick += new System.EventHandler(this.timerCleaner_Tick);
            // 
            // gViewer
            // 
            this.gViewer.ArrowheadLength = 10D;
            this.gViewer.AsyncLayout = false;
            this.gViewer.AutoScroll = true;
            this.gViewer.BackwardEnabled = false;
            this.gViewer.BuildHitTree = false;
            this.gViewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
            this.gViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gViewer.FileName = "";
            this.gViewer.ForwardEnabled = false;
            this.gViewer.Graph = null;
            this.gViewer.InsertingEdge = false;
            this.gViewer.LayoutAlgorithmSettingsButtonVisible = false;
            this.gViewer.LayoutEditingEnabled = false;
            this.gViewer.Location = new System.Drawing.Point(0, 0);
            this.gViewer.LooseOffsetForRouting = 0.25D;
            this.gViewer.MouseHitDistance = 0.05D;
            this.gViewer.Name = "gViewer";
            this.gViewer.NavigationVisible = false;
            this.gViewer.NeedToCalculateLayout = true;
            this.gViewer.OffsetForRelaxingInRouting = 0.6D;
            this.gViewer.PaddingForEdgeRouting = 8D;
            this.gViewer.PanButtonPressed = false;
            this.gViewer.SaveAsImageEnabled = false;
            this.gViewer.SaveAsMsaglEnabled = false;
            this.gViewer.SaveButtonVisible = false;
            this.gViewer.SaveGraphButtonVisible = false;
            this.gViewer.SaveInVectorFormatEnabled = false;
            this.gViewer.Size = new System.Drawing.Size(150, 150);
            this.gViewer.TabIndex = 0;
            this.gViewer.TightOffsetForRouting = 0.125D;
            this.gViewer.ToolBarIsVisible = false;
            this.gViewer.WindowZoomButtonPressed = false;
            this.gViewer.ZoomF = 1D;
            this.gViewer.ZoomFraction = 0.5D;
            this.gViewer.ZoomWhenMouseWheelScroll = false;
            this.gViewer.ZoomWindowThreshold = 0.05D;
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gViewer);
            this.Name = "Map";
            this.SizeChanged += new System.EventHandler(this.Map_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer;
        private System.Windows.Forms.Timer timerCleaner;
    }
}
