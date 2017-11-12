using Cloud_Migration_Tool.Helper_Classes;
using Cloud_Migration_Tool.Misc;
using Cloud_Migration_Tool.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;

namespace Cloud_Migration_Tool.ViewModels
{
    public class MigrationViewModel : INPC
    {

        public MigrationViewModel()
        {
            Predicate<object> canExecuteProjectButton = _ => !(ProjectTextBoxContents.Contains("/"));
            ProjectParseCommand = new RelayCommand((s) => ParseProjectsToBeMigrated(ProjectTextBoxContents),canExecuteProjectButton);
            FileParseCommand = new RelayCommand((s) => ParseFilesToBeMigrated(FileTextBoxContents));
            CheckFileIntegrityCommand = new RelayCommand(async (s) => await FilesIntegrityCheckTask());
            
        }


        private string _totalFilesInMigration = "0";
        private bool CheckingFiles = false;
        public string TotalFilesInMigration {
            get { return _totalFilesInMigration.ToString(); }
            set {
                _totalFilesInMigration = value;
                RaisePropertyChanged("TotalFilesInMigration");
            }
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
                TotalFilesInMigration = value.Count.ToString();
                RaisePropertyChanged("FilesToBeMigrated");

            }
        }
        #endregion
        #region Commands
        private ICommand _projectParseCommand;
        private ICommand _fileParseCommand;
        private ICommand _checkFileIntegrityCommand;

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
        public ICommand CheckFileIntegrityCommand {
            get {
                return _checkFileIntegrityCommand;
            }
            set {
                _checkFileIntegrityCommand = value;
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
        private async Task FilesIntegrityCheckTask()
        {
            CheckingFiles = true;
            await Task.Run(() => CheckFiles());
        }

        private void CheckFiles()
        {
            foreach (var file in FilesToBeMigrated)
            {
                file.FileExists = File.Exists(file.FilePath);
            };
            RaisePropertyChanged("FilesToBeMigrated");
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
