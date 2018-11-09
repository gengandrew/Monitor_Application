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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for SearchEngineView.xaml
    /// </summary>
    public partial class SearchEngineView : Window
    {
        private ObservableCollection<SearchElements> elements { get; set; }
        private ArrayList ovenNames;
        private SearchHelp helpBar;

        public SearchEngineView(string inValues)
        {
            helpBar = new SearchHelp();
            elements = new ObservableCollection<SearchElements>();
            ovenNames = new ArrayList();
            InitOvenNames();
            InitializeComponent();
            SearchValues.Text = inValues;
            SearchBody.ItemsSource = elements;
            searchClickHelper();
        }

        private void InitOvenNames()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM OvenPointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                string temp = scnr["OvenName"].ToString();
                ovenNames.Add(temp);
            }
            MainWindow.connect.Close();
        }

        private void SearchHelperIterate(string values)
        {
            elements = new ObservableCollection<SearchElements>();
            for (int i = 0; i < ovenNames.Count; i++)
            {
                string ovenName = ovenNames[i] as string;
                SearchHelper(ovenName, values);
            }
        }

        private void SearchHelper(string ovenName, string values)
        {
            int row = Server.getRowSizeFromDB(ovenName);
            int col = Server.getColumnSizeFromDB(ovenName);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    string containerTemp = Server.getContainerNums(ovenName, Conversions.intToChar(i), j);
                    string commentTemp = Server.getComments(ovenName, Conversions.intToChar(i), j);
                    if (Search(containerTemp, values) > 0.8)
                    {
                        SearchElements newElement = new SearchElements();
                        /*
                        TextBlock newText = new TextBlock();
                        Run newRun = new Run(values);
                        Hyperlink link = new Hyperlink();
                        link.Click += Link_Click;
                        newText.Inlines.Add(newRun);
                        newText.Inlines.Add(link);
                        newElement.containerNums = newText;
                        */
                        newElement.row = Conversions.intToChar(i);
                        newElement.column = j;
                        newElement.containerNumsString = values;
                        newElement.locationID = Conversions.intToChar(i).ToString() + j.ToString();
                        newElement.recipeName = Server.getRecipeName(Server.getRecipeKey(ovenName, Conversions.intToChar(i), j));
                        newElement.ovenName = ovenName;
                        DateTime[] limits = Server.getLimits(Conversions.progressionIDToArray(Server.getProgressionID(newElement.recipeName))[0]);
                        newElement.readyTime = Server.getStartTime(ovenName, Conversions.intToChar(i), j).Add(Conversions.DateTimeToTimeSpan(limits[0]));
                        elements.Add(newElement);
                        SearchBody.ItemsSource = elements;
                        SearchBody.Items.Refresh();
                    }
                    else if (Search(commentTemp, values) > 0.8)
                    {
                        SearchElements newElement = new SearchElements();
                        /*
                        TextBlock newText = new TextBlock();
                        Run newRun = new Run(values);
                        Hyperlink link = new Hyperlink();
                        link.Click += Link_Click;
                        newText.Inlines.Add(newRun);
                        newText.Inlines.Add(link);
                        newElement.comments = newText;
                        */
                        newElement.row = Conversions.intToChar(i);
                        newElement.column = j;
                        newElement.commentString = values;
                        newElement.locationID = Conversions.intToChar(i).ToString() + j.ToString();
                        newElement.recipeName = Server.getRecipeName(Server.getRecipeKey(ovenName, Conversions.intToChar(i), j));
                        newElement.ovenName = ovenName;
                        DateTime[] limits = Server.getLimits(Conversions.progressionIDToArray(Server.getProgressionID(newElement.recipeName))[0]);
                        newElement.readyTime = Server.getStartTime(ovenName, Conversions.intToChar(i), j).Add(Conversions.DateTimeToTimeSpan(limits[0]));
                        elements.Add(newElement);
                        SearchBody.ItemsSource = elements;
                        SearchBody.Items.Refresh();
                    }
                }
            }
        }

        private double Search(string searchValues, string values)
        {
            if (searchValues.Contains(values))
            {
                return 1.0;
            }
            else
            {
                return 0.0;
            }
        }

        private void AutoGeneratedColumns(object sender, EventArgs e)
        {
            SearchBody.Columns.RemoveAt(0);
            SearchBody.Columns.RemoveAt(0);
            SearchBody.Columns[0].Header = "Container";
            SearchBody.Columns[0].Width = 80;
            SearchBody.Columns[1].Header = "Comments";
            SearchBody.Columns[1].Width = 80;
            SearchBody.Columns[2].Header = "Oven Name";
            SearchBody.Columns[2].Width = 80;
            SearchBody.Columns[3].Header = "Row/Col";
            SearchBody.Columns[3].Width = 60;
            SearchBody.Columns[4].Header = "Recipe Name";
            SearchBody.Columns[4].Width = 90;
            SearchBody.Columns[5].Header = "Ready Time";
            SearchBody.Columns[5].Width = 130;
        }

        private void searchClickHelper()
        {
            string values = SearchValues.Text;
            if (values.Length > 3 && !values.Equals("Search Oven"))
            {
                SearchHelperIterate(values);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchClickHelper();
        }

        private void DisplayHelp()
        {
            if (!helpBar.IsLoaded)
            {
                helpBar = new SearchHelp();
                helpBar.Show();
            }
        }

        private void SearchHelp_Click(object sender, RoutedEventArgs e)
        {
            DisplayHelp();
        }

        private void DataGridCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me.RightButton == MouseButtonState.Pressed)
            {
                DataGridCell temp = sender as DataGridCell;
                DataGridRow dataRow = DataGridRow.GetRowContainingElement(temp);
                int index = dataRow.GetIndex();
                DataGridCellInfo myInfo = new DataGridCellInfo(SearchBody.Items[index], SearchBody.Columns[0]);
                SearchElements tempEle = myInfo.Item as SearchElements;
                char row = tempEle.row;
                int column = tempEle.column;
                string ovenName = tempEle.ovenName;
                ContextProperties newProperties = new ContextProperties(ovenName, row, column);
                newProperties.Show();
            }
        }

        private void Submit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                searchClickHelper();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SearchHelp.isChecked)
            {
                DisplayHelp();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (helpBar.Visibility == Visibility.Visible)
            {
                helpBar.Close();
            }
            base.OnClosing(e);
        }
    }

    public class SearchElements
    {
        //public DataGridHyperlinkColumn hyperColumn { get; set; }
        //public TextBlock containerNums { get; set; }
        //public TextBlock comments { get; set; }
        public char row { get; set; }
        public int column { get; set; }
        public string containerNumsString { get; set; }
        public string commentString { get; set; }
        public string ovenName { get; set; }
        public string locationID { get; set; }
        public string recipeName { get; set; }
        public DateTime readyTime { get; set; }
    }
}