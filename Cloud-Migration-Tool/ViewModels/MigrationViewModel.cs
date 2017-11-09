using Cloud_Migration_Tool.Misc;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Cloud_Migration_Tool.Helper_Classes;
using System.Collections.ObjectModel;
using Cloud_Migration_Tool.Models;

namespace Cloud_Migration_Tool.ViewModels
{
    public class MigrationViewModel : INPC
    {
        
        

        public MigrationViewModel()
        {
            ProjectParseCommand = new RelayCommand((s) => ParseProjectsToBeMigrated(ProjectTextBoxContents));
            FileParseCommand = new RelayCommand((s) => ParseFilesToBeMigrated(FileTextBoxContents));
            
        }




        #region TextBoxContent
        private string _projectTextBoxContents = "Path to Project Migration CSV...";
        private string _fileTextBoxContents = "Path to File Migration CSV...";
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
        #endregion
        #region ObservableCollections
        ObservableCollection<FileToBeMigrated> _filesToBeMigrated = new ObservableCollection<FileToBeMigrated>();
        public ObservableCollection<FileToBeMigrated> FilesToBeMigrated {
            get { return _filesToBeMigrated; }
            set {
                _filesToBeMigrated = value;
                RaisePropertyChanged("FilesToBeMigrated");
            }
        }
        #endregion
        #region Commands
        private ICommand _projectParseCommand;
        private ICommand _fileParseCommand;

        public ICommand ProjectParseCommand {
            get {
                return _projectParseCommand;
            }
            set {
                _projectParseCommand = value;
            }
        }       
        public ICommand FileParseCommand {
            get {
                return _fileParseCommand;
            }
            set {
                _fileParseCommand = value;
            }
        }



        #endregion
        #region CSV Parsing Methods

        private void ParseProjectsToBeMigrated(string filePath)
        {
            MessageBox.Show("This feature is not implemented yet.");
        }
        private async void ParseFilesToBeMigrated(string filePath)
        {
            
            Parser parser = new Parser();

            var result = await Task.Run(() => parser.Parse(filePath));
            FilesToBeMigrated = new ObservableCollection<FileToBeMigrated>(result);

          
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
