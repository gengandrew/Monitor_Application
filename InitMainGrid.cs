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
using MaterialDesignThemes;
namespace TimeTracker
{
    class InitMainGrid
    {
        private Grid retGrid;
        private string ovenName;
        private int baseHeight;
        private int baseWidth;

        public delegate void myEventHandler(string inOvenName);
        public event myEventHandler ChangeSizeClick;
        public event myEventHandler DeleteClick;
        public event myEventHandler HelpClick;

        public InitMainGrid(string inOvenName, int height, int width)
        {
            baseHeight = height;
            baseWidth = width;
            ovenName = inOvenName;
            retGrid = initMainGrid(new Grid());
            retGrid.Children.Add(initSizeChange());
            retGrid.Children.Add(initDeleteButton());
            retGrid.Children.Add(initHelpButton());
            retGrid.Children.Add(initHeaderText());
        }

        private TextBlock initHeaderText()
        {
            TextBlock text = new TextBlock();
            text.VerticalAlignment = VerticalAlignment.Top;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Text = ovenName;
            text.Height = 20;
            text.Width = Double.NaN;
            text.FontSize = 13;
            text.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            return text;
        }

        private Button initSizeChange()
        {
            Button changeSize = new Button();
            changeSize.VerticalAlignment = VerticalAlignment.Top;
            changeSize.HorizontalAlignment = HorizontalAlignment.Right;
            changeSize.Name = "ChangeSize";
            //changeSize.Content = "Oven Specifics";
            //changeSize.Content = MaterialDesignThemes.Wpf.PackIconKind.AccountEdit;
            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
            packIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.TableEdit;
            packIcon.Width = 18;
            packIcon.Height = 18;
            packIcon.VerticalAlignment = VerticalAlignment.Center;
            packIcon.HorizontalAlignment = HorizontalAlignment.Center;
            changeSize.Content = packIcon;
            Thickness margin = changeSize.Margin;
            margin.Right = 25;
            changeSize.Margin = margin;
            changeSize.Height = 20;
            changeSize.Width = 20;
            changeSize.Background = Brushes.LightSlateGray;
            changeSize.Click += ChangeSize_Click;
            return changeSize;
        }

        private Button initDeleteButton()
        {
            Button delButton = new Button();
            delButton.VerticalAlignment = VerticalAlignment.Top;
            delButton.HorizontalAlignment = HorizontalAlignment.Right;
            delButton.Width = 20;
            delButton.Height = 20;
            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
            packIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
            packIcon.Width = 18;
            packIcon.Height = 18;
            packIcon.VerticalAlignment = VerticalAlignment.Center;
            packIcon.HorizontalAlignment = HorizontalAlignment.Center;
            delButton.Content = packIcon;
            //delButton.Content = "X";
            //delButton.SetValue(Button.FontWeightProperty, FontWeights.Bold);
            //delButton.Foreground = Brushes.Black;
            delButton.Background = Brushes.LightSlateGray;
            delButton.Click += Delete_Click;
            return delButton;
        }

        private Button initHelpButton()
        {
            Button helpButton = new Button();
            helpButton.VerticalAlignment = VerticalAlignment.Top;
            helpButton.HorizontalAlignment = HorizontalAlignment.Right;
            helpButton.Width = 20;
            helpButton.Height = 20;
            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
            packIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Help;
            packIcon.Width = 18;
            packIcon.Height = 18;
            packIcon.VerticalAlignment = VerticalAlignment.Center;
            packIcon.HorizontalAlignment = HorizontalAlignment.Center;
            helpButton.Content = packIcon;
            Thickness margin = helpButton.Margin;
            margin.Right = 50;
            helpButton.Margin = margin;
            helpButton.Background = Brushes.LightSlateGray;
            helpButton.Click += HelpButton_Click;
            return helpButton;
        }

        private Grid initMainGrid(Grid mainGrid)
        {
            mainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#626262"));
            //mainGrid.Background = Brushes.DimGray;
            mainGrid.Height = baseHeight + 30;
            mainGrid.Width = baseWidth;
            mainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            mainGrid.VerticalAlignment = VerticalAlignment.Stretch;
            return mainGrid;
        }

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            ChangeSizeClick(ovenName);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteClick(ovenName);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpClick(ovenName);
        }

        public Grid getMainGrid()
        {
            return retGrid;
        }
    }
}
