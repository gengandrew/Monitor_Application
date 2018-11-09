using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Collections;
using System.Windows.Controls;
namespace TimeTracker
{
    class ContextMenuHelper
    {
        private ContextMenu cms = new ContextMenu();
        //private MenuItem addToCell = new MenuItem();
        private MenuItem reset = new MenuItem();
        private MenuItem properties = new MenuItem();
        /*
        private MenuItem menu1 = new MenuItem();
        private MenuItem menu2 = new MenuItem();
        private MenuItem menu3 = new MenuItem();
        private MenuItem menu4 = new MenuItem();
        private MenuItem menu5 = new MenuItem();
        */
        private Border panel;
        private Grid currGrid;
        private Label lbl;
        private int[] ProgressionID;
        private string ovenName;

        public delegate void myEventHandler(Cell input);
        public delegate void myEventHandler2(string inOvenName, Border inputBorder, Label inputLabel);

        public event myEventHandler ContextMenuClick;
        public event myEventHandler2 clearActionTaken;

        public ContextMenuHelper(string inOvenName, Border inputBorder, Label inputLabel, Grid inputGrid)
        {
            //this.addToCell.Header = "Add to Cell";
            //this.addToCell.Click += AddToCell_Click;
            this.reset.Header = "Clear Cell";
            this.reset.Click += Reset_Click;
            this.properties.Header = "Properties";
            this.properties.Click += Properties_Click;
            //cms.Items.Add(this.addToCell);
            //initMenu();
            cms.Items.Add(this.reset);
            cms.Items.Add(this.properties);
            this.ProgressionID = new int[1];
            this.ovenName = inOvenName;
            this.panel = inputBorder;
            this.lbl = inputLabel;
            this.currGrid = inputGrid;
        }

        private void Menu5_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuClickHelper(5);
        }

        private void Menu4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuClickHelper(4);
        }

        private void Menu3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuClickHelper(3);
        }

        private void Menu2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuClickHelper(2);
        }

        private void Menu1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuClickHelper(1);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            clearActionTaken(ovenName, panel, lbl);
        }

        private void Properties_Click(object sender, EventArgs e)
        {
            char UniqueChar = Conversions.intToChar(Grid.GetRow(panel));
            int UniqueInt = Grid.GetColumn(panel);
            ContextProperties myProp = new ContextProperties(ovenName, UniqueChar, UniqueInt);
            myProp.Show();
        }

        private void MenuClickHelper(int progressionID)
        {
            char UniqueChar = 'A';
            UniqueChar = (char)((int)UniqueChar + System.Windows.Controls.Grid.GetRow(panel));
            int UniqueInt = System.Windows.Controls.Grid.GetColumn(panel);
            DateTime[] DateTimeArray = Server.getLimits(progressionID);
            TimeSpan[] Limits = Conversions.DateTimeToTimeSpan(DateTimeArray);
            ProgressionID[0] = progressionID;
            Cell myCell = new Cell(ovenName, Limits, ProgressionID, 0, panel, lbl, currGrid);
            ContextMenuClick(myCell);
        }
        /*
        private void initMenu()
        {
            string[] names = Server.getRecipeName();
            if (names[1] != null)
            {
                this.menu1.Header = names[1];
                this.menu1.Click += Menu1_Click;
                this.cms.Items.Add(this.menu1);
            }
            if (names[2] != null)
            {
                this.menu2.Header = names[2];
                this.menu2.Click += Menu2_Click;
                this.cms.Items.Add(this.menu2);
            }
            if (names[3] != null)
            {
                this.menu3.Header = names[3];
                this.menu3.Click += Menu3_Click;
                this.cms.Items.Add(this.menu3);
            }
            if (names[4] != null)
            {
                this.menu4.Header = names[4];
                this.menu4.Click += Menu4_Click;
                this.cms.Items.Add(this.menu4);
            }
            if (names[5] != null)
            {
                this.menu5.Header = names[5];
                this.menu5.Click += Menu5_Click;
                this.cms.Items.Add(this.menu5);
            }
        }
        */
        public ContextMenu getContextMenu()
        {
            return this.cms;
        }
    }
}
