using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output.Json;
using XsDupFinder.Lib.Output.Renderer;

namespace XsDupFinder.Lib.Output.ChangeTracker
{
    class RenderChangeTracker : IRender
    {
        public const string FileName = "Changes.Json";
        const string JournalDirectoryName = "ChangesJournal";

        private JsonOutput LastData;
        private Configuration Configuration;
        private List<Duplicate> Duplicates;

        bool LoadLastData()
        {
            var jsonFileName = RenderFileHelper.BuildOutputFileName(Configuration, RenderJson.FileName);
            LastData = JsonOutput.Load(jsonFileName);
            return LastData != null;
        }

        bool ConfigurationChanged()
        {
            if (LastData?.Configuration == null)
                return true;

            return
                LastData.Configuration.MinLineForDuplicate != Configuration.MinLineForDuplicate ||
                LastData.Configuration.MinLineForFullMethodDuplicateCheck != Configuration.MinLineForFullMethodDuplicateCheck;
        }

        bool DuplicatesChanged()
        {
            if (LastData?.Duplicates == null || LastData.Duplicates.Count != Duplicates.Count)
                return true;

            for (int i = 0; i < LastData.Duplicates.Count; i++)
            {
                var lastDuplicate = LastData.Duplicates[i];
                var duplicate = Duplicates[i];
                if (lastDuplicate.Code != duplicate.Code || lastDuplicate.Locations.Count != duplicate.Locations.Count)
                    return true;

                for (int j = 0; j < lastDuplicate.Locations.Count; j++)
                {
                    var lastLocation = lastDuplicate.Locations[j];
                    var location = duplicate.Locations[j];
                    if (lastLocation.Filename != location.Filename || lastLocation.MethodName != location.MethodName || lastLocation.StartLine != location.StartLine)
                        return true;
                }
            }

            return false;
        }

        void AppendChangeToFile()
        {
            var fileName = RenderFileHelper.BuildOutputFileName(Configuration, FileName);
            var changes = JsonChangesOutput.Load(fileName) ?? new JsonChangesOutput();

            changes.Items.Add(new JsonChangesItem
            {
                Changed = DateTime.Now,
                ConfigurationChanged = ConfigurationChanged(),
                NumberOfFragements = Duplicates.Count,
                NumberOfLocations = Duplicates.Sum(q => q.Locations.Count)
            });

            File.WriteAllText(fileName, changes.ToJsonString());
        }

        string BuildBackupFileName(string fileName)
        {
            string backupFileName;
            var uniqueExt = "";
            do
            {
                backupFileName = RenderFileHelper.BuildOutputFileName(Configuration,
                    $"{JournalDirectoryName}\\{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Today:yyyyMMdd}{uniqueExt}{Path.GetExtension(fileName)}");
                uniqueExt = $"_{DateTime.Now:FFFF}";
            } while (File.Exists(backupFileName));

            return backupFileName;
        }

        void BackupFile(string fileName)
        {
            try
            {
                File.Copy(RenderFileHelper.BuildOutputFileName(Configuration, fileName), BuildBackupFileName(fileName));
            }
            catch
            { }
        }

        void AddCurrentFilesToJournal()
        {
            Directory.CreateDirectory(RenderFileHelper.BuildOutputFileName(Configuration, JournalDirectoryName));
            BackupFile(RenderMainHtml.FileName);
            BackupFile(RenderJson.FileName);
        }

        public void Execute(Configuration configuration, List<Duplicate> duplicates)
        {
            Configuration = configuration;
            Duplicates = duplicates;
            if (!configuration.TrackChanges || !LoadLastData())
                return;

            if (DuplicatesChanged() || !File.Exists(RenderFileHelper.BuildOutputFileName(Configuration, FileName)))
            {
                AppendChangeToFile();
                AddCurrentFilesToJournal();
            }
        }
    }
}
