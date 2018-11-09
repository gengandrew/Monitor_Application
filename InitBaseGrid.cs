using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.OleDb;
namespace TimeTracker
{
    class InitBaseGrid
    {
        private Grid retGrid;

        private string ovenName;
        private static int rowSize = 60;  //Height
        private static int colSize = 170;   //Width
        private int height;
        private int width;
        public static int rowCount;
        public static int columnCount;
        //private static int height = 350;
        //private static int width = 600;

        private Label[,] locationLabel;
        private Label[,] lbl;
        private Grid[,] Icolbl;
        private Border[,] panels;
        private Cell[,] myCells;

        public delegate void myEventHandler(bool fromMain);

        public static event myEventHandler ActionTaken;

        public InitBaseGrid(string inOvenName)
        {
            this.ovenName = inOvenName;
            try
            {
                rowCount = Server.getRowSizeFromDB(ovenName);
                columnCount = Server.getColumnSizeFromDB(ovenName);
                this.height = rowCount * rowSize;
                this.width = columnCount * colSize;
            }
            catch (Exception e)
            {
                MessageBox.Show("An Error has Occured, Please Contact System Admin with the Following Information: " + e.Message);
                Environment.Exit(1);
            }
            locationLabel = new Label[rowCount + 1, columnCount + 1];
            Icolbl = new Grid[rowCount + 1, columnCount + 1];
            lbl = new Label[rowCount + 1, columnCount + 1];
            panels = new Border[rowCount + 1, columnCount + 1];
            myCells = new Cell[rowCount + 1, columnCount + 1];

            retGrid = initBaseGrid(new Grid());
            retGrid = initGrid(retGrid);
            retGrid = initPanels(retGrid);

            Server.readAllFromDataBase(ovenName);
            InitFromDataBase(); //Ask someone about this!
        }

        private Grid initBaseGrid(Grid BaseGrid)
        {
            BaseGrid.Background = Brushes.Gray;
            BaseGrid.Name = "BaseGrid";
            BaseGrid.Height = height;
            BaseGrid.Width = width;
            BaseGrid.VerticalAlignment = VerticalAlignment.Top;
            BaseGrid.HorizontalAlignment = HorizontalAlignment.Center;
            return BaseGrid;
        }

        private Grid initPanels(Grid BaseGrid)
        {
            int myRow = -1;
            foreach (RowDefinition row in BaseGrid.RowDefinitions)
            {
                myRow++;
                int myCol = -1;
                foreach (ColumnDefinition col in BaseGrid.ColumnDefinitions)
                {
                    myCol++;
                    panels[myRow, myCol] = new Border();
                    Grid.SetColumn(panels[myRow, myCol], myCol);
                    Grid.SetRow(panels[myRow, myCol], myRow);

                    locationLabel[myRow, myCol] = new Label();
                    locationLabel[myRow, myCol].Content = Conversions.intToChar(myRow).ToString() + (myCol + 1).ToString();
                    locationLabel[myRow, myCol].FontSize = 12;
                    locationLabel[myRow, myCol].FontWeight = FontWeights.DemiBold;
                    locationLabel[myRow, myCol].VerticalAlignment = VerticalAlignment.Top;
                    locationLabel[myRow, myCol].HorizontalAlignment = HorizontalAlignment.Left;

                    lbl[myRow, myCol] = new Label();
                    lbl[myRow, myCol].Content = "";
                    lbl[myRow, myCol].FontSize = 13;
                    lbl[myRow, myCol].HorizontalAlignment = HorizontalAlignment.Center;
                    lbl[myRow, myCol].VerticalAlignment = VerticalAlignment.Center;

                    Icolbl[myRow, myCol] = new Grid();
                    Icolbl[myRow, myCol].Children.Add(lbl[myRow, myCol]);
                    Icolbl[myRow, myCol].Children.Add(locationLabel[myRow, myCol]);
                    Icolbl[myRow, myCol].Background = Brushes.Transparent;
                    panels[myRow, myCol].Child = Icolbl[myRow, myCol];

                    //panels[myRow, myCol].Child = lbl[myRow, myCol];
                    panels[myRow, myCol].Child = Icolbl[myRow, myCol];
                    panels[myRow, myCol].MouseEnter += Panel_MouseEnter;
                    panels[myRow, myCol].MouseLeave += Panel_MouseLeave;
                    panels[myRow, myCol].MouseDown += Panel_MouseDown;
                    panels[myRow, myCol].Margin = new Thickness(1);
                    panels[myRow, myCol].Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                    BaseGrid.Children.Add(panels[myRow, myCol]);

                    ContextMenuHelper myCMH = new ContextMenuHelper(this.ovenName, panels[myRow, myCol], lbl[myRow, myCol], Icolbl[myRow, myCol]);
                    myCMH.ContextMenuClick += ContextMenuClick;
                    myCMH.clearActionTaken += clearActionTaken;
                    panels[myRow, myCol].ContextMenu = myCMH.getContextMenu();
                }
            }
            return BaseGrid;
        }

        private Grid initGrid(Grid BaseGrid)
        {
            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition newRow = new RowDefinition();
                GridLength rowlength = new GridLength(rowSize);
                newRow.Height = rowlength;
                BaseGrid.RowDefinitions.Add(newRow);
                var rowControl = new UserControl();
                BaseGrid.Children.Add(rowControl);
                Grid.SetRow(rowControl, BaseGrid.RowDefinitions.Count);
            }
            for (int j = 0; j < columnCount; j++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                GridLength colLength = new GridLength(colSize);
                newCol.Width = colLength;
                BaseGrid.ColumnDefinitions.Add(newCol);
                var colControl = new UserControl();
                BaseGrid.Children.Add(colControl);
                Grid.SetRow(colControl, BaseGrid.ColumnDefinitions.Count);
            }
            return BaseGrid;
        }

        private void InitFromDataBase()
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (!Server.dataStructure[j, i].Equals(Conversions.nullity))
                    {
                        char Row = Conversions.intToChar(i);
                        string stringProgressionID = Server.getCurrProgressionID(ovenName, Row, j);
                        int[] progressionID = Conversions.progressionIDToArray(stringProgressionID);
                        int index = Conversions.getLastProgressionID(progressionID);
                        int currProgID = progressionID[index];
                        myCells[i, j] = new Cell(ovenName,
                            Conversions.DateTimeToTimeSpan(Server.getLimits(currProgID)), progressionID,
                            Server.getRecipeKey(ovenName, Row, j), panels[i, j], lbl[i, j], Icolbl[i, j]);
                        myCells[i, j].timerLevel1 += cellTimerLevel1;
                        myCells[i, j].timerLevel2 += cellTimerLevel2;
                        myCells[i, j].timerLevel3 += cellTimerLevel3;
                        myCells[i, j].timerLevelReset += cellTimerLevelReset;
                        myCells[i, j].Click(true);
                    }
                }
            }
        }

        public void filterGrid(string recipeName, int progressionID)
        {
            if (recipeName == null && progressionID == -1)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        Icolbl[i, j].Background = Brushes.Transparent;
                        locationLabel[i, j].Foreground = Brushes.Black;
                    }
                }
            }
            else
            {
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (myCells[i, j] != null)
                        {
                            int recipeKey = myCells[i, j].getRecipeKey();
                            string tempRecipeName = Server.getRecipeName(recipeKey);
                            int[] placeHolder = myCells[i, j].getProgressionID();
                            int index = Conversions.getLastProgressionID(placeHolder);
                            int temp = placeHolder[index];
                            if (recipeName.Equals(tempRecipeName) && progressionID == -1)
                            {
                                Icolbl[i, j].Background = Brushes.Transparent;
                                locationLabel[i, j].Foreground = Brushes.Black;
                            }
                            else if (recipeName.Equals(tempRecipeName) && progressionID == temp)
                            {
                                Icolbl[i, j].Background = Brushes.Transparent;
                                locationLabel[i, j].Foreground = Brushes.Black;
                            }
                            else
                            {
                                Icolbl[i, j].Background = Brushes.Black;
                                locationLabel[i, j].Foreground = Brushes.Gray;
                            }
                        }
                        else
                        {
                            Icolbl[i, j].Background = Brushes.Black;
                            locationLabel[i, j].Foreground = Brushes.Gray;
                        }
                    }
                }
            }
            myEventHandler handler = ActionTaken;
            if (handler != null)
            {
                ActionTaken(false);
            }
        }

        private void CellProgression(Border inputBorder, Label inputlbl, int[] inProgressionID,
            int index, char inRow, int inColumn, int RecipeKey)
        {
            int row = Conversions.charToInt(inRow);
            if (!Server.getRecipesIsOptional(inProgressionID[index]))
            {
                ProgressionHelperO1 progressionHelper1 = new ProgressionHelperO1();
                progressionHelper1.ShowDialog();
                myCells[row, inColumn] = null;
                TimeSpan[] Limits = Conversions.DateTimeToTimeSpan(Server.getLimits(inProgressionID[index]));
                myCells[row, inColumn] = new Cell(this.ovenName, Limits, inProgressionID, RecipeKey, inputBorder, inputlbl, Icolbl[row, inColumn]);
                myCells[row, inColumn].timerLevel1 += cellTimerLevel1;
                myCells[row, inColumn].timerLevel2 += cellTimerLevel2;
                myCells[row, inColumn].timerLevel3 += cellTimerLevel3;
                myCells[row, inColumn].timerLevelReset += cellTimerLevelReset;
                myCells[row, inColumn].Click(false);
            }
            else
            {
                ProgressionHelperO2 progressionWindow = new ProgressionHelperO2();
                progressionWindow.ShowDialog();
                if (progressionWindow.getIsSubmit())
                {
                    myCells[row, inColumn] = null;
                    TimeSpan[] Limits = Conversions.DateTimeToTimeSpan(Server.getLimits(inProgressionID[index]));
                    myCells[row, inColumn] = new Cell(this.ovenName, Limits, inProgressionID, RecipeKey, inputBorder, inputlbl, Icolbl[row, inColumn]);
                    myCells[row, inColumn].timerLevel1 += cellTimerLevel1;
                    myCells[row, inColumn].timerLevel2 += cellTimerLevel2;
                    myCells[row, inColumn].timerLevel3 += cellTimerLevel3;
                    myCells[row, inColumn].timerLevelReset += cellTimerLevelReset;
                    myCells[row, inColumn].Click(false);
                }
                else
                {
                    Server.clearOvenComments(ovenName, inRow, inColumn);
                    myCells[Conversions.charToInt(inRow), inColumn] = null;
                    inputBorder.Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                    inputlbl.Content = "";
                    if (Icolbl[row, inColumn].Children.Count == 3)
                    {
                        Icolbl[row, inColumn].Children.RemoveAt(2);
                    }
                    /*
                    inProgressionID[index] = 0;
                    bool hasProg = false;
                    int index2 = 0;
                    for (int i = 0; i < inProgressionID.Length; i++)
                    {
                        if (inProgressionID[i] != 0)
                        {
                            hasProg = true;
                            index2 = i;
                            break;
                        }
                    }
                    if (hasProg)
                    {
                        System.Threading.Thread.Sleep(800);
                        CellProgression(inputBorder, inputlbl, inProgressionID, index2, inRow, inColumn, inContainerNums, RecipeKey);
                    }
                    else
                    {
                        Server.clearOvenComments(ovenName, inRow, inColumn);
                        myCells[Conversions.charToInt(inRow), inColumn] = null;
                        inputBorder.Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                        inputlbl.Content = "";
                        if (Icolbl[row, inColumn].Children.Count == 3)
                        {
                            Icolbl[row, inColumn].Children.RemoveAt(2);
                        }
                    }
                    */
                }
            }
        }

        private void Panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border panel = sender as Border;
            MouseEventArgs me = e as MouseEventArgs;
            if (me.LeftButton == MouseButtonState.Pressed)
            {
                char UniqueChar = 'A';
                UniqueChar = (char)((int)UniqueChar + System.Windows.Controls.Grid.GetRow(panel));
                int UniqueInt = System.Windows.Controls.Grid.GetColumn(panel);
                if (myCells[Grid.GetRow(panel), Grid.GetColumn(panel)] != null)
                {
                    myCells[Grid.GetRow(panel), Grid.GetColumn(panel)].Click(false);
                }
                else
                {
                    ContainerHelper myContainer = new ContainerHelper(UniqueChar, UniqueInt);
                    myContainer.ShowDialog();
                    if (myContainer.getIsSubmit())
                    {
                        Server.UpdateComments(ovenName, UniqueChar, UniqueInt, myContainer.getComments());
                        Server.UpdateContainerNums(ovenName, UniqueChar, UniqueInt, myContainer.getContainerNums());
                        int[] ProgressionID = myContainer.getProgressionID();
                        int RecipeKey = myContainer.getRecipeKey();
                        TimeSpan[] Limits = Conversions.DateTimeToTimeSpan(Server.getLimits(ProgressionID[0]));
                        myCells[Grid.GetRow(panel), Grid.GetColumn(panel)] = new Cell(this.ovenName, Limits, ProgressionID, RecipeKey, panel, lbl[Grid.GetRow(panel), Grid.GetColumn(panel)], Icolbl[Grid.GetRow(panel), Grid.GetColumn(panel)]);
                        myCells[Grid.GetRow(panel), Grid.GetColumn(panel)].timerLevel1 += cellTimerLevel1;
                        myCells[Grid.GetRow(panel), Grid.GetColumn(panel)].timerLevel2 += cellTimerLevel2;
                        myCells[Grid.GetRow(panel), Grid.GetColumn(panel)].timerLevel3 += cellTimerLevel3;
                        myCells[Grid.GetRow(panel), Grid.GetColumn(panel)].timerLevelReset += cellTimerLevelReset;
                        myCells[Grid.GetRow(panel), Grid.GetColumn(panel)].Click(false);
                    }
                }
                myEventHandler handler = ActionTaken;
                if (handler != null)
                {
                    ActionTaken(false);
                }
            }
        }

        private void Panel_MouseLeave(object sender, MouseEventArgs e)
        {
            Border panel = sender as Border;
            panel.BorderThickness = new Thickness(0);
        }

        private void Panel_MouseEnter(object sender, MouseEventArgs e)
        {
            Border panel = sender as Border;
            panel.BorderThickness = new Thickness(1);
            panel.BorderBrush = new SolidColorBrush(Color.FromArgb(200, 20, 20, 20));
        }

        private void ContextMenuClick(Cell input)
        {
            input.timerLevel1 += cellTimerLevel1;
            input.timerLevel2 += cellTimerLevel2;
            input.timerLevel3 += cellTimerLevel3;
            input.timerLevelReset += cellTimerLevelReset;
            input.Click(false);
        }

        private void clearActionTaken(string inOvenName, Border inputBorder, Label inputLabel)
        {
            char row = Conversions.intToChar(Grid.GetRow(inputBorder));
            int column = Grid.GetColumn(inputBorder);
            Server.clearOvenComments(ovenName, row, column);
            myCells[Conversions.charToInt(row), column].stopCellTimer();
            myCells[Conversions.charToInt(row), column].clearDataBaseEntires();
            myCells[Conversions.charToInt(row), column] = null;
            inputBorder.Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
            inputLabel.Content = "";
            if (Icolbl[Conversions.charToInt(row), column].Children.Count == 3)
            {
                Icolbl[Conversions.charToInt(row), column].Children.RemoveAt(2);
            }

            myEventHandler handler = ActionTaken;
            if (handler != null)
            {
                ActionTaken(false);
            }
        }

        private void cellTimerLevelReset(Border inputBorder, Label inputlbl, int[] inProgressionID, char inRow, int inColumn, int RecipeKey)
        {
            int row = Conversions.charToInt(inRow);
            bool hasProg = false;
            int index = 0;
            for (int i = 0; i < inProgressionID.Length; i++)
            {
                if (inProgressionID[i] != 0)
                {
                    hasProg = true;
                    index = i;
                    break;
                }
            }
            if (hasProg)
            {
                CellProgression(inputBorder, inputlbl, inProgressionID, index, inRow, inColumn, RecipeKey);
            }
            else
            {
                Server.clearOvenComments(ovenName, inRow, inColumn);
                myCells[Conversions.charToInt(inRow), inColumn] = null;
                inputBorder.Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                inputlbl.Content = "";
                if (Icolbl[row, inColumn].Children.Count == 3)
                {
                    Icolbl[row, inColumn].Children.RemoveAt(2);
                }
            }
        }

        private void cellTimerLevel3(Border inputBorder, Label inputLabel, int[] inProgressionID, char inRow, int inColumn, int RecipeKey)
        {
            int row = Conversions.charToInt(inRow);
            if (Icolbl[row, inColumn].Children.Count == 3)
            {
                Icolbl[row, inColumn].Children.RemoveAt(2);
                Icons myIcons = new Icons();
                Icolbl[row, inColumn].Children.Add(myIcons.getRed());
            }
            inputBorder.Background = Brushes.Red;
        }

        private void cellTimerLevel2(Border inputBorder, Label inputlbl, int[] inProgressionID, char inRow, int inColumn, int RecipeKey)
        {
            int row = Conversions.charToInt(inRow);
            if (Icolbl[row, inColumn].Children.Count == 3)
            {
                Icolbl[row, inColumn].Children.RemoveAt(2);
                Icons myIcons = new Icons();
                Icolbl[row, inColumn].Children.Add(myIcons.getYellow());
            }
            inputBorder.Background = Brushes.Yellow;
        }

        private void cellTimerLevel1(Border inputBorder, Label inputlbl, int[] inProgressionID, char inRow, int inColumn, int RecipeKey)
        {
            int row = Conversions.charToInt(inRow);
            if (Icolbl[row, inColumn].Children.Count == 3)
            {
                Icolbl[row, inColumn].Children.RemoveAt(2);
                Icons myIcons = new Icons();
                Icolbl[row, inColumn].Children.Add(myIcons.getGreen());
            }
            inputBorder.Background = Brushes.Green;
        }

        public void forceTimerStop()
        {
            for (int i = 0; i < myCells.GetLength(0); i++)
            {
                for (int j = 0; j < myCells.GetLength(1); j++)
                {
                    if (myCells[i, j] != null)
                    {
                        myCells[i, j].stopCellTimer();
                    }
                }
            }
        }

        public Grid getGrid()
        {
            return this.retGrid;
        }

        public int getHeight()
        {
            return this.height;
        }

        public int getWidth()
        {
            return this.width;
        }

        public string getOvenName()
        {
            return this.ovenName;
        }

        public Cell[,] getCells()
        {
            return this.myCells;
        }

        public Grid[,] getMaskingGrid()
        {
            return this.Icolbl;
        }
    }
}
