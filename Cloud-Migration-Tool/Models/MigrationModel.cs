using System.ComponentModel;

namespace Cloud_Migration_Tool.Models
{
    public class MigrationModel : INotifyPropertyChanged
    {
        private string _hostAddress;
        private string _bucket;
        private string _sessionKey;


        public string HostAddress {
            get { return _hostAddress; }
            set {
                _hostAddress = value;
                RaisePropertyChanged("HostAddress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged (string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
