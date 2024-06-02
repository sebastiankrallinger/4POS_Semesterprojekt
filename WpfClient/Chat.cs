using System;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfClient
{
    class Chat
    {
        //Properties der Eigenschaften
        public string bezeichnung { get; set; }
        public string receiver { get; set; }
        public bool newMsg { get; set; }
        public List<Message> messages { get; set; }

        public Brush BackgroundColor { get; set; } = Brushes.Transparent;
    }
}
