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
    /// Interaction logic for AddOven.xaml
    /// </summary>
    public partial class AddOven : Window
    {
        public AddOven()
        {
            InitializeComponent();
        }

        private void SubmitHelper()
        {
            try
            {
                string ovenName = Block_OvenName.Text;
                int rowCount = Int32.Parse(Block_RowCount.Text);
                int colCount = Int32.Parse(Block_ColCount.Text);
                if (ovenName.Length > 3 && rowCount >= 3 && colCount >= 3)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to Add the Oven '{0}'?", Block_OvenName.Text),
                        "Add Oven", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Server.makeOven(ovenName, rowCount, colCount);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Row Count, Column Count, or Oven Name is Invalid, Please Try Again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Row Count, Column Count, or Oven Name is Invalid, Please Try Again.");
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SubmitHelper();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragEvent(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Submit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SubmitHelper();
            }
        }
    }
}