using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.OleDb;
namespace TimeTracker
{
    class InitMainBorder
    {
        private Border retBorder;
        private string ovenName;
        private int baseHeight;
        private int baseWidth;

        public InitMainBorder(string inOvenName, int height, int width)
        {
            baseHeight = height;
            baseWidth = width;
            ovenName = inOvenName;
            retBorder = initMainBorder();
            //retBorder.Child = initSizeChange();
            //InitReadAllFromDataBase(); Ask someone about this!
        }

        private Border initMainBorder()
        {
            Border output = new Border();
            output.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#626262"));
            //output.Background = Brushes.DimGray;
            output.Height = baseHeight + 50;
            output.Width = baseWidth + 20;
            output.HorizontalAlignment = HorizontalAlignment.Stretch;
            output.VerticalAlignment = VerticalAlignment.Stretch;

            CornerRadius radi = output.CornerRadius;
            radi.BottomLeft = 10;
            radi.BottomRight = 10;
            radi.TopLeft = 10;
            radi.TopRight = 10;
            output.CornerRadius = radi;

            output.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8F9A8F"));
            //output.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#006E00"));
            //output.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03920f"));
            Thickness borderThickness = output.BorderThickness;
            borderThickness.Bottom = 2;
            borderThickness.Top = 2;
            borderThickness.Right = 2;
            borderThickness.Left = 2;
            output.BorderThickness = borderThickness;

            Thickness margin = output.Margin;
            margin.Bottom = 10;
            margin.Top = 10;
            margin.Right = 10;
            margin.Left = 10;
            output.Margin = margin;

            output.Name = ovenName;
            return output;
        }

        public Border getMainBorder()
        {
            return retBorder;
        }
    }
}
