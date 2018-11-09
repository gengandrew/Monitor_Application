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
    /// Interaction logic for OvenSpecifics.xaml
    /// </summary>
    public partial class OvenSpecifics : Window
    {
        private string ovenName;
        private int currRowSize;
        private int currColSize;
        private bool sizeChange;

        public OvenSpecifics(string inOvenName)
        {
            sizeChange = false;
            ovenName = inOvenName;
            currColSize = Server.getColumnSizeFromDB(ovenName);
            currRowSize = Server.getRowSizeFromDB(ovenName);
            InitializeComponent();
            InitAdminPrivileges();
            InitBlocks();
        }

        private void InitBlocks()
        {
            Block_OvenName.Text = " " + ovenName + " ";
            Block_ColCount.Text = " " + currColSize + " ";
            Block_RowCount.Text = " " + currRowSize.ToString() + " ";
            Block_Recipes.Text = InitRecipeBlock();
            Block_TimeStamps.Text = InitTimestampBlock();
        }

        private void InitAdminPrivileges()
        {
            if (MainWindow.isAdmin)
            {
                Block_ColCount.IsReadOnly = false;
                Block_RowCount.IsReadOnly = false;
            }
        }

        private string InitTimestampBlock()
        {
            string output = "";
            int counter = 0;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT COUNT(DISTINCT ProgressionID) as count from Recipes;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                counter = Int32.Parse(scnr["count"].ToString());
            }
            scnr.Close();
            MainWindow.connect.Close();
            for (int i = 0; i < counter; i++)
            {
                string temp = "";
                TimeSpan[] mySpans = Conversions.DateTimeToTimeSpan(Server.getLimits(i + 1));
                if (i != counter - 1)
                {
                    temp = temp + " (" + (i + 1) + ") | " + mySpans[0].ToString() + ", " + mySpans[1].ToString() + ", " + mySpans[2].ToString() + " | " + Server.getLastUser(i + 1) + "\n";
                }
                else
                {
                    temp = temp + " (" + (i + 1) + ") | " + mySpans[0].ToString() + ", " + mySpans[1].ToString() + ", " + mySpans[2].ToString() + " | " + Server.getLastUser(i + 1);
                }
                output = output + temp;
            }
            return output;
        }

        private string InitRecipeBlock()
        {
            string output = "";
            string[] RecipeNames = Server.getRecipeName();
            for (int i = 0; i < RecipeNames.Length; i++)
            {
                string temp = "";
                if (i != RecipeNames.Length - 1 && RecipeNames[i] != null)
                {
                    temp = temp + " (" + (i + 1) + ") | " + RecipeNames[i] + " \n";
                }
                else if (RecipeNames[i] != null)
                {
                    temp = temp + " (" + (i + 1) + ") | " + RecipeNames[i];
                }
                output = output + temp;
            }
            return output;
        }

        private void SubmitHelper()
        {
            int newColCount = Int32.Parse(Block_ColCount.Text);
            int newRowCount = Int32.Parse(Block_RowCount.Text);
            if (newColCount >= 3 && newRowCount >= 3)
            {
                if (MessageBox.Show(string.Format("Are you sure you want to change the size of '{0}'?", ovenName),
                    "Remove Recipe", MessageBoxButton.YesNo) == MessageBoxResult.Yes
                    && (currColSize != newColCount || currRowSize != newRowCount))
                {
                    InitBaseGrid.columnCount = newColCount;
                    InitBaseGrid.rowCount = newRowCount;
                    Server.resetOven(ovenName, newRowCount, newColCount);
                    sizeChange = true;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Oven Size Specified, Please Try Again.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            sizeChange = false;
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SubmitHelper();
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

        public bool getSizeChange()
        {
            return sizeChange;
        }
    }
}
