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
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for AddRecipe.xaml
    /// </summary>
    public partial class AddRecipe : Window
    {
        private int counter;
        private ArrayList ArrayLimits;
        private ArrayList ArrayOptionals;

        public AddRecipe()
        {
            ArrayOptionals = new ArrayList();
            ArrayLimits = new ArrayList();
            counter = 0;
            InitializeComponent();
        }

        private void addProgression()
        {
            counter++;
            Grid innerGrid = new Grid();
            innerGrid.Children.Add(initHeaderGrid());
            innerGrid.Children.Add(initTextBoxGrid());
            innerGrid.Children.Add(initTextBlockGrid());

            Border myBorder = new Border();
            myBorder.BorderBrush = Brushes.Black;
            Thickness thick = myBorder.BorderThickness;
            thick.Top = 2;
            thick.Bottom = 2;
            thick.Right = 2;
            thick.Left = 2;
            myBorder.BorderThickness = thick;
            Thickness margin = myBorder.Margin;
            margin.Bottom = 10;
            myBorder.Margin = margin;

            myBorder.Width = 300;
            CornerRadius radi = myBorder.CornerRadius;
            radi.BottomLeft = 12;
            radi.BottomRight = 12;
            radi.TopLeft = 12;
            radi.TopRight = 12;
            myBorder.CornerRadius = radi;
            myBorder.Background = Brushes.Gray;
            myBorder.Child = innerGrid;
            Grid mainGrid = new Grid();
            mainGrid.Width = 330;
            mainGrid.Children.Add(myBorder);
            WrapContent.Children.Insert(counter - 1, mainGrid);
        }

        private Grid initHeaderGrid()
        {
            Grid header = new Grid();
            header.Width = 280;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            header.VerticalAlignment = VerticalAlignment.Top;

            TextBlock txt = new TextBlock();
            txt.Text = "Progression " + counter;
            txt.FontSize = 15;
            txt.SetValue(FontWeightProperty, FontWeights.Bold);
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.VerticalAlignment = VerticalAlignment.Top;

            CheckBox optional = new CheckBox();
            optional.Content = "Optional";
            optional.HorizontalAlignment = HorizontalAlignment.Right;
            optional.VerticalAlignment = VerticalAlignment.Top;
            optional.SetValue(FontWeightProperty, FontWeights.Bold);
            Thickness margin = optional.Margin;
            margin.Top = 5;
            optional.Margin = margin;

            header.Children.Add(txt);
            header.Children.Add(optional);
            ArrayOptionals.Add(optional);
            return header;
        }

        private Grid initTextBoxGrid()
        {
            Grid textBoxGrid = new Grid();
            textBoxGrid.Width = 280;
            textBoxGrid.VerticalAlignment = VerticalAlignment.Top;
            Thickness margin = textBoxGrid.Margin;
            margin.Top = 28;
            margin.Bottom = 10;
            textBoxGrid.Margin = margin;

            TextBox Limit1 = new TextBox();
            Limit1.Name = "Running";
            Limit1.Text = "";
            Limit1.Width = 85;
            Limit1.HorizontalAlignment = HorizontalAlignment.Left;
            Limit1.PreviewMouseDown += SetLimitTime;

            TextBox Limit2 = new TextBox();
            Limit2.Name = "Ready";
            Limit2.Text = "";
            Limit2.Width = 85;
            Limit2.HorizontalAlignment = HorizontalAlignment.Center;
            Limit2.PreviewMouseDown += SetLimitTime;

            TextBox Limit3 = new TextBox();
            Limit3.Name = "Alert";
            Limit3.Text = "";
            Limit3.Width = 85;
            Limit3.HorizontalAlignment = HorizontalAlignment.Right;
            Limit3.PreviewMouseDown += SetLimitTime;

            TextBox[] Limits = new TextBox[3];
            Limits[0] = Limit1;
            Limits[1] = Limit2;
            Limits[2] = Limit3;
            ArrayLimits.Add(Limits);
            textBoxGrid.Children.Add(Limit1);
            textBoxGrid.Children.Add(Limit2);
            textBoxGrid.Children.Add(Limit3);
            return textBoxGrid;
        }

        private Grid initTextBlockGrid()
        {
            Grid textBlockGrid = new Grid();
            textBlockGrid.Width = 250;
            textBlockGrid.VerticalAlignment = VerticalAlignment.Top;
            Thickness margin = textBlockGrid.Margin;
            margin.Right = 5;
            margin.Top = 45;
            textBlockGrid.Margin = margin;

            TextBlock txt1 = new TextBlock();
            txt1.Width = 60;
            txt1.Text = "Running";
            txt1.FontSize = 15;
            txt1.HorizontalAlignment = HorizontalAlignment.Left;

            TextBlock txt2 = new TextBlock();
            txt2.Width = 50;
            txt2.Text = "Ready";
            txt2.FontSize = 15;
            txt2.HorizontalAlignment = HorizontalAlignment.Center;
            Thickness margin2 = txt2.Margin;
            margin2.Left = 10;
            txt2.Margin = margin2;

            TextBlock txt3 = new TextBlock();
            txt3.Width = 50;
            txt3.Text = "Alert";
            txt3.FontSize = 15;
            txt3.HorizontalAlignment = HorizontalAlignment.Right;

            textBlockGrid.Children.Add(txt1);
            textBlockGrid.Children.Add(txt2);
            textBlockGrid.Children.Add(txt3);
            return textBlockGrid;
        }

        private void SubmitHelper()
        {
            string recipeName = RecipeName.Text;
            if (recipeName.Length > 3 && ArrayLimits.Count > 0)
            {
                if (MessageBox.Show(string.Format("Are you sure you want to Add the Recipe '{0}'?", RecipeName.Text),
                    "Add Recipe", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    string progressionID = "";
                    for (int i = 0; i < ArrayLimits.Count; i++)
                    {
                        TextBox[] limits = ArrayLimits[i] as TextBox[];
                        DateTime limit1 = DateTime.Parse("0001-01-01 " + limits[0].Text);
                        DateTime limit2 = DateTime.Parse("0001-01-01 " + limits[1].Text);
                        DateTime limit3 = DateTime.Parse("0001-01-01 " + limits[2].Text);
                        CheckBox optionalBox = ArrayOptionals[i] as CheckBox;
                        bool optional = false;
                        if (optionalBox.IsChecked.HasValue && optionalBox.IsChecked.Value == true)
                        {
                            optional = true;
                        }
                        int placeHolder = Server.InsertRecipes(optional, MainWindow.username, limit1, limit2, limit3);
                        if (i != ArrayLimits.Count - 1)
                        {
                            progressionID = progressionID + placeHolder.ToString() + ",";
                        }
                        else
                        {
                            progressionID = progressionID + placeHolder.ToString();
                        }
                    }
                    Server.InsertRecipePointer(RecipeName.Text, progressionID);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Recipe Name or Time Limit Values are Invalid, Please Try Again.");
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SubmitHelper();
        }

        private void SetLimitTime(object sender, MouseButtonEventArgs e)
        {
            string limitName = (sender as TextBox).Name;
            AddRecipeHelper myHelper = new AddRecipeHelper(limitName);
            myHelper.ShowDialog();
            TimeSpan outTime = myHelper.getTime();
            (sender as TextBox).Text = outTime.ToString();
        }

        private void AddProgression_Click(object sender, RoutedEventArgs e)
        {
            addProgression();
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
    }
}