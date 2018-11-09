using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.OleDb;
namespace TimeTracker
{
    class Icons
    {
        private Image blue;
        private Image green;
        private Image yellow;
        private Image red;

        public Icons()
        {
            InitImages();
        }

        private void InitImages()
        {
            BitmapImage img1 = new BitmapImage();
            img1.BeginInit();
            img1.UriSource = new Uri("/TimeTracker;component/Images/Blue.png", UriKind.Relative);
            img1.EndInit();
            blue = new Image();
            blue.Source = img1;
            blue.VerticalAlignment = VerticalAlignment.Top;
            blue.HorizontalAlignment = HorizontalAlignment.Right;
            blue.Width = 18;
            blue.Height = 18;
            Thickness bmargin = blue.Margin;
            bmargin.Top = 3;
            bmargin.Right = 3;
            blue.Margin = bmargin;

            BitmapImage img2 = new BitmapImage();
            img2.BeginInit();
            img2.UriSource = new Uri("/TimeTracker;component/Images/Green.png", UriKind.Relative);
            img2.EndInit();
            green = new Image();
            green.Source = img2;
            green.VerticalAlignment = VerticalAlignment.Top;
            green.HorizontalAlignment = HorizontalAlignment.Right;
            green.Width = 18;
            green.Height = 18;
            Thickness gmargin = green.Margin;
            gmargin.Top = 3;
            gmargin.Right = 3;
            green.Margin = gmargin;

            BitmapImage img3 = new BitmapImage();
            img3.BeginInit();
            img3.UriSource = new Uri("/TimeTracker;component/Images/Yellow.png", UriKind.Relative);
            img3.EndInit();
            yellow = new Image();
            yellow.Source = img3;
            yellow.VerticalAlignment = VerticalAlignment.Top;
            yellow.HorizontalAlignment = HorizontalAlignment.Right;
            yellow.Width = 18;
            yellow.Height = 18;
            Thickness ymargin = yellow.Margin;
            ymargin.Top = 3;
            ymargin.Right = 3;
            yellow.Margin = ymargin;

            BitmapImage img4 = new BitmapImage();
            img4.BeginInit();
            img4.UriSource = new Uri("/TimeTracker;component/Images/Red.png", UriKind.Relative);
            img4.EndInit();
            red = new Image();
            red.Source = img4;
            red.VerticalAlignment = VerticalAlignment.Top;
            red.HorizontalAlignment = HorizontalAlignment.Right;
            red.Width = 18;
            red.Height = 18;
            Thickness rmargin = red.Margin;
            rmargin.Top = 3;
            rmargin.Right = 3;
            red.Margin = rmargin;
        }

        public Image getBlue()
        {
            return blue;
        }

        public Image getGreen()
        {
            return green;
        }

        public Image getYellow()
        {
            return yellow;
        }

        public Image getRed()
        {
            return red;
        }
    }
}
