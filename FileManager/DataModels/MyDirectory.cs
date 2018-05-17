using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// MyDirectory Class represents system directories
/// </summary>

namespace FileManager.DataModels
{
    public class MyDirectory : IDiscElement
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public MyDirectory(string name, string path)
        {
            Path = path;
            Name = name;
        }

        public int GetElementCount()
        {
            return GetSubElements().Count;
        }

        /// <summary>
        /// Gets child files and directories and adds them to a List of IDiscElement elements
        /// </summary>
        /// <returns></returns>

        public List<IDiscElement> GetSubElements()
        {
            List<IDiscElement> result = new List<IDiscElement>();
            DirectoryInfo d = new DirectoryInfo(this.Path);
            try
            {
                FileInfo[] fs = d.GetFiles();
                DirectoryInfo[] dirs = d.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    MyDirectory directory = new MyDirectory(dir.Name, dir.FullName);
                    result.Add(directory);
                }
                foreach (FileInfo file in fs)
                {
                    MyFile myfile = new MyFile(file.Name, file.FullName, file.Length);
                    result.Add(myfile);
                }
                return result;
            }
            catch(Exception e)
            {
                //Exception handler
                MessageBox.Show(e.ToString());
                return null; 
            }
        }

        public DateTime GetCreationDate()
        {
            return Directory.GetCreationTime(Path);
        }

        public string GetDescription()
        {
            return Path + " liczba elementów: " + GetElementCount();
        }

    }
}
