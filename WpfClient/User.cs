using System;

namespace WpfClient
{
    class User
    {
        //Properties der Eigenschaften
        public string id {  get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<Chat> chats { get; set; }
    }
}
