using FileManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Tools
{
    class NameComparer : IComparer<IDiscElement>
    {
        public int Compare(IDiscElement x, IDiscElement y)
        {
            if (String.Compare(x.Name, y.Name) > 0)
                return 1;
            else if (String.Compare(x.Name, y.Name) < 0)
                return -1;
            else
                return 0;
        }
    }
}
