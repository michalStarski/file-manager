using FileManager.CRUD_Windows.Create_Window;
using FileManager.CRUD_Windows.Update_Window;
using FileManager.DataModels;
using FileManager.Tools;
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

namespace FileManager.Views
{
    /// <summary>
    /// Logic for FileBox.xaml
    /// </summary>
    public partial class FileBox : UserControl
    {
        public string currentPath;
        public string currentSelectedItem;
        public string currentSelectedItemType;

        private List<IDiscElement> currentList;

        private string nameSorted = "default";
        private string dateSorted = "default";

        //Preview Handler
        public delegate void ShowPreview(string source, string extension);
        public event ShowPreview PreviewEvent;

        public FileBox()
        {
            //Initialize FileBox

            InitializeComponent();
            GetDrives();
            //Delete Event
            ManagementPanel.DeleteFileEvent += FileDeleteOperation;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Dispatcher.Invoke(()=> DisplayFiles(currentPath));
        }
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Dispatcher.Invoke(() => DisplayFiles(currentPath));
        }

        /// <summary>
        /// Gets a list of logical drives
        /// </summary>
        /// 
        public void GetDrives()
        {
            foreach(DriveInfo drive in System.IO.DriveInfo.GetDrives())
            {
                //Adds drive names to a combobox
                DriveList.Items.Add(drive.Name.ToString());
            }
        }

        private void DisplayFiles(string path)
        {
            //Clear the listbox first
            FileList.Items.Clear();

            //Set the current path
            currentPath = path;
            CurrentPath.Text = path;

            //Initialize FileWatcher
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = currentPath;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;

            //Create an DirectoryInfo object to extract child files from it
            DirectoryInfo d = new DirectoryInfo(path);
            MyDirectory directory = new MyDirectory(d.Name, d.FullName);
            currentList = directory.GetSubElements();
            //Iterate in directory
            try
            {
                foreach (IDiscElement element in directory.GetSubElements()) 
                {
                    //For each element create an view to describe it
                    FileView display = new FileView(element);
                    //Subscribing to an event, if invoked execute FileView_DisplayFilesEvent;
                    display.DirectoryOpenedEvent += FileView_DisplayFilesEvent;
                    //File open on doubleclick
                    display.FileOpenedEvent += FileView_OpenFileEvent;
                    //File Preview event
                    display.FilePreviewedEvent += FileView_PreviewFile;
                    //And add it to the list
                    FileList.Items.Add(display);
                }

            }
            catch(Exception e)
            {
                //Error Box
                MessageBox.Show(e.ToString());
                DisplayFiles(DriveList.SelectedItem.ToString());
                return;
            }
        }


        private void FileView_DisplayFilesEvent(MyDirectory directory)
        {
            DisplayFiles(directory.Path);
        }

        /// <summary>
        /// Opens a file in default program, if .txt or [.jpg, .png, .gif, .bmp] opens in a created preview box
        /// </summary>
        /// <param name="file"></param>
        private void FileView_OpenFileEvent(MyFile file)
        {
            System.Diagnostics.Process.Start(file.Path);
        }

        private void FileView_PreviewFile(MyFile file)
        {
            if (file == null)
            {
                if (PreviewEvent != null)
                    PreviewEvent.Invoke(null, null);
            }
            else
            {
                string extension = System.IO.Path.GetExtension(file.Path);
                string[] previewableFiles = new string[] { ".txt", ".jpg", ".png", ".gif", ".bmp" };
                if (previewableFiles.Contains(extension))
                {
                    if (PreviewEvent != null)
                        PreviewEvent.Invoke(file.Path, extension);
                    else
                    {
                        if (PreviewEvent != null)
                            PreviewEvent.Invoke(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// Displays files after drive selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DriveList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Get info about drive to display it in textblock next to the combobox
            DriveInfo drive = new DriveInfo(DriveList.SelectedItem.ToString());

            //Prepare the info
            string info = drive.Name + " " + "Free " + (drive.TotalFreeSpace/1024).ToString() + " KB" + " of " + (drive.TotalSize/1024).ToString() + " KB";
            DriveInfo.Text = info;

            DisplayFiles(drive.Name);
        }

        /// <summary>
        /// Displays info at the bottom about selected file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Display prepared info at the bottom
            SelectedInfo.Text = "Selected: " + (FileList.SelectedIndex+1).ToString() + " of " + FileList.Items.Count;
            FileView selectedItem = FileList.SelectedItem as FileView;
            try
            {
                if (selectedItem.file is MyDirectory)
                {
                    currentSelectedItem = (selectedItem.file as MyDirectory).Name;
                    currentSelectedItemType = "dir";
                }
                else
                {
                    currentSelectedItem = (selectedItem.file as MyFile).Name;
                    currentSelectedItemType = "file";
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Going up one level after clicking a go back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoUp(object sender, RoutedEventArgs e)
        {
            //take parent of a current directory
            DirectoryInfo parent = Directory.GetParent(currentPath);
            if (parent != null)
            {
                //if not null proceed
                DisplayFiles(parent.FullName);
            }
        }
 
        /// <summary>
        /// Deletes a file
        /// </summary>

        private void FileDeleteOperation()
        {
            FileView toDelete = (FileList.SelectedItem as FileView);
            try
            {
                if(toDelete.file is MyFile)
                {
                    File.Delete((toDelete.file as MyFile).Path);
                }
                else if (toDelete.file is MyDirectory)
                {
                    Directory.Delete((toDelete.file as MyDirectory).Path);
                }
                MessageBox.Show("Deleted");

            }catch(NullReferenceException nEx)
            {
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //Sorts files by Name on button click
        private void SortByName(object sender, RoutedEventArgs e)
        {
            FileList.Items.Clear();

            NameComparer myNameComparer = new NameComparer();

            List<IDiscElement> sortedByName = currentList;
            //Application has 3 sorted by date states - name, nameReversed and default
            //States switches if different type of sort is chosen
            switch (nameSorted)
            {
                case "default":
                    sortedByName.Sort(myNameComparer);
                    nameSorted = "name";
                    break;
                case "name":
                    sortedByName.Sort(myNameComparer);
                    sortedByName.Reverse();
                    nameSorted = "nameReversed";
                    break;
                case "nameReversed":
                    DisplayFiles(currentPath);
                    nameSorted = "default";
                    return;
            }
            try
            {
                foreach (IDiscElement element in sortedByName)
                {
                    //For each element create an view to describe it
                    FileView display = new FileView(element);
                    //Subscribing to an event, if invoked execute FileView_DisplayFilesEvent;
                    display.DirectoryOpenedEvent += FileView_DisplayFilesEvent;
                    //File open on doubleclick
                    display.FileOpenedEvent += FileView_OpenFileEvent;
                    //File Preview event
                    display.FilePreviewedEvent += FileView_PreviewFile;
                    //And add it to the list
                    FileList.Items.Add(display);
                }
            }
            catch (Exception ex)
            {
                //Error Box
                MessageBox.Show(ex.ToString());
                DisplayFiles(DriveList.SelectedItem.ToString());
                return;
            }
        }
        /// <summary>
        /// Sorts files by Date on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortbyDate(object sender, RoutedEventArgs e)
        {
            FileList.Items.Clear();

            CreationDateComparer myDateComparer = new CreationDateComparer();

            List<IDiscElement> sortedByDate = currentList;
            //Application has 3 sorted by date states - date, dateReversed and default
            //State switches if different type of sort is chosen
            switch (dateSorted)
            {
                case "default":
                    sortedByDate.Sort(myDateComparer);
                    dateSorted = "date";
                    break;
                case "date":
                    sortedByDate.Sort(myDateComparer);
                    sortedByDate.Reverse();
                    dateSorted = "dateReversed";
                    break;
                case "dateReversed":
                    DisplayFiles(currentPath);
                    dateSorted = "default";
                    return;
            }
            try
            {
                foreach (IDiscElement element in sortedByDate)
                {
                    //For each element create an view to describe it
                    FileView display = new FileView(element);
                    //Subscribing to an event, if invoked execute FileView_DisplayFilesEvent;
                    display.DirectoryOpenedEvent += FileView_DisplayFilesEvent;
                    //File open on doubleclick
                    display.FileOpenedEvent += FileView_OpenFileEvent;
                    //File Preview event
                    display.FilePreviewedEvent += FileView_PreviewFile;
                    //And add it to the list
                    FileList.Items.Add(display);
                }
            }
            catch (Exception ex)
            {
                //Error Box
                MessageBox.Show(ex.ToString());
                DisplayFiles(DriveList.SelectedItem.ToString());
                return;
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            FileList.Items.Clear();

            if(String.Compare(SearchBox.Text, "") == 0)
            {
                DisplayFiles(currentPath);
            }
            List<IDiscElement> fileList = currentList.Where(element => element.Name.Contains(SearchBox.Text)).ToList();
            try
            {
                foreach (IDiscElement element in fileList)
                {
                    //For each element create an view to describe it
                    FileView display = new FileView(element);
                    //Subscribing to an event, if invoked execute FileView_DisplayFilesEvent;
                    display.DirectoryOpenedEvent += FileView_DisplayFilesEvent;
                    //File open on doubleclick
                    display.FileOpenedEvent += FileView_OpenFileEvent;
                    //File Preview event
                    display.FilePreviewedEvent += FileView_PreviewFile;
                    //And add it to the list
                    FileList.Items.Add(display);
                }
            }catch(Exception ex)
            {
                //Error Box
                MessageBox.Show(ex.ToString());
                DisplayFiles(DriveList.SelectedItem.ToString());
                return;
            }
        }
    }
}
