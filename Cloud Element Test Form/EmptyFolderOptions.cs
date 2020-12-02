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
        public bool SingleFileTagRequired = true;
        public ulong SingleFileSizeUnder = 999;
        public string SingleFileType = ".htm";
        public double  SingleFileAgeInHours = 12.0;

        public string PathMustContain = "/rfi/";
        public System.Collections.Specialized.StringCollection CheckFolders;
    }
}
