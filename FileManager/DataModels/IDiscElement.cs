using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Disc Element Interface
/// GetCreationDate method returns creation date
/// GetDescription method returns prepared description about a file
/// </summary>

namespace FileManager.DataModels
{
    public interface IDiscElement
    {
        string Name { get; set; }
        string Path { get; set; } 

        DateTime GetCreationDate();
        string GetDescription();
    }
}
