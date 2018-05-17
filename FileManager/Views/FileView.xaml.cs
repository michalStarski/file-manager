using FileManager.DataModels;
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

namespace FileManager.Views
{
    /// <summary>
    /// Logika interakcji dla klasy FileView.xaml
    /// </summary>
    public partial class FileView : UserControl
    {
        public IDiscElement file;

        public FileView(IDiscElement element)
        {
            InitializeComponent();

            if(element is MyFile)
            {
                MyFile file = element as MyFile;
                this.file = file;
                filename.Text = file.Name;
                filedate.Text = file.GetCreationDate().ToString();
                filesize.Text = file.Size.ToString();
                filetype.Text = @"<FILE>";
            }
            else if(element is MyDirectory)
            {
                MyDirectory directory = element as MyDirectory;
                this.file = directory;
                filename.Text = directory.Name;
                filedate.Text = directory.GetCreationDate().ToString();
                filesize.Text = "--";
                filetype.Text = @"<DIR>";
            }
        }
        /// <summary>
        /// Open a folder event, fires on double click
        /// </summary>
        /// <param name="directory"></param>
        public delegate void OpenDirectory(MyDirectory directory);
        public event OpenDirectory DirectoryOpenedEvent;

        public delegate void OpenFile(MyFile file);
        public event OpenFile FileOpenedEvent;

        public delegate void PreviewFile(MyFile file);
        public event PreviewFile FilePreviewedEvent;

        private void fileClicked(object sender, MouseButtonEventArgs e)
        {
            //Check for double click
            if (e.ClickCount > 1)
            {
                if (this.file is MyDirectory)
                {
                    //Invoking an event
                    if(DirectoryOpenedEvent != null) { DirectoryOpenedEvent.Invoke(this.file as MyDirectory); }
                }
                
                if (this.file is MyFile)
                {
                    if(FileOpenedEvent != null) { FileOpenedEvent.Invoke(this.file as MyFile);  }
                }
                    
            }
            else if (e.ClickCount == 1)
            {
                if (this.file is MyFile)
                {
                    if(FilePreviewedEvent != null) { FilePreviewedEvent.Invoke(this.file as MyFile);  }
                }
                else
                {
                    if(FilePreviewedEvent != null) { FilePreviewedEvent.Invoke(null); }
                }
            }
        }
    }
}
