using Cloud_Migration_Tool.Models;
using OpenAsset.RestClient.Library;
using OpenAsset.RestClient.Library.Noun;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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


        public FileMigrationHandler(ObservableCollection<FileToBeMigrated> filesToWorkWith, MigrationModel migrationModel, string preferredKeywordDelimiter, string keywordCategoryName) {
            FilesNotUploaded = filesToWorkWith;
            Migration = migrationModel;
            KeywordDelimiter = preferredKeywordDelimiter;
            PreferredKeywordCategoryname = keywordCategoryName;

        }

        public async void StartMigration() {
            if (Migration != null) {
                this.FileKeywordCategoriesCache = GetKeywordCategories();
                this.DataMigrationCategory = GetOrCreateMigrationKeywordCategory();
                this.FileKeywordsCache = GetKeywords();
                
                
                foreach (var file in FilesNotUploaded) {
                    Task<bool> task = UploadFile(file);

                }
            }
        }

        private KeywordCategory GetOrCreateMigrationKeywordCategory() {
            if (this.FileKeywordCategoriesCache.Any(keywordCategory => keywordCategory.Name == this.PreferredKeywordCategoryname)) {
                var categoryCheck = FileKeywordCategoriesCache.Where(keywordCategory => keywordCategory.Name == PreferredKeywordCategoryname);
                return categoryCheck.FirstOrDefault(keywordCategory => keywordCategory.CategoryId == 1);
            }
            else {
                return CreateKeywordCategory(this.PreferredKeywordCategoryname);
            }
        }

        private async Task<bool> UploadFile(FileToBeMigrated file) {
            var fileInfo = new FileInfo(file.FilePath);
            string[] potentialKeywords = new string[] { };
            //Prepare file Object for API ENDPOINT
            OpenAsset.RestClient.Library.Noun.File fileUpload = new OpenAsset.RestClient.Library.Noun.File() {
                OriginalFilename = fileInfo.Name,
                CategoryId = 1,
                ProjectId = 150
            };

            //Check if there are keywords to tag.
            if (!(string.IsNullOrEmpty(file.Keywords))) {

                //Return an array of keywords to tag.
                potentialKeywords = formatFileMigrationKeywords(file.Keywords);
            }

            //If there are keywords to tag then for each keyword, add it to the file to be uploaded's keyword list.
            if (potentialKeywords.Length > 0) {
                foreach (var keyword in potentialKeywords) {
                    int idOfKeyword = GetProperKeywordId(keyword);
                    fileUpload.Keywords.Add(new Keyword { Id = idOfKeyword });
                }
            }

            var response = Migration.Conn.SendObject<OpenAsset.RestClient.Library.Noun.File>(fileUpload, file.FilePath, true);
            file.FileSuccessfullyMigrated = true;
            return true;
        }

        private string[] formatFileMigrationKeywords(string keywordString) {
            string[] strArray = keywordString.Split(new string[] { this.KeywordDelimiter }, StringSplitOptions.None);
            return strArray;
        }
        private int GetProperKeywordId(string keywordName) {

            //If our keyword cache contains any keywords that match the current keyword being checked
            if (FileKeywordsCache.Any(keyword => keyword.Name == keywordName)) {

                //Check to see if we get more than one result from the previous conditional.
                var keywordCheck = (ICollection<Keyword>)FileKeywordsCache.Where(keyword => keyword.Name == keywordName);
                if (keywordCheck.Count > 1) {
                    //If we get multiple hits for the same keyword name string then grab the one that has the same category ID
                    //as our migration keyword category.
                    return keywordCheck.First(keyword => keyword.KeywordCategoryId == this.DataMigrationCategory.Id).Id;
                }
                //Else, if there is only one keyword result (or none), return the first result ID.
                else {
                    return keywordCheck.FirstOrDefault(x => x.Name == keywordName).Id;
                }
            }
            //If the keyword is nowhere to be found, we are going to go ahead and create it, add it to the cache and return ID.
            else {
                var newlyCreatedKeyword = CreateKeyword(keywordName);
                FileKeywordsCache.Add(newlyCreatedKeyword);
                return newlyCreatedKeyword.Id;
            }
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

