using Cloud_Migration_Tool.Models;
using OpenAsset.RestClient.Library;
using OpenAsset.RestClient.Library.Noun;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Migration_Tool.Helper_Classes {
    public class FileMigrationHandler {
        public string KeywordDelimiter;
        public MigrationModel Migration;
        private string PreferredKeywordCategoryname;
        private List<Keyword> FileKeywordsCache = new List<Keyword>();
        private List<KeywordCategory> FileKeywordCategoriesCache = new List<KeywordCategory>();
        private KeywordCategory DataMigrationCategory;


        private ObservableCollection<FileToBeMigrated> _filesNotUploaded;
        private ObservableCollection<FileToBeMigrated> FilesNotUploaded {
            get { return _filesNotUploaded; }
            set { _filesNotUploaded = value; }
        }


        public FileMigrationHandler(ObservableCollection<FileToBeMigrated> filesToWorkWith, MigrationModel migrationModel,
            string preferredKeywordDelimiter, string keywordCategoryName) {
            FilesNotUploaded = filesToWorkWith;
            Migration = migrationModel;
            KeywordDelimiter = preferredKeywordDelimiter;
            PreferredKeywordCategoryname = keywordCategoryName;

        }

        public void StartMigration() {
            int maxConcurrency = 20;
            using (SemaphoreSlim concurrencySemaphores = new SemaphoreSlim(maxConcurrency)) {
                List<Task> tasks = new List<Task>();
                foreach (var file in FilesNotUploaded) {
                    concurrencySemaphores.Wait();
                    var t = Task.Factory.StartNew(() => {
                        try {
                            var fileInfo = new FileInfo(file.FilePath);
                            OpenAsset.RestClient.Library.Noun.File fileUpload = new OpenAsset.RestClient.Library.Noun.File() {
                                OriginalFilename = fileInfo.Name,
                                CategoryId = 1,
                                ProjectId = 10
                            };
                            var result = Migration.Conn.SendObject(fileUpload, file.FilePath, true);
                        }
                        catch (RESTAPIException ex) {
                            file.MigrationErrorObj = ex.ErrorObj;
                            switch (ex.ErrorObj.HttpStatusCode) {
                                case 409:
                                    file.MigrationHttpStatusCode = ex.ErrorObj.HttpStatusCode;
                                    break;
                            }
                        }

                        catch (Exception oddEx) {
                        }
                        finally {
                            if (file.MigrationErrorObj == null) {
                                file.FileSuccessfullyMigrated = true;
                            }
                            else {
                                file.FileSuccessfullyMigrated = false;
                            }
                            concurrencySemaphores.Release();
                        }
                    });
                    tasks.Add(t);
                }

                Task.WaitAll(tasks.ToArray());
            }
        }


        private string[] formatFileMigrationKeywords(string keywordString) {
            string[] strArray = keywordString.Split(new string[] { this.KeywordDelimiter }, StringSplitOptions.None);
            return strArray;
        }

        #region General GET Requests for Caching
        private List<Keyword> GetKeywords() {
            return Migration.Conn.GetObjects<Keyword>(new RESTOptions<Keyword>());
        }
        private List<KeywordCategory> GetKeywordCategories() {
            return Migration.Conn.GetObjects<KeywordCategory>(new RESTOptions<KeywordCategory>());
        }
        #endregion
        #region General POST Requests for creation
        private Keyword CreateKeyword(string keywordName) {

            Keyword newKeyword = new Keyword() {
                KeywordCategoryId = DataMigrationCategory.Id,
                Name = keywordName
            };
            return Migration.Conn.SendObject<Keyword>(newKeyword, true);

        }
        private KeywordCategory CreateKeywordCategory(string keywordCategoryName) {
            KeywordCategory newKeywordCategory = new KeywordCategory() {
                CategoryId = 1,
                Name = keywordCategoryName
            };
            return Migration.Conn.SendObject<KeywordCategory>(newKeywordCategory, true);

        }
        #endregion
    }


}

