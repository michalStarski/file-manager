using FileManager.CRUD_Windows.Create_Window;
using FileManager.CRUD_Windows.Update_Window;
using FileManager.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManager
{
    /// <summary>
    /// Logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Create, Update Events
            ManagementPanel.UpdateFileEvent += InvokeUpdateFileWindow;
            ManagementPanel.CreateFileEvent += InvokeCreateFileWindow;


            LeftPanel.PreviewEvent += PreviewElement;
        }
        /// <summary>
        /// Update Window takes left path as current path and right path as a new destination
        /// </summary>
        private void InvokeUpdateFileWindow()
        {
            UpdateFile UpdateWindow = new UpdateFile(LeftPanel.currentPath, RightPanel.currentPath, LeftPanel.currentSelectedItem, RightPanel.currentSelectedItem, LeftPanel.currentSelectedItemType);
            UpdateWindow.Show();
        }


        /// <summary>
        /// Opens a create file window
        /// </summary>

        private void InvokeCreateFileWindow()
        {
            CreateFile CreateWindow = new CreateFile(LeftPanel.currentPath);
            CreateWindow.Show();
        }

        /// <summary>
        /// Displays text or image element on the right hand side
        /// </summary>
        /// <param name="source"></param>

        private void PreviewElement(string source, string extension)
        {
            Preview.Children.Clear();

            if(source == null && extension == null)
            {
                Label notAvaliable = new Label();
                notAvaliable.Content = "Preview not avaliable";
                notAvaliable.VerticalAlignment = VerticalAlignment.Center;
                notAvaliable.HorizontalAlignment = HorizontalAlignment.Center;
                notAvaliable.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABB2BF"));
                Preview.Children.Add(notAvaliable);
            }

            string[] images = new string[] { ".jpg", ".bmp", ".png", ".gif" };
            if (images.Contains(extension))
            {
                Image img = new Image();
                ImageSource src = new BitmapImage(new Uri(source, UriKind.RelativeOrAbsolute));
                img.Source = src;
                Preview.Children.Add(img);
            }
            else if(extension == ".txt")
            {
                RichTextBox textPreview = new RichTextBox();
                string content = File.ReadAllText(source);
                textPreview.AppendText(content);
                Preview.Children.Add(textPreview);
            }
        }
    }
}
