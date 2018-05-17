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
using System.IO;

namespace FileManager.CRUD_Windows.Create_Window
{
    /// <summary>
    /// Logika klasy CreateFile.xaml
    /// </summary>
    public partial class CreateFile : Window
    {
        public CreateFile(string path)
        {
            InitializeComponent();

            CreatePath.Text = path + @"\";

            //Put both radiobuttons in one group
            DirectoryCheck.GroupName = "TypeCheck";
            FileCheck.GroupName = "TypeCheck";
        }

        /// <summary>
        /// Creates a file after button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CreateAFile(object sender, RoutedEventArgs e)
        {
            string toBeCreated = CreatePath.Text;
            try
            {
                if (FileCheck.IsChecked == true)
                {
                    File.Create(toBeCreated);
                }
                else if (DirectoryCheck.IsChecked == true)
                {
                    Directory.CreateDirectory(toBeCreated);
                }
                else
                {
                    MessageBox.Show("Please specify type");
                    this.Close();
                }
                this.Close();

            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
