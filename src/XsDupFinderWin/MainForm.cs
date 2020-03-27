using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output;

namespace XsDupFinderWin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string LastConfigName = "";

        private void SaveConfigToForm(Configuration config)
        {
            SourceDirectoryEdit.Text = config.SourceDirectory;
            OutputDirectoryEdit.Text = config.OutputDirectory;
            CacheFileNameEdit.Text = config.CacheFileName;
            MinLineForDuplicateEdit.Value = config.MinLineForDuplicate;
            MinLineForFullMethodDuplicateCheckEdit.Value = config.MinLineForFullMethodDuplicateCheck;
        }

        private Configuration LoadConfigFromForm()
            => new Configuration()
            {
                SourceDirectory = SourceDirectoryEdit.Text,
                OutputDirectory = OutputDirectoryEdit.Text,
                CacheFileName = CacheFileNameEdit.Text,
                MinLineForDuplicate = (int)MinLineForDuplicateEdit.Value,
                MinLineForFullMethodDuplicateCheck = (int)MinLineForFullMethodDuplicateCheckEdit.Value
            };

        private void ClearOutputLog()
            => AnalysisLog.Clear();

        private void UpdateOutputLog(string msg)
            => Invoke((MethodInvoker)(() =>
            {
                AnalysisLog.AppendText(msg + Environment.NewLine);
                AnalysisLog.Refresh();
            }));

        private void LoadConfigButton_Click(object sender, EventArgs e)
        {
            if (OpenConfigFileDialog.ShowDialog() != DialogResult.OK)
                return;

            LastConfigName = OpenConfigFileDialog.FileName;
            SaveConfigToForm(Configuration.Load(LastConfigName));
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            SaveConfigFileDialog.FileName = LastConfigName;
            if (SaveConfigFileDialog.ShowDialog() != DialogResult.OK)
                return;

            LoadConfigFromForm().SaveConfig(SaveConfigFileDialog.FileName);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var config = LoadConfigFromForm().FixOptionalValues();
            SaveConfigToForm(config);
            ClearOutputLog();

            try
            {
                config.Validate();
                var duplicateFinder = new DirectoryDuplicateFinder(config, UpdateOutputLog);
                new RenderOutput(config, duplicateFinder.Execute()).Execute();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "XsDupeFinder Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SourceDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderSelectDialog.ShowNewFolderButton = false;
            if (FolderSelectDialog.ShowDialog() == DialogResult.OK)
                SourceDirectoryEdit.Text = FolderSelectDialog.SelectedPath;
        }

        private void OutputDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderSelectDialog.ShowNewFolderButton = true;
            if (FolderSelectDialog.ShowDialog() == DialogResult.OK)
                OutputDirectoryEdit.Text = FolderSelectDialog.SelectedPath;
        }

        private void CacheFileNameButton_Click(object sender, EventArgs e)
        {
            if (SaveCacheFileDialog.ShowDialog() == DialogResult.OK)
                CacheFileNameEdit.Text = SaveCacheFileDialog.FileName;
        }
    }
}
