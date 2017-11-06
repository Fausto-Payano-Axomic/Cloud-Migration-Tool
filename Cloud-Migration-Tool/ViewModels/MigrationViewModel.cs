using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Migration_Tool.ViewModels
{
    public class MigrationViewModel : INPC
    {
        public MigrationViewModel()
        {
          
        }
        private Models.MigrationModel _currentMigration;

        public Models.MigrationModel CurrentMigration {
            get { return _currentMigration; }
            set {
                _currentMigration = value;
                RaisePropertyChanged("CurrentMigration");
            }
        }
    }
}
