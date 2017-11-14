using System.ComponentModel;
using OpenAsset.RestClient.Library;

namespace Cloud_Migration_Tool.Models
{
    public class MigrationModel : INotifyPropertyChanged
    {
        private string _hostAddress;
        private string _sessionKey;
        private Connection _conn;
        private bool _credentialsValidated;

        public MigrationModel()
        {
        }

        public void Login(string username, string password, string hostAddress)
        {
            Conn = Connection.GetConnection(hostAddress, username, password);
            CredentialsValidated = Conn.ValidateCredentials();
            if (CredentialsValidated)
            {
                SessionKey = Conn.SessionKey;
            }
            else
            {
                SessionKey = "Incorrect Username or Password";
            }
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
        public Connection Conn {
            get { return _conn; }
            set { _conn = value;
                RaisePropertyChanged("Conn");
            }
        }
        public bool CredentialsValidated {
            get { return _credentialsValidated; }
            set { _credentialsValidated = value;
                RaisePropertyChanged("CredentialsValidated");
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
