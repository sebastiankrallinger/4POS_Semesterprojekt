using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfClient
{
    class User
    {
        public string id {  get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<Chat> chats { get; set; }
    }
}
