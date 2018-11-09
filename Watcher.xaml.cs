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
using System.Diagnostics;
using System.Data.OleDb;
using System.ComponentModel;
using System.Timers;
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for Watcher.xaml
    /// </summary>
    public partial class Watcher : Window
    {
        public static ArrayList elements = new ArrayList();
        private Timer refreshTimer;
        internal Help helpBar;

        public delegate void myEventHandler(bool fromMain);
        public static event myEventHandler ActionTaken;

        public Watcher()
        {
            refreshTimer = new Timer();
            helpBar = new Help();
            InitializeComponent();
            InitAdminPrivileges();
            FillDisplayOven();
            Block_UserName.Text = MainWindow.username;
            Block_GroupID.Text = MainWindow.groupID;
            refreshTimer.Interval = 10000; // 10 sec refresh rate maybe change to 5 seconds
            refreshTimer.Elapsed += ScreenRefresh;
            refreshTimer.Start();
        }

        private void removeInstance(string inOvenName)
        {
            InitInstance.prevOvens.Remove(inOvenName);
            for (int i = 0; i < elements.Count; i++)
            {
                InitBaseGrid placeHolder = elements[i] as InitBaseGrid;
                if (placeHolder.getOvenName().Equals(inOvenName))
                {
                    placeHolder.forceTimerStop();
                    WraperContent.Children.RemoveAt(i);
                    elements.RemoveAt(i);
                }
            }
            myEventHandler handler = ActionTaken;
            if (handler != null)
            {
                ActionTaken(true);
            }
        }

        private void addInstance(string ovenName)
        {
            InitBaseGrid baseGridClass = new InitBaseGrid(ovenName);
            int baseHeight = baseGridClass.getHeight();
            int baseWidth = baseGridClass.getWidth();
            if (baseWidth > WraperContent.MaxWidth)
            {
                WraperContent.MaxWidth = baseWidth + 50;
            }
            InitMainBorder mainBorderClass = new InitMainBorder(ovenName, baseHeight, baseWidth);
            InitMainGrid mainGridClass = new InitMainGrid(ovenName, baseHeight, baseWidth);
            mainGridClass.ChangeSizeClick += ChangeSizeClick;
            mainGridClass.DeleteClick += DeleteClick;
            mainGridClass.HelpClick += DisplayHelp;

            Border mainBorder = mainBorderClass.getMainBorder();
            Grid mainGrid = mainGridClass.getMainGrid();
            Grid baseGrid = baseGridClass.getGrid();
            Thickness margin = baseGrid.Margin;
            margin.Top = 25;
            margin.Left = 0;
            margin.Right = 0;
            margin.Bottom = 0;
            baseGrid.Margin = margin;
            elements.Add(baseGridClass);
            mainGrid.Children.Add(baseGrid);
            mainBorder.Child = mainGrid;
            WraperContent.Children.Add(mainBorder);

            myEventHandler handler = ActionTaken;
            if (handler != null)
            {
                ActionTaken(false);
            }
        }

        private void AddInstanceTab()
        {
            InitInstance tabChooser = new InitInstance();
            tabChooser.ShowDialog();
            if (tabChooser.getIsSubmit())
            {
                string ovenName = tabChooser.getOvenName();
                addInstance(ovenName);
            }
        }

        private void ChangeSizeClick(string inOvenName)
        {
            OvenSpecifics ovenSpec = new OvenSpecifics(inOvenName);
            ovenSpec.ShowDialog();
            if (ovenSpec.getSizeChange())
            {
                removeInstance(inOvenName);

                InitBaseGrid baseGridClass = new InitBaseGrid(inOvenName);
                int baseHeight = baseGridClass.getHeight();
                int baseWidth = baseGridClass.getWidth();
                if (baseWidth <= WraperContent.MaxWidth)
                {
                    WraperContent.MaxWidth = SystemParameters.PrimaryScreenWidth;
                }
                if (baseWidth > WraperContent.MaxWidth)
                {
                    WraperContent.MaxWidth = baseWidth + 50;
                }
                InitMainBorder mainBorderClass = new InitMainBorder(inOvenName, baseHeight, baseWidth);
                InitMainGrid mainGridClass = new InitMainGrid(inOvenName, baseHeight, baseWidth);
                mainGridClass.ChangeSizeClick += ChangeSizeClick;
                mainGridClass.DeleteClick += DeleteClick;
                mainGridClass.HelpClick += DisplayHelp;

                Border mainBorder = mainBorderClass.getMainBorder();
                Grid mainGrid = mainGridClass.getMainGrid();
                Grid baseGrid = baseGridClass.getGrid();
                Thickness margin = baseGrid.Margin;
                margin.Top = 25;
                margin.Left = 0;
                margin.Right = 0;
                margin.Bottom = 0;
                baseGrid.Margin = margin;

                mainGrid.Children.Add(baseGrid);
                mainBorder.Child = mainGrid;
                elements.Add(baseGridClass);
                WraperContent.Children.Add(mainBorder);
            }
        }

        private void InitAdminPrivileges()
        {
            if (MainWindow.isAdmin)
            {
                AddAdminDomain.Visibility = Visibility.Visible;
                AddRecipe.Visibility = Visibility.Visible;
                DeleteRecipe.Visibility = Visibility.Visible;
                AddOven.Visibility = Visibility.Visible;
                DeleteOven.Visibility = Visibility.Visible;
            }
        }

        private WrapPanel InitDisplayRecipes()
        {
            TextBlock RecipeOptionText = new TextBlock();
            RecipeOptionText.Text = "Select Recipe: ";
            RecipeOptionText.VerticalAlignment = VerticalAlignment.Center;
            RecipeOptionText.SetValue(FontWeightProperty, FontWeights.DemiBold);
            Thickness recMargin = RecipeOptionText.Margin;
            recMargin.Right = 5;
            RecipeOptionText.Margin = recMargin;
            ComboBox DisplayRecipe = new ComboBox();
            DisplayRecipe.Name = "DisplayRecipe";
            DisplayRecipe.Width = 90;
            DisplayRecipe.VerticalAlignment = VerticalAlignment.Center;
            Thickness cRecMargin = DisplayRecipe.Margin;
            cRecMargin.Right = 5;
            DisplayRecipe.Margin = cRecMargin;
            DisplayRecipe = FillDisplayRecipe(DisplayRecipe);
            DisplayRecipe.SelectionChanged += DisplayRecipeChanged;
            WrapPanel DisplayOptions = new WrapPanel();
            DisplayOptions.Children.Add(RecipeOptionText);
            DisplayOptions.Children.Add(DisplayRecipe);
            Thickness margin = DisplayOptions.Margin;
            margin.Right = 5;
            DisplayOptions.Margin = margin;
            return DisplayOptions;
        }

        private WrapPanel InitDisplayProgression(string RecipeName)
        {
            TextBlock ProgressionOptionText = new TextBlock();
            ProgressionOptionText.Text = "Select Progression: ";
            ProgressionOptionText.VerticalAlignment = VerticalAlignment.Center;
            ProgressionOptionText.SetValue(FontWeightProperty, FontWeights.DemiBold);
            Thickness proMargin = ProgressionOptionText.Margin;
            proMargin.Right = 5;
            ProgressionOptionText.Margin = proMargin;

            ComboBox DisplayProgression = new ComboBox();
            DisplayProgression.Name = "DisplayProgression";
            DisplayProgression.Width = 90;
            DisplayProgression.VerticalAlignment = VerticalAlignment.Center;
            //DisplayProgression = FillDisplayProgression(DisplayProgression);

            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM RecipePointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            string ProgressionID = "";
            while (scnr.Read())
            {
                string temp = scnr["Name"].ToString();
                if (temp.Equals(RecipeName))
                {
                    ProgressionID = scnr["ProgressionID"].ToString();
                }
            }
            MainWindow.connect.Close();
            int[] progressionIDArray = Conversions.progressionIDToArray(ProgressionID);
            DisplayProgression.Items.Add("All");
            for (int i = 0; i < progressionIDArray.Length; i++)
            {
                DisplayProgression.Items.Add(progressionIDArray[i].ToString());
            }
            DisplayProgression.SelectionChanged += DisplayProgressionChanged;
            DisplayProgression.SelectedIndex = 0;

            WrapPanel DisplayOptions = new WrapPanel();
            DisplayOptions.Children.Add(ProgressionOptionText);
            DisplayOptions.Children.Add(DisplayProgression);
            Thickness margin = DisplayOptions.Margin;
            margin.Right = 5;
            DisplayOptions.Margin = margin;
            return DisplayOptions;
        }

        private void FillDisplayOven()
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
                DisplayOven.Items.Add(temp);
            }
            MainWindow.connect.Close();
        }

        private ComboBox FillDisplayRecipe(ComboBox DisplayRecipe)
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
                DisplayRecipe.Items.Add(temp);
            }
            MainWindow.connect.Close();
            return DisplayRecipe;
        }

        private void Logout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border panel = sender as Border;
            MouseEventArgs me = e as MouseEventArgs;
            if (me.LeftButton == MouseButtonState.Pressed)
            {
                this.Close();
            }
        }

        private void Logout_MouseLeave(object sender, MouseEventArgs e)
        {
            Border panel = sender as Border;
            panel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E00000"));
            panel.BorderThickness = new Thickness(0);
            //panel.Background = Brushes.Red;
        }

        private void Logout_MouseEnter(object sender, MouseEventArgs e)
        {
            Border panel = sender as Border;
            panel.Background = Brushes.OrangeRed;
            panel.BorderThickness = new Thickness(1);
            panel.BorderBrush = Brushes.Black;
        }

        private void DisplayOvenChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                string ovenName = (sender as ComboBox).SelectedItem as string;
                string prevOvenName = DisplayOven.Text;
                if (elements.Count == 1)
                {
                    removeInstance(prevOvenName);
                }
                addInstance(ovenName);
                if (DisplayOptionsWrap.Children.Count > 3)
                {
                    DisplayOvenOptionsMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.UnfoldMoreVertical;
                    while (DisplayOptionsWrap.Children.Count > 3)
                    {
                        DisplayOptionsWrap.Children.RemoveAt(3);
                    }
                    InitBaseGrid baseGrid = elements[0] as InitBaseGrid;
                    baseGrid.filterGrid(null, -1);
                }
            }
        }

        private void DisplayProgressionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitBaseGrid baseGrid = elements[0] as InitBaseGrid;
            string prevProgressionID = (sender as ComboBox).Text;
            WrapPanel placeHolderWrapPanel = DisplayOptionsWrap.Children[3] as WrapPanel;
            ComboBox DisplayRecipe = placeHolderWrapPanel.Children[1] as ComboBox;
            string recipeName = DisplayRecipe.SelectedItem as string;
            if (prevProgressionID != null)
            {
                baseGrid.filterGrid(recipeName, -1);
            }
            string progressionID = (sender as ComboBox).SelectedItem as string;
            if (!progressionID.Equals("All"))
            {
                baseGrid.filterGrid(recipeName, Int32.Parse(progressionID));
            }
        }

        private void DisplayRecipeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DisplayOptionsWrap.Children.Count > 4)
            {
                DisplayOptionsWrap.Children.RemoveAt(4);
            }
            string RecipeName = (sender as ComboBox).SelectedItem as string;
            WrapPanel addGrid = InitDisplayProgression(RecipeName);
            DisplayOptionsWrap.Children.Add(addGrid);
            InitBaseGrid baseGrid = elements[0] as InitBaseGrid;
            baseGrid.filterGrid(RecipeName, -1);
        }

        private void DisplayOvenOptions_Click(object sender, RoutedEventArgs e)
        {
            string ovenName = DisplayOven.Text;
            if (ovenName.Equals(""))
            {
                MessageBox.Show("No Oven Specified, Please Select the Appropriate Oven.");
            }
            else
            {
                if (DisplayOptionsWrap.Children.Count > 3)
                {
                    DisplayOvenOptionsMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.UnfoldMoreVertical;
                    while (DisplayOptionsWrap.Children.Count > 3)
                    {
                        DisplayOptionsWrap.Children.RemoveAt(3);
                    }
                    InitBaseGrid baseGrid = elements[0] as InitBaseGrid;
                    baseGrid.filterGrid(null, -1);
                }
                else
                {
                    DisplayOvenOptionsMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.UnfoldLessVertical;
                    WrapPanel addGrid = InitDisplayRecipes();
                    DisplayOptionsWrap.Children.Add(addGrid);
                }
            }
        }

        private void ScreenRefresh(object sender, ElapsedEventArgs e)
        {
            if (MainWindow.connect.State.Equals(System.Data.ConnectionState.Closed) && elements.Count > 0)
            {
                this.Dispatcher.Invoke(() =>
                {
                    InitBaseGrid baseGrid = elements[0] as InitBaseGrid;
                    string ovenName = baseGrid.getOvenName();
                    if (InitBaseGrid.columnCount != Server.getColumnSizeFromDB(ovenName) ||
                        InitBaseGrid.rowCount != Server.getRowSizeFromDB(ovenName))
                    {
                        removeInstance(ovenName);
                        addInstance(ovenName);
                    }
                });
            }
        }

        private void DisplayHelp(string inOvenName)
        {
            if (!helpBar.IsLoaded)
            {
                helpBar = new Help();
                helpBar.Show();
            }
        }

        private void DeleteClick(string inOvenName)
        {
            removeInstance(inOvenName);
            DisplayOven.SelectedIndex = -1;
        }

        private void AddOven_Click(object sender, RoutedEventArgs e)
        {
            AddOven myOven = new AddOven();
            myOven.ShowDialog();
            while (DisplayOven.Items.Count != 0)
            {
                DisplayOven.Items.RemoveAt(0);
            }
            FillDisplayOven();
        }

        private void AddAdminDomain_Click(object sender, RoutedEventArgs e)
        {
            AddAdminDomain addDomain = new AddAdminDomain();
            addDomain.ShowDialog();
        }

        private void AddInstance_Click(object sender, RoutedEventArgs e)
        {
            AddInstanceTab();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe myRecipe = new AddRecipe();
            myRecipe.ShowDialog();
        }

        private void DeleteOven_Click(object sender, RoutedEventArgs e)
        {
            DeleteOven delOven = new DeleteOven();
            delOven.ShowDialog();
            while (DisplayOven.Items.Count != 0)
            {
                DisplayOven.Items.RemoveAt(0);
            }
            FillDisplayOven();
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            DeleteRecipe delRecipe = new DeleteRecipe();
            delRecipe.ShowDialog();
        }

        private void ShowList_Click(object sender, RoutedEventArgs e)
        {
            TimerList timerlist = new TimerList();
            timerlist.Show();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchValues = SearchBar.Text;
            SearchEngineView searchEngine = new SearchEngineView(searchValues);
            searchEngine.Show();
        }

        private void Submit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string searchValues = SearchBar.Text;
                SearchEngineView searchEngine = new SearchEngineView(searchValues);
                searchEngine.Show();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Help.isChecked)
            {
                DisplayHelp("");
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            refreshTimer.Stop();
            bool closedFromCode = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() ==
                    typeof(Window).GetMethod("Close")) != null;
            if (closedFromCode)
            {
                if (helpBar.Visibility == Visibility.Visible)
                {
                    helpBar.Close();
                }
                base.OnClosing(e);
            }
            else
            {
                Environment.Exit(1);
            }
        }
    }
}
