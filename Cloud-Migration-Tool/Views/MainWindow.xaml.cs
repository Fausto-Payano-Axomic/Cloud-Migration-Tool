using Cloud_Migration_Tool.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cloud_Migration_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISecurePassword
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public System.Security.SecureString Password {
            get {
                return PasswordBox.SecurePassword;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };
            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                if((sender as Button).Name == "FilesToBeMigratedButton")
                {
                    FileCSVPathTextbox.Text = openFile.FileName;
                }
                else
                {
                    ProjectCSVPathTextbox.Text = openFile.FileName;
                }
            }
        }
    }
}
