using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfClient
{
    class Message
    {
        public string message { get; set; }
        public bool receiver { get; set; }
        public string date { get; set; }
    }
}
