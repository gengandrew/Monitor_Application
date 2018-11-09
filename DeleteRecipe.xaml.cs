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
    /// Interaction logic for DeleteRecipe.xaml
    /// </summary>
    public partial class DeleteRecipe : Window
    {
        public DeleteRecipe()
        {
            InitializeComponent();
            FillComboBox();
        }

        private void SubmitHelper()
        {
            string recipeName = ComboBox.Text;
            if (recipeName.Length == 0)
            {
                MessageBox.Show("Invalid Recipe Name, Please Try Again.");
            }
            else
            {
                if (MessageBox.Show(string.Format("Are you sure you want to remove the Recipe '{0}'?", recipeName),
                    "Remove Recipe", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Server.DeleteRecipes(recipeName);
                    Server.DeleteRecipePointer(recipeName);
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

        private void Submit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SubmitHelper();
            }
        }
    }
}