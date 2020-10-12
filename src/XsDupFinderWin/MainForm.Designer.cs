namespace XsDupFinderWin
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.StartButton = new System.Windows.Forms.Button();
            this.SaveConfigButton = new System.Windows.Forms.Button();
            this.LoadConfigButton = new System.Windows.Forms.Button();
            this.ConfigGroupBox = new System.Windows.Forms.GroupBox();
            this.TrackChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.MinLineForFullMethodDuplicateCheckEdit = new System.Windows.Forms.NumericUpDown();
            this.MinLineForDuplicateEdit = new System.Windows.Forms.NumericUpDown();
            this.CacheFileNameButton = new System.Windows.Forms.Button();
            this.OutputDirectoryButton = new System.Windows.Forms.Button();
            this.SourceDirectoryButton = new System.Windows.Forms.Button();
            this.CacheFileNameEdit = new System.Windows.Forms.TextBox();
            this.OutputDirectoryEdit = new System.Windows.Forms.TextBox();
            this.SourceDirectoryEdit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.AnalysisLog = new System.Windows.Forms.TextBox();
            this.LogPanel = new System.Windows.Forms.Panel();
            this.SaveConfigFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.FolderSelectDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveCacheFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.TopPanel.SuspendLayout();
            this.ConfigGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinLineForFullMethodDuplicateCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinLineForDuplicateEdit)).BeginInit();
            this.LogPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.StartButton);
            this.TopPanel.Controls.Add(this.SaveConfigButton);
            this.TopPanel.Controls.Add(this.LoadConfigButton);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(4, 4);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(955, 36);
            this.TopPanel.TabIndex = 1;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(207, 6);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(89, 23);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start analysis";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SaveConfigButton
            // 
            this.SaveConfigButton.Location = new System.Drawing.Point(104, 6);
            this.SaveConfigButton.Name = "SaveConfigButton";
            this.SaveConfigButton.Size = new System.Drawing.Size(97, 23);
            this.SaveConfigButton.TabIndex = 1;
            this.SaveConfigButton.Text = "Save settings";
            this.SaveConfigButton.UseVisualStyleBackColor = true;
            this.SaveConfigButton.Click += new System.EventHandler(this.SaveConfigButton_Click);
            // 
            // LoadConfigButton
            // 
            this.LoadConfigButton.Location = new System.Drawing.Point(3, 6);
            this.LoadConfigButton.Name = "LoadConfigButton";
            this.LoadConfigButton.Size = new System.Drawing.Size(95, 23);
            this.LoadConfigButton.TabIndex = 0;
            this.LoadConfigButton.Text = "Load settings";
            this.LoadConfigButton.UseVisualStyleBackColor = true;
            this.LoadConfigButton.Click += new System.EventHandler(this.LoadConfigButton_Click);
            // 
            // ConfigGroupBox
            // 
            this.ConfigGroupBox.Controls.Add(this.TrackChangesCheckBox);
            this.ConfigGroupBox.Controls.Add(this.MinLineForFullMethodDuplicateCheckEdit);
            this.ConfigGroupBox.Controls.Add(this.MinLineForDuplicateEdit);
            this.ConfigGroupBox.Controls.Add(this.CacheFileNameButton);
            this.ConfigGroupBox.Controls.Add(this.OutputDirectoryButton);
            this.ConfigGroupBox.Controls.Add(this.SourceDirectoryButton);
            this.ConfigGroupBox.Controls.Add(this.CacheFileNameEdit);
            this.ConfigGroupBox.Controls.Add(this.OutputDirectoryEdit);
            this.ConfigGroupBox.Controls.Add(this.SourceDirectoryEdit);
            this.ConfigGroupBox.Controls.Add(this.label5);
            this.ConfigGroupBox.Controls.Add(this.label4);
            this.ConfigGroupBox.Controls.Add(this.label3);
            this.ConfigGroupBox.Controls.Add(this.label2);
            this.ConfigGroupBox.Controls.Add(this.label1);
            this.ConfigGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ConfigGroupBox.Location = new System.Drawing.Point(4, 40);
            this.ConfigGroupBox.Name = "ConfigGroupBox";
            this.ConfigGroupBox.Size = new System.Drawing.Size(955, 174);
            this.ConfigGroupBox.TabIndex = 2;
            this.ConfigGroupBox.TabStop = false;
            this.ConfigGroupBox.Text = "Settings";
            // 
            // TrackChangesCheckBox
            // 
            this.TrackChangesCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TrackChangesCheckBox.Location = new System.Drawing.Point(6, 147);
            this.TrackChangesCheckBox.Name = "TrackChangesCheckBox";
            this.TrackChangesCheckBox.Size = new System.Drawing.Size(205, 24);
            this.TrackChangesCheckBox.TabIndex = 28;
            this.TrackChangesCheckBox.Text = "Track changes";
            this.TrackChangesCheckBox.UseVisualStyleBackColor = true;
            // 
            // MinLineForFullMethodDuplicateCheckEdit
            // 
            this.MinLineForFullMethodDuplicateCheckEdit.Location = new System.Drawing.Point(197, 124);
            this.MinLineForFullMethodDuplicateCheckEdit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MinLineForFullMethodDuplicateCheckEdit.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.MinLineForFullMethodDuplicateCheckEdit.Name = "MinLineForFullMethodDuplicateCheckEdit";
            this.MinLineForFullMethodDuplicateCheckEdit.Size = new System.Drawing.Size(73, 20);
            this.MinLineForFullMethodDuplicateCheckEdit.TabIndex = 27;
            this.MinLineForFullMethodDuplicateCheckEdit.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MinLineForDuplicateEdit
            // 
            this.MinLineForDuplicateEdit.Location = new System.Drawing.Point(197, 98);
            this.MinLineForDuplicateEdit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MinLineForDuplicateEdit.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.MinLineForDuplicateEdit.Name = "MinLineForDuplicateEdit";
            this.MinLineForDuplicateEdit.Size = new System.Drawing.Size(73, 20);
            this.MinLineForDuplicateEdit.TabIndex = 26;
            this.MinLineForDuplicateEdit.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // CacheFileNameButton
            // 
            this.CacheFileNameButton.Location = new System.Drawing.Point(761, 71);
            this.CacheFileNameButton.Name = "CacheFileNameButton";
            this.CacheFileNameButton.Size = new System.Drawing.Size(25, 20);
            this.CacheFileNameButton.TabIndex = 25;
            this.CacheFileNameButton.TabStop = false;
            this.CacheFileNameButton.Text = "...";
            this.CacheFileNameButton.UseVisualStyleBackColor = true;
            this.CacheFileNameButton.Click += new System.EventHandler(this.CacheFileNameButton_Click);
            // 
            // OutputDirectoryButton
            // 
            this.OutputDirectoryButton.Location = new System.Drawing.Point(761, 45);
            this.OutputDirectoryButton.Name = "OutputDirectoryButton";
            this.OutputDirectoryButton.Size = new System.Drawing.Size(25, 20);
            this.OutputDirectoryButton.TabIndex = 24;
            this.OutputDirectoryButton.TabStop = false;
            this.OutputDirectoryButton.Text = "...";
            this.OutputDirectoryButton.UseVisualStyleBackColor = true;
            this.OutputDirectoryButton.Click += new System.EventHandler(this.OutputDirectoryButton_Click);
            // 
            // SourceDirectoryButton
            // 
            this.SourceDirectoryButton.Location = new System.Drawing.Point(761, 19);
            this.SourceDirectoryButton.Name = "SourceDirectoryButton";
            this.SourceDirectoryButton.Size = new System.Drawing.Size(25, 20);
            this.SourceDirectoryButton.TabIndex = 23;
            this.SourceDirectoryButton.TabStop = false;
            this.SourceDirectoryButton.Text = "...";
            this.SourceDirectoryButton.UseVisualStyleBackColor = true;
            this.SourceDirectoryButton.Click += new System.EventHandler(this.SourceDirectoryButton_Click);
            // 
            // CacheFileNameEdit
            // 
            this.CacheFileNameEdit.Location = new System.Drawing.Point(197, 71);
            this.CacheFileNameEdit.Name = "CacheFileNameEdit";
            this.CacheFileNameEdit.Size = new System.Drawing.Size(562, 20);
            this.CacheFileNameEdit.TabIndex = 20;
            // 
            // OutputDirectoryEdit
            // 
            this.OutputDirectoryEdit.Location = new System.Drawing.Point(197, 45);
            this.OutputDirectoryEdit.Name = "OutputDirectoryEdit";
            this.OutputDirectoryEdit.Size = new System.Drawing.Size(562, 20);
            this.OutputDirectoryEdit.TabIndex = 19;
            // 
            // SourceDirectoryEdit
            // 
            this.SourceDirectoryEdit.Location = new System.Drawing.Point(197, 19);
            this.SourceDirectoryEdit.Name = "SourceDirectoryEdit";
            this.SourceDirectoryEdit.Size = new System.Drawing.Size(562, 20);
            this.SourceDirectoryEdit.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Min. lines for duplicate method";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Min. lines for duplicate fragment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Cache filename";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Output directory";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Source code directory";
            // 
            // OpenConfigFileDialog
            // 
            this.OpenConfigFileDialog.DefaultExt = "yaml";
            this.OpenConfigFileDialog.Filter = "XsDupeFinder Settings|*.yaml";
            this.OpenConfigFileDialog.Title = "Load settings";
            // 
            // AnalysisLog
            // 
            this.AnalysisLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnalysisLog.Location = new System.Drawing.Point(8, 8);
            this.AnalysisLog.Multiline = true;
            this.AnalysisLog.Name = "AnalysisLog";
            this.AnalysisLog.ReadOnly = true;
            this.AnalysisLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AnalysisLog.Size = new System.Drawing.Size(939, 300);
            this.AnalysisLog.TabIndex = 6;
            this.AnalysisLog.TabStop = false;
            // 
            // LogPanel
            // 
            this.LogPanel.Controls.Add(this.AnalysisLog);
            this.LogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogPanel.Location = new System.Drawing.Point(4, 214);
            this.LogPanel.Name = "LogPanel";
            this.LogPanel.Padding = new System.Windows.Forms.Padding(8);
            this.LogPanel.Size = new System.Drawing.Size(955, 316);
            this.LogPanel.TabIndex = 4;
            // 
            // SaveConfigFileDialog
            // 
            this.SaveConfigFileDialog.DefaultExt = "yaml";
            this.SaveConfigFileDialog.Filter = "XsDupeFinder Settings|*.yaml";
            this.SaveConfigFileDialog.Title = "Save settings";
            // 
            // SaveCacheFileDialog
            // 
            this.SaveCacheFileDialog.DefaultExt = "db";
            this.SaveCacheFileDialog.Filter = "CacheDB|*.db";
            this.SaveCacheFileDialog.Title = "Cache filename";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 534);
            this.Controls.Add(this.LogPanel);
            this.Controls.Add(this.ConfigGroupBox);
            this.Controls.Add(this.TopPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XsDupeFinder";
            this.TopPanel.ResumeLayout(false);
            this.ConfigGroupBox.ResumeLayout(false);
            this.ConfigGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinLineForFullMethodDuplicateCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinLineForDuplicateEdit)).EndInit();
            this.LogPanel.ResumeLayout(false);
            this.LogPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SaveConfigButton;
        private System.Windows.Forms.Button LoadConfigButton;
        private System.Windows.Forms.GroupBox ConfigGroupBox;
        private System.Windows.Forms.Button CacheFileNameButton;
        private System.Windows.Forms.Button OutputDirectoryButton;
        private System.Windows.Forms.Button SourceDirectoryButton;
        private System.Windows.Forms.TextBox CacheFileNameEdit;
        private System.Windows.Forms.TextBox OutputDirectoryEdit;
        private System.Windows.Forms.TextBox SourceDirectoryEdit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown MinLineForFullMethodDuplicateCheckEdit;
        private System.Windows.Forms.NumericUpDown MinLineForDuplicateEdit;
        private System.Windows.Forms.OpenFileDialog OpenConfigFileDialog;
        private System.Windows.Forms.TextBox AnalysisLog;
        private System.Windows.Forms.Panel LogPanel;
        private System.Windows.Forms.SaveFileDialog SaveConfigFileDialog;
        private System.Windows.Forms.FolderBrowserDialog FolderSelectDialog;
        private System.Windows.Forms.SaveFileDialog SaveCacheFileDialog;
        private System.Windows.Forms.CheckBox TrackChangesCheckBox;
    }
}

