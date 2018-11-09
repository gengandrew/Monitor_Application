using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Threading;
using System.Configuration;
using System.Data.OleDb;
using System.Windows.Controls;
namespace TimeTracker
{
    public class Cell
    {
        private TimeSpan totalTimer;
        private TimeSpan spanTimer;
        private TimeSpan secSpan = new TimeSpan(0, 0, 0, 1, 0);
        private TimeSpan[] limits;
        private int Limit1;
        private int Limit2;
        private int Limit3;
        private int timer;

        private DispatcherTimer disTimer;
        private Border currBorder;
        private Label currlbl;
        private Grid currGrid;

        private char row;
        private int column;
        private string ovenName;
        private int[] ProgressionID;
        private int RecipeKey;
        private bool[] singleCounter;

        public delegate void myEventHandler(Border inputBorder, Label inputLabel,
            int[] inProgressionID, char inRow, int inColumn, int RecipeKey);

        public event myEventHandler timerLevel1;

        public event myEventHandler timerLevel2;

        public event myEventHandler timerLevel3;

        public event myEventHandler timerLevelReset;

        public Cell(string inOvenName, TimeSpan[] inLimits,
            int[] inProgressionID, int inRecipeKey, Border inputBorder, Label inputLabel, Grid inputGrid)
        {
            char UniqueChar = 'A';
            this.row = (char)((int)UniqueChar + Grid.GetRow(inputBorder));
            this.column = Grid.GetColumn(inputBorder);
            this.currBorder = inputBorder;
            this.currGrid = inputGrid;
            this.currlbl = inputLabel;
            singleCounter = new bool[2] { false, false };
            this.limits = inLimits;
            this.totalTimer = inLimits[2].Add(inLimits[1]).Add(inLimits[0]);
            this.spanTimer = inLimits[0];
            this.Limit1 = (int)inLimits[0].TotalSeconds;
            this.Limit2 = (int)inLimits[0].TotalSeconds + (int)inLimits[1].TotalSeconds;
            this.Limit3 = (int)inLimits[0].TotalSeconds + (int)inLimits[1].TotalSeconds + (int)inLimits[2].TotalSeconds;
            this.timer = 0;
            this.ovenName = inOvenName;
            this.ProgressionID = inProgressionID;
            this.RecipeKey = inRecipeKey;
        }

        public void Click(bool autoFromRead)
        {
            DateTime startTime = Server.getStartTime(ovenName, row, column);
            if (startTime.Equals(Server.nullity))
            {
                DateTime curr = new DateTime();
                curr = DateTime.Now;
                int placeHolder = Server.UpdateOven(this.ovenName, this.row, this.column, curr, RecipeKey, this.ProgressionID);
                if (placeHolder != -1)
                {
                    disTimer = new DispatcherTimer();
                    disTimer.Interval = secSpan;
                    disTimer.Tick += DisTimer_Tick;
                    currBorder.Background = System.Windows.Media.Brushes.Blue;
                    if (currGrid.Children.Count == 2)
                    {
                        Icons myIcons = new Icons();
                        currGrid.Children.Add(myIcons.getBlue());
                    }
                    else if (currGrid.Children.Count == 3)
                    {
                        currGrid.Children.RemoveAt(2);
                        Icons myIcons = new Icons();
                        currGrid.Children.Add(myIcons.getBlue());
                    }
                    currlbl.Content = Conversions.StripMiliSec(spanTimer).ToString();
                    //currlbl.Content = string.Format("{0}:{1}:{2}:{3}", (int)spanTimer.Days, (int)spanTimer.Hours, (int)spanTimer.Minutes, (int)spanTimer.Seconds);
                    disTimer.Start();
                }
                else
                {
                    MainWindow.connect.Close();
                    System.Windows.MessageBox.Show("Database Entry has Failed, Please Contact System Admin.");
                }
                Server.readAllFromDataBase(ovenName);
            }
            else
            {
                DateTime curr = DateTime.Now;

                //The entire calculation below is fucked but it works
                TimeSpan tempSpans1 = totalTimer;
                TimeSpan tempSpans2 = curr.Subtract(startTime);
                TimeSpan remSpan = tempSpans1.Subtract(tempSpans2);
                double sec = tempSpans2.TotalSeconds;

                TimeSpan currStime = curr.Subtract(startTime);
                TimeSpan timeTest = totalTimer.Subtract(currStime);
                double totSec = timeTest.TotalSeconds;
                if (totSec <= -0.5) //-500 mili seconds as a buffer for timer threading issues
                {
                    if (autoFromRead)
                    {
                        currBorder.Background = System.Windows.Media.Brushes.Red;
                        currlbl.Content = currlbl.Content = "00:00:00:00";
                        if (currGrid.Children.Count == 2)
                        {
                            Icons myIcons = new Icons();
                            currGrid.Children.Add(myIcons.getRed());
                        }
                    }
                    else
                    {
                        this.ProgressionID = Conversions.UpdateProgressionID(this.ProgressionID);
                        int placeHolder = Server.UpdateOven(this.ovenName, this.row, this.column, Server.nullity, RecipeKey, this.ProgressionID);
                        if (placeHolder != -1)
                        {
                            timerLevelReset(this.currBorder, this.currlbl, ProgressionID, this.row, this.column, this.RecipeKey);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Database nullified has Failed, Please Contact System Admin.");
                        }
                    }
                }
                else
                {
                    if (autoFromRead) //need to fix this
                    {
                        disTimer = new DispatcherTimer();
                        disTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);    //current timer increments in 1 seconds
                        disTimer.Tick += DisTimer_Tick;
                        this.timer = (int)sec;
                        spanTimer = remSpan;
                        if (timer >= Limit2 && timer < Limit3) //Instead of equals to try using less than in order to incorporate all the possible time frames.
                        {
                            currBorder.Background = System.Windows.Media.Brushes.Yellow;
                            if (currGrid.Children.Count == 2)
                            {
                                Icons myIcons = new Icons();
                                currGrid.Children.Add(myIcons.getYellow());
                            }
                        }
                        else if (timer >= Limit1 && timer < Limit2)
                        {
                            currBorder.Background = System.Windows.Media.Brushes.Green;
                            if (currGrid.Children.Count == 2)
                            {
                                Icons myIcons = new Icons();
                                currGrid.Children.Add(myIcons.getGreen());
                            }
                        }
                        else
                        {
                            currBorder.Background = System.Windows.Media.Brushes.Blue;
                            if (currGrid.Children.Count == 2)
                            {
                                Icons myIcons = new Icons();
                                currGrid.Children.Add(myIcons.getBlue());
                            }
                        }
                        currlbl.Content = Conversions.StripMiliSec(spanTimer).ToString();
                        disTimer.Start();
                    }
                    else if (singleCounter[0])
                    {
                        disTimer.Stop();
                        if (currGrid.Children.Count == 3)
                        {
                            currGrid.Children.RemoveAt(2);
                        }
                        this.ProgressionID = Conversions.UpdateProgressionID(this.ProgressionID);
                        int placeHolder = Server.UpdateOven(this.ovenName, this.row, this.column, Server.nullity, RecipeKey, this.ProgressionID);
                        if (placeHolder != -1)
                        {
                            timerLevelReset(this.currBorder, this.currlbl, ProgressionID, this.row, this.column, this.RecipeKey);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Database nullified has Failed, Please Contact System Admin.");
                        }
                    }
                }
            }
            Server.readAllFromDataBase(ovenName);
        }

        private void DisTimer_Tick(object sender, EventArgs e)
        {
            timer++;
            spanTimer = spanTimer.Subtract(secSpan);
            if (timer <= Limit3)
            {
                if (timer >= Limit2 && timer < Limit3) //Instead of equals to try using less than in order to incorporate all the possible time frames.
                {
                    if (!singleCounter[1])
                    {
                        spanTimer = limits[2];
                        singleCounter[1] = true;
                    }
                    timerLevel2(this.currBorder, this.currlbl, ProgressionID, this.row, this.column, this.RecipeKey);
                }
                else if (timer >= Limit1 && timer < Limit2)
                {
                    if (!singleCounter[0])
                    {
                        spanTimer = limits[1];
                        singleCounter[0] = true;
                    }
                    timerLevel1(this.currBorder, this.currlbl, ProgressionID, this.row, this.column, this.RecipeKey);
                }
                currlbl.Content = Conversions.StripMiliSec(spanTimer).ToString();
                //currlbl.Content = currlbl.Content = string.Format("{0}:{1}:{2}:{3}", (int)spanTimer.Days, (int)spanTimer.Hours, (int)spanTimer.Minutes, (int)spanTimer.Seconds);
            }
            else
            {
                timerLevel3(this.currBorder, this.currlbl, ProgressionID, this.row, this.column, this.RecipeKey);
                disTimer.Stop();
            }
        }

        public void clearDataBaseEntires()
        {
            int placeHolder = Server.UpdateOven(this.ovenName, this.row, this.column, Server.nullity, 1, new int[1] { 0 });
            if (placeHolder == -1)
            {
                System.Windows.MessageBox.Show("Database nullified has Failed, Please Contact System Admin.");
            }
        }

        public void stopCellTimer()
        {
            if (disTimer != null)
            {
                disTimer.Stop();
            }
        }

        public int getSecTimerLimit()
        {
            return this.Limit3 - this.timer;
        }

        public bool[] getSingleCounter()
        {
            return new bool[2] { this.singleCounter[0], this.singleCounter[1] };
        }

        public TimeSpan[] getLimits()
        {
            return limits;
        }

        public TimeSpan getTotalTimer()
        {
            return totalTimer;
        }

        public TimeSpan getTimeSpan()
        {
            return spanTimer;
        }

        public char getRow()
        {
            return this.row;
        }

        public int getColumn()
        {
            return this.column;
        }

        public string getOvenName()
        {
            return this.ovenName;
        }

        public int getRecipeKey()
        {
            return this.RecipeKey;
        }

        public int[] getProgressionID()
        {
            return this.ProgressionID;
        }
    }
}
