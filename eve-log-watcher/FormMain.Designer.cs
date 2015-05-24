using eve_log_watcher.controls;

namespace eve_log_watcher
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelChannels = new System.Windows.Forms.Label();
            this.labelCurentSystem = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.comboLogs = new System.Windows.Forms.ComboBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.dataGridIntel = new System.Windows.Forms.DataGridView();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.map = new eve_log_watcher.controls.Map();
            this.logWatcherLocal = new eve_log_watcher.controls.LogWatcher();
            this.logWatcherIntel = new eve_log_watcher.controls.LogWatcher();
            this.panelTop.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIntel)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(8, 8);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(80, 25);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(94, 8);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(80, 25);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonRefresh);
            this.panelTop.Controls.Add(this.labelChannels);
            this.panelTop.Controls.Add(this.labelCurentSystem);
            this.panelTop.Controls.Add(this.labelWarning);
            this.panelTop.Controls.Add(this.comboLogs);
            this.panelTop.Controls.Add(this.labelTime);
            this.panelTop.Controls.Add(this.buttonStart);
            this.panelTop.Controls.Add(this.buttonStop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1276, 39);
            this.panelTop.TabIndex = 4;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.BackgroundImage = global::eve_log_watcher.Properties.Resources.refresh;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.Location = new System.Drawing.Point(1243, 8);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(21, 21);
            this.buttonRefresh.TabIndex = 9;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // labelChannels
            // 
            this.labelChannels.AutoSize = true;
            this.labelChannels.Location = new System.Drawing.Point(875, 11);
            this.labelChannels.Name = "labelChannels";
            this.labelChannels.Size = new System.Drawing.Size(54, 13);
            this.labelChannels.TabIndex = 8;
            this.labelChannels.Text = "Channels:";
            // 
            // labelCurentSystem
            // 
            this.labelCurentSystem.AutoSize = true;
            this.labelCurentSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCurentSystem.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelCurentSystem.Location = new System.Drawing.Point(180, 12);
            this.labelCurentSystem.Name = "labelCurentSystem";
            this.labelCurentSystem.Size = new System.Drawing.Size(114, 17);
            this.labelCurentSystem.TabIndex = 7;
            this.labelCurentSystem.Text = "CurrentSystem";
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelWarning.ForeColor = System.Drawing.Color.DarkRed;
            this.labelWarning.Location = new System.Drawing.Point(180, 12);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(359, 17);
            this.labelWarning.TabIndex = 6;
            this.labelWarning.Text = "Dont know where u are ... please use stargate :)";
            // 
            // comboLogs
            // 
            this.comboLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLogs.FormattingEnabled = true;
            this.comboLogs.Location = new System.Drawing.Point(948, 8);
            this.comboLogs.Name = "comboLogs";
            this.comboLogs.Size = new System.Drawing.Size(289, 21);
            this.comboLogs.TabIndex = 5;
            // 
            // labelTime
            // 
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTime.Location = new System.Drawing.Point(775, 9);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(94, 24);
            this.labelTime.TabIndex = 4;
            this.labelTime.Text = "00:00:00";
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.dataGridIntel);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(875, 39);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(401, 384);
            this.panelRight.TabIndex = 5;
            // 
            // dataGridIntel
            // 
            this.dataGridIntel.AllowUserToAddRows = false;
            this.dataGridIntel.AllowUserToDeleteRows = false;
            this.dataGridIntel.AllowUserToOrderColumns = true;
            this.dataGridIntel.AllowUserToResizeColumns = false;
            this.dataGridIntel.AllowUserToResizeRows = false;
            this.dataGridIntel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridIntel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colText,
            this.colSystem});
            this.dataGridIntel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridIntel.EnableHeadersVisualStyles = false;
            this.dataGridIntel.Location = new System.Drawing.Point(0, 0);
            this.dataGridIntel.MultiSelect = false;
            this.dataGridIntel.Name = "dataGridIntel";
            this.dataGridIntel.ReadOnly = true;
            this.dataGridIntel.RowHeadersVisible = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridIntel.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridIntel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridIntel.Size = new System.Drawing.Size(401, 384);
            this.dataGridIntel.TabIndex = 4;
            // 
            // colText
            // 
            this.colText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colText.DataPropertyName = "Text";
            this.colText.HeaderText = "Text";
            this.colText.Name = "colText";
            this.colText.ReadOnly = true;
            // 
            // colSystem
            // 
            this.colSystem.DataPropertyName = "System";
            this.colSystem.HeaderText = "System";
            this.colSystem.Name = "colSystem";
            this.colSystem.ReadOnly = true;
            // 
            // timerTime
            // 
            this.timerTime.Enabled = true;
            this.timerTime.Interval = 500;
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.White;
            this.map.CurrentSystemName = null;
            this.map.Dock = System.Windows.Forms.DockStyle.Top;
            this.map.Location = new System.Drawing.Point(0, 39);
            this.map.Name = "map";
            this.map.RedShowDuration = 10;
            this.map.Size = new System.Drawing.Size(875, 411);
            this.map.TabIndex = 0;
            this.map.SizeChanged += new System.EventHandler(this.map_SizeChanged);
            // 
            // logWatcherLocal
            // 
            this.logWatcherLocal.Interval = 250;
            this.logWatcherLocal.LogName = "Local";
            this.logWatcherLocal.SynchronizingObject = this;
            this.logWatcherLocal.ProcessNewData += new System.EventHandler<eve_log_watcher.controls.ProcessNewDataEventArgs>(this.logWatcherLocal_ProcessNewData);
            // 
            // logWatcherIntel
            // 
            this.logWatcherIntel.Interval = 250;
            this.logWatcherIntel.LogName = "TheCitadel";
            this.logWatcherIntel.SynchronizingObject = this;
            this.logWatcherIntel.ProcessNewData += new System.EventHandler<eve_log_watcher.controls.ProcessNewDataEventArgs>(this.logWatcherIntel_ProcessNewData);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 423);
            this.Controls.Add(this.map);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "eve-log-watcher";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIntel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private controls.Map map;
        private LogWatcher logWatcherLocal;
        private LogWatcher logWatcherIntel;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.DataGridView dataGridIntel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSystem;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerTime;
        private System.Windows.Forms.ComboBox comboLogs;
        private System.Windows.Forms.Label labelCurentSystem;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Label labelChannels;
        private System.Windows.Forms.Button buttonRefresh;
    }
}

