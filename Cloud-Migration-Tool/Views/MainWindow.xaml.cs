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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };

            
            
            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show($"You selected {openFile.FileName}");
                if((sender as Button).Name == "FilesToBeMigratedButton")
                {
                    MessageBox.Show("This is a files to be migrated file lookup");
                }
                else
                {
                    MessageBox.Show("This is a projects to be migrated file lookup");
                }
            }
        }
    }
}
