using Microsoft.Win32;
using Scanner;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string inf { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        public void SelectFile(object sender,RoutedEventArgs e)
        {
            string filename;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text documents (.txt)|*.txt";
            fileDialog.Title = "Select File to Scane";
            bool? result = fileDialog.ShowDialog();
            if (result == true)
                filename = fileDialog.FileName;
            else
                return;
            Scan a=new Scan(filename);
            a.Run();
            SSS.Text = a.Result;
        }
    }
}
