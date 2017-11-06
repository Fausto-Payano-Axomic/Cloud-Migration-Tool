using Cloud_Migration_Tool.Misc;
using Cloud_Migration_Tool.Models;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cloud_Migration_Tool.ViewModels
{
    public class MigrationViewModel : INPC
    {
        private readonly IDialogService _dialogService;
        public ICommand OpenFileCommand { get; }
        private string _path;
        
        public MigrationViewModel()
        {

        }

        public MigrationViewModel(IDialogService dialogService)
        {
            this._dialogService = dialogService;
            OpenFileCommand = new RelayCommand(OpenFile);
        }

        public string Path {
            get { return _path; }
            private set { Set(() => Path, ref _path, value); }
        }

        private void OpenFile()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Pick the CSV File",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "CSV Documents (*.csv)|All Files (*.*)|*.*"
            };

            bool? success = _dialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                var Path = settings.FileName;
            }
        }



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
