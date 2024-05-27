using System;

namespace WpfClient
{
    class Chat
    {
        //Properties der Eigenschaften
        public string bezeichnung { get; set; }
        public string receiver { get; set; }
        public List<Message> messages { get; set; }

    }
}
