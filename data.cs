using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPTtoUSB_Converter
{
    public class Data
    {
        private string _folder;
        public string Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        private string _comPort;
        public string ComPort
        {
            get { return _comPort; }
            set { _comPort = value; }
        }


    }
}
