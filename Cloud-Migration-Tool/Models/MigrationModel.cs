using System.ComponentModel;
using OpenAsset.RestClient;

namespace Cloud_Migration_Tool.Models
{
    public class MigrationModel : INotifyPropertyChanged
    {
        private string _hostAddress;
        private string _sessionKey;

        public MigrationModel()
        {

        }

        public string HostAddress {
            get { return _hostAddress; }
            set {
                _hostAddress = value;
                RaisePropertyChanged("HostAddress");
            }
        }
        public string SessionKey {
            get { return _sessionKey; }
            set { _sessionKey = value;
                RaisePropertyChanged("SessionKey");
            }
        }


        #region InterfaceMethods
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
