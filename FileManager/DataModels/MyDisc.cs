using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// MyDisc Class represents system logical drives
/// </summary>


namespace FileManager.DataModels
{
    class MyDisc
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public long AvaliableSpace { get; set; }

        public MyDisc(string name, long size, long avaliablespace)
        {
            Name = name;
            Size = size;
            AvaliableSpace = avaliablespace;
        }

        //Returns drive child elements

        public List<IDiscElement> GetDriveElements()
        {
            List<IDiscElement> result = new List<IDiscElement>();
            DirectoryInfo drive = new DirectoryInfo(this.Name);
            FileInfo[] fs = drive.GetFiles();
            DirectoryInfo[] dirs = drive.GetDirectories();
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
    }
}
