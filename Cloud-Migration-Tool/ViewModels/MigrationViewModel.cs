using Cloud_Migration_Tool.Helper_Classes;
using Cloud_Migration_Tool.Misc;
using Cloud_Migration_Tool.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Cloud_Migration_Tool.ViewModels
{
    public class MigrationViewModel : INPC
    {
        MigrationModel migration = new MigrationModel();

        public MigrationViewModel()
        {
            Predicate<object> canExecuteProjectButton = _ => !(ProjectTextBoxContents.Contains("/"));
            ProjectParseCommand = new RelayCommand((s) => ParseProjectsToBeMigrated(ProjectTextBoxContents), canExecuteProjectButton);
            FileParseCommand = new RelayCommand((s) => ParseFilesToBeMigrated(FileTextBoxContents));
            CheckFileIntegrityCommand = new RelayCommand(async (s) => await FilesIntegrityCheckTask());
            LoginCommand = new RelayCommand(Login);

        }


        private int _totalFilesInMigration = 0;
        private int _checkCount = 0;
        private bool _checkingFiles = false;
        private string _hostAddress;
        private string _username;

        public int CheckCount {
            get { return _checkCount; }
            set {
                _checkCount = value;
                RaisePropertyChanged("CheckCount");
            }

        }
        public bool CheckingFiles {
            get { return _checkingFiles; }
            set {
                _checkingFiles = value;
                RaisePropertyChanged("CheckingFiles");
            }
        }
        public int TotalFilesInMigration {
            get { return _totalFilesInMigration; }
            set {
                _totalFilesInMigration = value;
                RaisePropertyChanged("TotalFilesInMigration");
            }
        }



        public string HostAddress {
            get { return _hostAddress; }
            set {
                _hostAddress = value;
                RaisePropertyChanged("HostAddress");
            }
        }
        public string Username {
            get { return _username; }
            set {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }

        #region Logging_in

        private void Login(object parameter)
        {
            var passwordContainer = parameter as ISecurePassword;
            if(passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                MessageBox.Show(ConvertToUnsecureString(secureString));
            }

        }
        #endregion

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
                TotalFilesInMigration = value.Count;
                RaisePropertyChanged("FilesToBeMigrated");

            }
        }
        #endregion
        #region Commands
        private ICommand _projectParseCommand;
        private ICommand _fileParseCommand;
        private ICommand _checkFileIntegrityCommand;
        private ICommand _loginCommand;

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
        public ICommand LoginCommand {
            get { return _loginCommand; }
            set {
                _loginCommand = value;
                RaisePropertyChanged("LoginCommand");
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
            CheckCount = 0;


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
                CheckCount++;
            };
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

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }












    }
}
