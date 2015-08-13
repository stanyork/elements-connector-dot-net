using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Element_Test_Form
{
    public class EmptyFolderOptions
    {
        public bool SingleFileOK = true;
        public bool PathCheck = true;
        public ulong  SingleFileSizeUnder = 999;
        public string SingleFileType = ".htm";

        public string PathMustContain = "/rfi/";
    }
}
