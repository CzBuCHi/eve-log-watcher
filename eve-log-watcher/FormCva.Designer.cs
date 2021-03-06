﻿namespace eve_log_watcher
{
    partial class FormCva
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridCva = new System.Windows.Forms.DataGridView();
            this.colPilotName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCorporationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAllianceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPilotKos = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCorporationKos = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colAllianceKos = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colKos = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.labelLoading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCva)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridCva
            // 
            this.dataGridCva.AllowUserToAddRows = false;
            this.dataGridCva.AllowUserToDeleteRows = false;
            this.dataGridCva.AllowUserToOrderColumns = true;
            this.dataGridCva.AllowUserToResizeColumns = false;
            this.dataGridCva.AllowUserToResizeRows = false;
            this.dataGridCva.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridCva.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridCva.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridCva.ColumnHeadersHeight = 30;
            this.dataGridCva.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridCva.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPilotName,
            this.colCorporationName,
            this.colAllianceName,
            this.colPilotKos,
            this.colCorporationKos,
            this.colAllianceKos,
            this.colKos});
            this.dataGridCva.EnableHeadersVisualStyles = false;
            this.dataGridCva.Location = new System.Drawing.Point(0, 0);
            this.dataGridCva.MultiSelect = false;
            this.dataGridCva.Name = "dataGridCva";
            this.dataGridCva.ReadOnly = true;
            this.dataGridCva.RowHeadersVisible = false;
            this.dataGridCva.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridCva.Size = new System.Drawing.Size(292, 134);
            this.dataGridCva.TabIndex = 4;
            this.dataGridCva.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridCva_DataBindingComplete);
            // 
            // colPilotName
            // 
            this.colPilotName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPilotName.DataPropertyName = "PilotName";
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.colPilotName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPilotName.HeaderText = "PilotName";
            this.colPilotName.Name = "colPilotName";
            this.colPilotName.ReadOnly = true;
            this.colPilotName.Width = 79;
            // 
            // colCorporationName
            // 
            this.colCorporationName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCorporationName.DataPropertyName = "CorporationName";
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.colCorporationName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCorporationName.HeaderText = "CorporationName";
            this.colCorporationName.Name = "colCorporationName";
            this.colCorporationName.ReadOnly = true;
            this.colCorporationName.Width = 113;
            // 
            // colAllianceName
            // 
            this.colAllianceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAllianceName.DataPropertyName = "AllianceName";
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.colAllianceName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAllianceName.HeaderText = "AllianceName";
            this.colAllianceName.Name = "colAllianceName";
            this.colAllianceName.ReadOnly = true;
            this.colAllianceName.Width = 96;
            // 
            // colPilotKos
            // 
            this.colPilotKos.DataPropertyName = "PilotKos";
            this.colPilotKos.HeaderText = "PilotKos";
            this.colPilotKos.Name = "colPilotKos";
            this.colPilotKos.ReadOnly = true;
            this.colPilotKos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPilotKos.Visible = false;
            this.colPilotKos.Width = 70;
            // 
            // colCorporationKos
            // 
            this.colCorporationKos.DataPropertyName = "CorporationKos";
            this.colCorporationKos.HeaderText = "CorporationKos";
            this.colCorporationKos.Name = "colCorporationKos";
            this.colCorporationKos.ReadOnly = true;
            this.colCorporationKos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCorporationKos.Visible = false;
            this.colCorporationKos.Width = 104;
            // 
            // colAllianceKos
            // 
            this.colAllianceKos.DataPropertyName = "AllianceKos";
            this.colAllianceKos.HeaderText = "AllianceKos";
            this.colAllianceKos.Name = "colAllianceKos";
            this.colAllianceKos.ReadOnly = true;
            this.colAllianceKos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAllianceKos.Visible = false;
            this.colAllianceKos.Width = 87;
            // 
            // colKos
            // 
            this.colKos.DataPropertyName = "Kos";
            this.colKos.HeaderText = "Kos";
            this.colKos.Name = "colKos";
            this.colKos.ReadOnly = true;
            this.colKos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colKos.Visible = false;
            this.colKos.Width = 50;
            // 
            // labelLoading
            // 
            this.labelLoading.Location = new System.Drawing.Point(12, 9);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(280, 125);
            this.labelLoading.TabIndex = 5;
            this.labelLoading.Text = "Loading ...";
            // 
            // FormCva
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(343, 231);
            this.Controls.Add(this.labelLoading);
            this.Controls.Add(this.dataGridCva);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCva";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "http://local.cva-eve.org";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCva_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCva)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridCva;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPilotName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCorporationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAllianceName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPilotKos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCorporationKos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAllianceKos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colKos;
    }
}