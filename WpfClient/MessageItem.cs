using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfClient
{
    internal class MessageItem
    {
        public Brush Background { get; set; }
        public string Text { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public MessageItem(string message, bool receiver, string date)
        {
            string[] datTime = date.Split(' ');
            string time = datTime[1];
            Text = message + " \n" + time;
            if (receiver == false)
            {
                Background = Brushes.LightGreen;
            }
            else
            {
                Background = Brushes.LightGray;
                HorizontalAlignment = HorizontalAlignment.Right;
            }
            
        }
    }
}
