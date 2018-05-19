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
    /// Logic for ManagementPanel.xaml
    /// </summary>
    public partial class ManagementPanel : UserControl
    {

        public ManagementPanel()
        {
            InitializeComponent();
        }
        //Events are being invoked on management panel's button click

        //Create a file Event 
        public delegate void CreateNewFile();
        public static event CreateNewFile CreateFileEvent;

        //Delete a file Event
        public delegate void DeleteAFile();
        public static event DeleteAFile DeleteFileEvent;

        //Update a file Event
        public delegate void UpdateAFile();
        public static event UpdateAFile UpdateFileEvent;


        //Invoking methods
        private void CreateFile(object sender, RoutedEventArgs e)
        {
            if (CreateFileEvent != null)
            {
                CreateFileEvent.Invoke();
            }
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            if (DeleteFileEvent != null)
                DeleteFileEvent.Invoke();
        }

        private void UpdateFile(object sender, RoutedEventArgs e)
        {
            if (UpdateFileEvent != null)
                UpdateFileEvent.Invoke();
        }
    }
}
