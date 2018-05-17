using FileManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Tools
{
    class CreationDateComparer : IComparer<IDiscElement>
    {
        public int Compare(IDiscElement x, IDiscElement y)
        {
            if (x.GetCreationDate() > y.GetCreationDate())
                return 1;
            else if (x.GetCreationDate() < y.GetCreationDate())
                return -1;
            else return 0;
        }
    }
}
