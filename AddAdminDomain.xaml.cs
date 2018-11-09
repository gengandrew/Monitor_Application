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
    /// Interaction logic for AddAdminDomain.xaml
    /// </summary>
    public partial class AddAdminDomain : Window
    {
        public AddAdminDomain()
        {
            InitializeComponent();
        }

        private void SubmitHelper()
        {
            string domainName = Block_DomainName.Text;
            if (domainName.Length > 3)
            {
                if (MessageBox.Show(string.Format("Are you sure you want to Add the Domain '{0}'?", domainName),
                    "Add Admin Domain", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Server.InsertAdminDomain(domainName);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Domain Name Invalid Please Try Again");
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