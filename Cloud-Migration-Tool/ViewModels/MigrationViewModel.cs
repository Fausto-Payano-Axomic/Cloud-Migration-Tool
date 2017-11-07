using Cloud_Migration_Tool.Misc;
using System;
using System.Windows.Input;

namespace Cloud_Migration_Tool.ViewModels
{
    public class MigrationViewModel : INPC
    {
        public MigrationViewModel()
        {

        }

        private string _path;
        private string _projectTextBoxContents = "Path to Project Migration CSV...";
        private string _fileTextBoxContents = "Path to File Migration CSV...";
        public String Path {
            get { return _path; }
            set {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }
        public String ProjectTextBoxContents {
            get { return _projectTextBoxContents; }
            set {
                _projectTextBoxContents = value;
                RaisePropertyChanged("ProjectTextBoxContents");
            }
        }
        public String FileTextBoxContents {
            get { return _fileTextBoxContents; }
            set {
                _fileTextBoxContents = value;
                RaisePropertyChanged("FileTextBoxContents");
            }
        }


        #region CSV Parsing Methods

        private void ParseAndSerializeProjectsToBeMigrated(string filePath)
        {

        }

        private void ParseAndSerializeFilesToBeMigrated(string filePath)
        {

        }


        #endregion

        #region ICommand Related Stuff

        private ICommand toggleExecuteCommand { get; set; }
        private bool canExecute = true;
        public bool CanExcute {
            get {
                return this.canExecute;
            }
            set {
                if (this.canExecute == value)
                {
                    return;
                }
                this.canExecute = value;
            }
        }
        public ICommand ToggleExecuteCommand {
            get {
                return toggleExecuteCommand;
            }
            set {
                toggleExecuteCommand = value;
            }
        }
        
        public void ChangeCanExecute(object obj)
        {
            canExecute = !canExecute;
        }
        #endregion



      








    }
}
