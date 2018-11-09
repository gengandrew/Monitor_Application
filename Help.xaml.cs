using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public static bool isChecked;

        public Help()
        {
            InitializeComponent();
            InitCheckBox();
        }

        private void InitCheckBox()
        {
            if (isChecked)
            {
                MessCheckBox.IsChecked = true;
            }
            else
            {
                MessCheckBox.IsChecked = false;
            }
        }

        private void MessCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox input = sender as CheckBox;
            string ovenName = input.Name;
            if ((bool)input.IsChecked)
            {
                isChecked = true;
            }
            else
            {
                isChecked = false;
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragEvent(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
