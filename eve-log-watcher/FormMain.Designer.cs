using eve_log_watcher.controls;
using exscape;

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
            this.label2 = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hotkeyControlKosCheck = new exscape.HotkeyControl();
            this.comboBoxSystems = new System.Windows.Forms.ComboBox();
            this.labelChannels = new System.Windows.Forms.Label();
            this.comboLogs = new System.Windows.Forms.ComboBox();
            this.panelRight = new System.Windows.Forms.Panel();
            this.dataGridIntel = new System.Windows.Forms.DataGridView();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.map = new eve_log_watcher.controls.Map();
            this.logWatcherLocal = new eve_log_watcher.controls.LogWatcher();
            this.logWatcherIntel = new eve_log_watcher.controls.LogWatcher();
            this.buttonHotkeyChangeConfirm = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIntel)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(112, 8);
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
            this.buttonStop.Location = new System.Drawing.Point(198, 8);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(80, 25);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonHotkeyChangeConfirm);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.labelTime);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.hotkeyControlKosCheck);
            this.panelTop.Controls.Add(this.comboBoxSystems);
            this.panelTop.Controls.Add(this.buttonRefresh);
            this.panelTop.Controls.Add(this.labelChannels);
            this.panelTop.Controls.Add(this.comboLogs);
            this.panelTop.Controls.Add(this.buttonStart);
            this.panelTop.Controls.Add(this.buttonStop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1276, 39);
            this.panelTop.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Current system:";
            // 
            // labelTime
            // 
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTime.Location = new System.Drawing.Point(12, 9);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(94, 24);
            this.labelTime.TabIndex = 4;
            this.labelTime.Text = "00:00:00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(563, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "KOS check shortcut:";
            // 
            // hotkeyControlKosCheck
            // 
            this.hotkeyControlKosCheck.Hotkey = System.Windows.Forms.Keys.F;
            this.hotkeyControlKosCheck.HotkeyModifiers = System.Windows.Forms.Keys.Control;
            this.hotkeyControlKosCheck.Location = new System.Drawing.Point(675, 9);
            this.hotkeyControlKosCheck.Name = "hotkeyControlKosCheck";
            this.hotkeyControlKosCheck.Size = new System.Drawing.Size(182, 20);
            this.hotkeyControlKosCheck.TabIndex = 11;
            this.hotkeyControlKosCheck.Text = "Control + F";            
            // 
            // comboBoxSystems
            // 
            this.comboBoxSystems.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSystems.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSystems.DisplayMember = "SolarSystemName";
            this.comboBoxSystems.FormattingEnabled = true;
            this.comboBoxSystems.Location = new System.Drawing.Point(369, 11);
            this.comboBoxSystems.Name = "comboBoxSystems";
            this.comboBoxSystems.Size = new System.Drawing.Size(188, 21);
            this.comboBoxSystems.TabIndex = 10;
            this.comboBoxSystems.SelectedIndexChanged += new System.EventHandler(this.comboBoxSystems_SelectedIndexChanged);
            // 
            // labelChannels
            // 
            this.labelChannels.AutoSize = true;
            this.labelChannels.Location = new System.Drawing.Point(881, 12);
            this.labelChannels.Name = "labelChannels";
            this.labelChannels.Size = new System.Drawing.Size(54, 13);
            this.labelChannels.TabIndex = 8;
            this.labelChannels.Text = "Channels:";
            // 
            // comboLogs
            // 
            this.comboLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLogs.FormattingEnabled = true;
            this.comboLogs.Location = new System.Drawing.Point(941, 9);
            this.comboLogs.Name = "comboLogs";
            this.comboLogs.Size = new System.Drawing.Size(296, 21);
            this.comboLogs.TabIndex = 5;
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
            // buttonHotkeyChangeConfirm
            // 
            this.buttonHotkeyChangeConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHotkeyChangeConfirm.BackgroundImage = global::eve_log_watcher.Properties.Resources.confirm;
            this.buttonHotkeyChangeConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonHotkeyChangeConfirm.Location = new System.Drawing.Point(854, 8);
            this.buttonHotkeyChangeConfirm.Name = "buttonHotkeyChangeConfirm";
            this.buttonHotkeyChangeConfirm.Size = new System.Drawing.Size(21, 21);
            this.buttonHotkeyChangeConfirm.TabIndex = 14;
            this.buttonHotkeyChangeConfirm.UseVisualStyleBackColor = true;
            this.buttonHotkeyChangeConfirm.Click += new System.EventHandler(this.buttonHotkeyChangeConfirm_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.BackgroundImage = global::eve_log_watcher.Properties.Resources.refresh;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.Location = new System.Drawing.Point(1243, 9);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(21, 21);
            this.buttonRefresh.TabIndex = 9;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
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
        private System.Windows.Forms.Label labelChannels;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ComboBox comboBoxSystems;
        private System.Windows.Forms.Label label1;
        private HotkeyControl hotkeyControlKosCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonHotkeyChangeConfirm;
    }
}

