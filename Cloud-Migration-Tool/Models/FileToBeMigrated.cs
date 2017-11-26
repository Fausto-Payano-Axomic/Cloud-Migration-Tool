using FileHelpers;
using OpenAsset.RestClient.Library;

namespace Cloud_Migration_Tool.Models
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    [IgnoreFirst(1)]

    public class FileToBeMigrated : INPC
    {
        [FieldOrder(1)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string FilePath { get; set; }

        [FieldOrder(2)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ProjectCode { get; set; }

        [FieldOrder(3)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Keywords { get; set; }

        [FieldHidden]
        private bool _fileExists;

        public bool FileExists {
            get {
                return _fileExists;
            }
            set {
                _fileExists = value;
                RaisePropertyChanged("FileExists");
            }

        }

        [FieldHidden]
        private bool _fileSuccessfullyMigrated;
        public bool FileSuccessfullyMigrated {
            get { return _fileSuccessfullyMigrated; }
            set {
                _fileSuccessfullyMigrated = value;
                RaisePropertyChanged("FileSuccessfullyMigrated");
            }
        }

        [FieldHidden]
        private int _migrationHttpStatusCode;
        public int MigrationHttpStatusCode {
            get { return _migrationHttpStatusCode; }
            set { _migrationHttpStatusCode = value; }
        }

        [FieldHidden]
        private Error _migrationErrorObj;
        public Error MigrationErrorObj {
            get { return _migrationErrorObj; }
            set { _migrationErrorObj = value; }
        }
    }
}
