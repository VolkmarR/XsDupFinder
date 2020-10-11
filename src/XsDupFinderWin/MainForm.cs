using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output;

namespace XsDupFinderWin
{
    public partial class MainForm : Form
    {
        private readonly SynchronizationContext SynchronizationContext;

        public MainForm()
        {
            InitializeComponent();
            SynchronizationContext = SynchronizationContext.Current;
        }

        private string LastConfigName = "";

        private void SaveConfigToForm(Configuration config)
        {
            SourceDirectoryEdit.Text = config.SourceDirectory;
            OutputDirectoryEdit.Text = config.OutputDirectory;
            CacheFileNameEdit.Text = config.CacheFileName;
            MinLineForDuplicateEdit.Value = config.MinLineForDuplicate;
            MinLineForFullMethodDuplicateCheckEdit.Value = config.MinLineForFullMethodDuplicateCheck;
            TrackChangesCheckBox.Checked = config.TrackChanges;
        }

        private Configuration LoadConfigFromForm()
            => new Configuration()
            {
                SourceDirectory = SourceDirectoryEdit.Text,
                OutputDirectory = OutputDirectoryEdit.Text,
                CacheFileName = CacheFileNameEdit.Text,
                MinLineForDuplicate = (int)MinLineForDuplicateEdit.Value,
                MinLineForFullMethodDuplicateCheck = (int)MinLineForFullMethodDuplicateCheckEdit.Value,
                TrackChanges = TrackChangesCheckBox.Checked
            };

        private void ClearOutputLog()
            => AnalysisLog.Clear();

        private void SetFormState(bool enabled)
        {
            LoadConfigButton.Enabled = enabled;
            SaveConfigButton.Enabled = enabled;
            StartButton.Enabled = enabled;
            ConfigGroupBox.Enabled = enabled;
            UseWaitCursor = !enabled;
            Application.DoEvents();
        }

        private void UpdateOutputLog(string msg)
            => SynchronizationContext.Post(new SendOrPostCallback(updateMsg =>
            {
                AnalysisLog.AppendText((string)updateMsg + Environment.NewLine);
            }), msg);

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

        private async void StartButton_Click(object sender, EventArgs e)
        {
            var config = LoadConfigFromForm().FixOptionalValues();
            SaveConfigToForm(config);
            ClearOutputLog();

            try
            {
                config.Validate();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "XsDupeFinder Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetFormState(false);
            try
            {
                var duplicateFinder = new DirectoryDuplicateFinder(config, UpdateOutputLog);
                await Task.Run(() => new RenderOutput(config, duplicateFinder.Execute()).Execute());
            }
            finally
            {
                SetFormState(true);
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
