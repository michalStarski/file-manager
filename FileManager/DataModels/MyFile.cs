using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// MyFile Class represents system files
/// </summary>

namespace FileManager.DataModels
{
    public class MyFile : IDiscElement
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public MyFile(string name, string path, long size)
        {
            Name = name;
            Path = path;
            Size = size;
        }

        public DateTime GetCreationDate()
        {
            return File.GetCreationTime(Path);
        }

        public string GetDescription()
        {
            return Path + "wielkość: " + Size + " b";
        }
    }
}
