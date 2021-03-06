﻿using System;
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
    /// Interaction logic for DeleteOven.xaml
    /// </summary>
    public partial class DeleteOven : Window
    {
        public DeleteOven()
        {
            InitializeComponent();
            FillComboBox();
        }

        private void SubmitHelper()
        {
            string ovenName = ComboBox.Text;
            if (ovenName.Length == 0)
            {
                MessageBox.Show("Invalid Oven Selected, Please Try Again");
            }
            else
            {
                if (MessageBox.Show(string.Format("Are you sure you want to remove the Oven '{0}'?", ovenName),
                    "Remove Oven", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Server.DeleteOven(ovenName);
                    Server.DeleteOvenPointer(ovenName);
                    this.Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragEvent(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
                string temp = scnr["OvenName"].ToString();
                ComboBox.Items.Add(temp);
            }
            MainWindow.connect.Close();
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
