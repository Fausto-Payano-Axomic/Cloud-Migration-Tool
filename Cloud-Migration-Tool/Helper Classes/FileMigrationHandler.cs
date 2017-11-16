using Cloud_Migration_Tool.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OpenAsset.RestClient.Library;
using OpenAsset.RestClient.Library.Noun;
using System.IO;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace Cloud_Migration_Tool.Helper_Classes
{
    public class FileMigrationHandler
    {
        public string KeywordDelimiter;
        public MigrationModel Migration;
        private List<Keyword> FileKeywordsCache = new List<Keyword>();
        private List<KeywordCategory> FileKeywordCategoriesCache = new List<KeywordCategory>();
        private KeywordCategory DataMigrationCategory;


        private ObservableCollection<FileToBeMigrated> _filesNotUploaded;
        private ObservableCollection<FileToBeMigrated> FilesNotUploaded {
            get { return _filesNotUploaded; }
            set { _filesNotUploaded = value; }
        }
        

        public FileMigrationHandler(ObservableCollection<FileToBeMigrated> filesToWorkWith, MigrationModel migrationModel, string preferredKeywordDelimiter)
        {
            FilesNotUploaded = filesToWorkWith;
            Migration = migrationModel;
            KeywordDelimiter = preferredKeywordDelimiter;
        }

        public async void StartMigration()
        {
            if (Migration != null)
            {
              /*  this.FileKeywordsCache = GetKeywords();
                this.FileKeywordCategoriesCache = GetKeywordCategories();
                this.DataMigrationCategory = GetOrCreateMigrationKeywordCategory();
                */
                foreach (var file in FilesNotUploaded)
                {
                    Task<bool> task = UploadFile(file);

                }
            }
        }

       /* private KeywordCategory GetOrCreateMigrationKeywordCategory()
        {
           if(this.FileKeywordCategoriesCache.Any((keywordCategory => keywordCategory.Name == "Data Migration")
        }*/
        private async Task<bool> UploadFile(FileToBeMigrated file)
        {
            var fileInfo = new FileInfo(file.FilePath);
            string[] potentialKeywords = new string[] { };
            //Prepare file Object for API ENDPOINT
            OpenAsset.RestClient.Library.Noun.File fileUpload = new OpenAsset.RestClient.Library.Noun.File()
            {
                OriginalFilename = fileInfo.Name,
                CategoryId = 1,
                ProjectId = 150
            };
            if (!(string.IsNullOrEmpty(file.Keywords))) {
                potentialKeywords = formatFileMigrationKeywords(file.Keywords);
            }
            if(potentialKeywords.Length > 0)
            {
                foreach(var keyword in potentialKeywords)
                {
                    
                }
            }

            var response = Migration.Conn.SendObject<OpenAsset.RestClient.Library.Noun.File>(fileUpload, file.FilePath, true);
            file.FileSuccessfullyMigrated = true;
            return true;
        }
        private string[] formatFileMigrationKeywords(string keywordString)
        {
            string[] strArray = keywordString.Split(new string[] { this.KeywordDelimiter }, StringSplitOptions.None);
            return strArray;
        }

       /* private int GetProperKeywordId(string keywordName)
        {
            if (FileKeywordsCache.Any(keyword => keyword.Name == keywordName)){
                var keywordCheck = FileKeywordsCache.Where(keyword => keyword.Name == keywordName);
                if(keywordCheck.Count > 1)
                {

                }
            }
        }
        */
        #region General GET Requests for Caching
        private List<Keyword> GetKeywords()
        {
            return Migration.Conn.GetObjects<Keyword>(new RESTOptions<Keyword>());
        }
        private List<KeywordCategory> GetKeywordCategories()
        {
            return Migration.Conn.GetObjects<KeywordCategory>(new RESTOptions<KeywordCategory>());
        }
        #endregion
    }


}

