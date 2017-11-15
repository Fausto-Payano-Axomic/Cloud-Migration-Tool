using Cloud_Migration_Tool.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OpenAsset.RestClient.Library;
using OpenAsset.RestClient.Library.Noun;
using System.IO;

namespace Cloud_Migration_Tool.Helper_Classes
{
    public class FileMigrationHandler
    {
        private ObservableCollection<FileToBeMigrated> _filesNotUploaded;
        private ObservableCollection<FileToBeMigrated> FilesNotUploaded {
            get { return _filesNotUploaded; }
            set { _filesNotUploaded = value; }

        }

        private MigrationModel Migration;

        public FileMigrationHandler(ObservableCollection<FileToBeMigrated> filesToWorkWith, MigrationModel migrationModel)
        {
            FilesNotUploaded = filesToWorkWith;
            Migration = migrationModel;
        }

        public async void StartMigration()
        {
            if (Migration != null)
            {
                foreach (var file in FilesNotUploaded)
                {
                    Task<bool> task = UploadFile(file);

                }
            }
        }
        private async Task<bool> UploadFile(FileToBeMigrated file)
        {
            var fileInfo = new FileInfo(file.FilePath);
            //Prepare file Object for API ENDPOINT
            OpenAsset.RestClient.Library.Noun.File fileUpload = new OpenAsset.RestClient.Library.Noun.File()
            {
                OriginalFilename = fileInfo.Name,
                CategoryId = 1,
                ProjectId = 150
            };

            var response = Migration.Conn.SendObject<OpenAsset.RestClient.Library.Noun.File>(fileUpload, file.FilePath, true);
            file.FileSuccessfullyMigrated = true;
            return true;
        }
    }


}

