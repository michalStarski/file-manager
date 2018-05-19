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
        string fileType;
        public UpdateFile(string fromPath, string toPath, string fromElement, string toElement, string type)
        {
            InitializeComponent();
            From.Text = fromPath + @"\" + fromElement;
            To.Text = toPath + @"\" + toElement;
            this.fileType = type;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(CopyFile.IsChecked == true)
            {
                try
                {
                    if (fileType == "file")
                        File.Copy(From.Text, To.Text);
                    else
                        DirectoryCopy(From.Text, To.Text, true);

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
                    if (fileType == "file")
                        File.Move(From.Text, To.Text);
                    else
                        Directory.Move(From.Text, To.Text);

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

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

