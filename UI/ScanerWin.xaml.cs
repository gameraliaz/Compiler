using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for ScanerWin.xaml
    /// </summary>
    public partial class ScanerWin : Window
    {
        public ScanerWin()
        {
            InitializeComponent();
        }
        private void DragMoveMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseMouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void MaximizeMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void MinimizeMouseUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new() { Title = "Select a file to compile" };
            string filePath;
            if (fd.ShowDialog() == true)
            {
                Scanner.Scan scan;
                filePath = fd.FileName;
                scan = new(filePath);
                scan.Run();
                dg_SymbolTable.ItemsSource = scan.SymbolsTable;
                rtb_Output.Document.Blocks.Clear();
                rtb_Output.Document.Blocks.Add(new Paragraph(new Run(scan.Result.Trim())));
            }
            }

    }
}
