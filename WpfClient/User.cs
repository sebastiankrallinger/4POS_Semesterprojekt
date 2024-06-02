using System;

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
