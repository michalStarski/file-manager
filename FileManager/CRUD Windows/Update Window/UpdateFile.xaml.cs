using FileManager.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace FileManager.CRUD_Windows.Update_Window
{
    /// <summary>
    /// Logika interakcji dla klasy UpdateFile.xaml
    /// </summary>
    public partial class UpdateFile : Window
    {
        public UpdateFile(string fromPath, string toPath, string fromElement, string toElement)
        {
            InitializeComponent();
            From.Text = fromPath + @"\" + fromElement;
            To.Text = toPath + @"\" + toElement;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(CopyFile.IsChecked == true)
            {
                try
                {
                    File.Copy(From.Text, To.Text);

                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            }
            else if(MoveFile.IsChecked == true)
            {
                try
                {
                    File.Move(From.Text, To.Text);

                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please specify an operation");
            }
        }
    }
}
