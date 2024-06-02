using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfClient
{
    //Styling der verschiedenen Nachrichtentypen verwalten


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

        public MessageItem(string date)
        {
            Text = date;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6E6E6"));
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}
