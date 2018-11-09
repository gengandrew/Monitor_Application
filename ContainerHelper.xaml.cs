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

using System.Data.OleDb;
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for ContainerHelper.xaml
    /// </summary>
    public partial class ContainerHelper : Window
    {
        private char row;
        private int column;
        private bool isSubmit;
        private string RecipeName;
        private string ContainerNums;
        private string comments;
        private int[] ProgressionID;
        private int RecipeKey;

        public ContainerHelper(char inRow, int inColumn)
        {
            this.row = inRow;
            this.column = inColumn;
            InitializeComponent();
            InitLocationID();
            FillComboBox();
        }

        private void FillComboBox()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM RecipePointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                string temp = scnr["Name"].ToString();
                ComboBox.Items.Add(temp);
            }
            MainWindow.connect.Close();
        }

        private void InitLocationID()
        {
            LocationID.Text = row.ToString() + (column + 1).ToString();
        }

        private void SubmitHelper()
        {
            RecipeName = ComboBox.Text;
            ContainerNums = Container.Text;
            comments = Comments.Text;
            if (RecipeName.Length == 0)
            {
                MessageBox.Show("Invalid Recipe Choosen, Please Try Again.");
            }
            else
            {
                string output = "";
                OleDbCommand command = new OleDbCommand();
                MainWindow.connect.Open();
                command.Connection = MainWindow.connect;
                string query = "SELECT * FROM RecipePointer;";
                command.CommandText = query;
                OleDbDataReader scnr = command.ExecuteReader();
                while (scnr.Read())
                {
                    string temp = scnr["Name"].ToString();
                    if (temp.Equals(RecipeName))
                    {
                        output = scnr["ProgressionID"].ToString();
                        break;
                    }
                }
                MainWindow.connect.Close();
                ProgressionID = Conversions.progressionIDToArray(output);
                isSubmit = true;
                initRecipeKey();
                this.Close();
            }
        }

        private void initRecipeKey()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM RecipePointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                string temp = scnr["Name"].ToString();
                if (temp.Equals(RecipeName))
                {
                    this.RecipeKey = (int)scnr["RecipeKey"];
                    break;
                }
            }
            MainWindow.connect.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SubmitHelper();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            isSubmit = false;
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

        public bool getIsSubmit()
        {
            return this.isSubmit;
        }

        public string getRecipeName()
        {
            return this.RecipeName;
        }

        public int[] getProgressionID()
        {
            return this.ProgressionID;
        }

        public string getContainerNums()
        {
            return this.ContainerNums;
        }

        public int getRecipeKey()
        {
            return this.RecipeKey;
        }

        public string getComments()
        {
            return this.comments;
        }
    }
}