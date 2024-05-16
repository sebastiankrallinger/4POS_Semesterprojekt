using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient
{
    class Chat
    {
        public string bezeichnung { get; set; }
        public string receiver { get; set; }
        public List<Message> messages { get; set; }

    }
}
