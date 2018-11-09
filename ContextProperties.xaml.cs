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

using System.Windows.Threading;
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for ContextProperties.xaml
    /// </summary>
    public partial class ContextProperties : Window
    {
        private char row;
        private int column;
        private string ovenName;
        private DispatcherTimer disTimer;
        private int timerLimit;
        private TimeSpan tempRem;
        private TimeSpan secTick;

        public ContextProperties(string inOvenName, char inRow, int inColumn)
        {
            this.row = inRow;
            this.ovenName = inOvenName;
            this.column = inColumn;
            this.secTick = new TimeSpan(0, 0, 0, 1, 0);
            InitializeComponent();
            InitLocationID();
            Init_Blocks();
        }

        private void Init_Blocks()
        {
            int tempKey = Server.getRecipeKey(ovenName, row, column);
            string tempProgID = Server.getCurrProgressionID(ovenName, row, column);

            int[] placeHolder = Conversions.progressionIDToArray(tempProgID);
            int index = Conversions.getLastProgressionID(placeHolder);
            int currProgID = placeHolder[index];

            string tempContainerNums = Server.getContainerNums(ovenName, row, column);
            string tempComments = Server.getComments(ovenName, row, column);
            string tempName = Server.getRecipeName(tempKey);

            DateTime[] tempLimits = Server.getLimits(currProgID);
            DateTime tempStartTime = Server.getStartTime(ovenName, row, column);
            DateTime currTime = DateTime.Now;
            TimeSpan tempSpans1 = Conversions.DateTimeToTimeSpan(tempLimits[0]);
            TimeSpan tempSpans2 = currTime.Subtract(tempStartTime);
            tempRem = tempSpans1.Subtract(tempSpans2);

            string tempTime = "0:0:0:0";
            if (tempRem.Ticks > 0 && !tempStartTime.Equals(Conversions.nullity))
            {
                disTimer = new DispatcherTimer();
                disTimer.Interval = secTick;   //current timer increments in 1 seconds
                disTimer.Tick += DisTimer_Tick;
                //tempTime = string.Format("{0}:{1}:{2}:{3}", (int)tempRem.Days, (int)tempRem.Hours, (int)tempRem.Minutes, (int)tempRem.Seconds);
                tempTime = Conversions.StripMiliSec(tempRem).ToString();
                timerLimit = (int)tempRem.TotalSeconds;
                disTimer.Start();
            }

            Block_RecipeKey.Text = " " + tempKey + " ";
            Block_ProgressionID.Text = " " + Conversions.formatProgressionIDString(tempProgID) + " ";
            Block_Container.Text = " " + tempContainerNums + " ";
            Block_Comments.Text = " " + tempComments + " ";
            Block_RecipeName.Text = " " + tempName + " ";
            Block_Timer.Text = " " + tempTime + " ";
        }

        private void InitLocationID()
        {
            LocationID.Text = row.ToString() + (column + 1).ToString();
        }

        private void SubmitHelper()
        {
            Server.UpdateComments(ovenName, row, column, Block_Comments.Text);
            Server.UpdateContainerNums(ovenName, row, column, Block_Container.Text);
            this.Close();
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (disTimer != null)
            {
                disTimer.Stop();
            }
        }

        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if (timerLimit > 0)
            {
                //Block_Timer.Text = " " + string.Format("{0}:{1}:{2}:{3}", (int)tempRem.Days, (int)tempRem.Hours, (int)tempRem.Minutes, (int)tempRem.Seconds) + " ";
                Block_Timer.Text = " " + Conversions.StripMiliSec(tempRem).ToString() + " ";
                tempRem = tempRem.Subtract(secTick);
                timerLimit--;
            }
            else
            {
                //Block_Timer.Text = " " + string.Format("{0}:{1}:{2}:{3}", (int)tempRem.Days, (int)tempRem.Hours, (int)tempRem.Minutes, (int)tempRem.Seconds) + " ";
                Block_Timer.Text = " " + Conversions.StripMiliSec(tempRem).ToString() + " ";
                disTimer.Stop();
            }
        }
    }
}