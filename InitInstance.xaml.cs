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

using System.Collections;
using System.Data.OleDb;
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for InitInstance.xaml
    /// </summary>
    public partial class InitInstance : Window
    {
        private bool isSubmit;
        private string ovenName;
        public static ArrayList prevOvens = new ArrayList();

        public InitInstance()
        {
            isSubmit = false;
            InitializeComponent();
            FillComboBox();
        }

        private void SubmitHelper()
        {
            if (prevOvens.Contains(ComboBox.Text))
            {
                MessageBox.Show("Specific instance already exists, please choose another.");
            }
            else
            {
                isSubmit = true;
                ovenName = ComboBox.Text;
                if (ovenName.Length == 0)
                {
                    MessageBox.Show("Invalid Oven Name, Please Try Again");
                }
                else
                {
                    prevOvens.Add(ovenName);
                    this.Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            isSubmit = false;
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SubmitHelper();
        }

        private void FillComboBox()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM OvenPointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                string temp = scnr["ovenName"].ToString();
                ComboBox.Items.Add(temp);
            }
            MainWindow.connect.Close();
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

        public bool getIsSubmit()
        {
            return this.isSubmit;
        }

        public string getOvenName()
        {
            return this.ovenName;
        }
    }
}
