using FileHelpers;

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
    }
}
