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

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for AddRecipeHelper.xaml
    /// </summary>
    public partial class AddRecipeHelper : Window
    {
        private TimeSpan time;

        public AddRecipeHelper(string inLimit)
        {
            InitializeComponent();
            Title.Text = inLimit + "'s Limit Value";
            InitComboBox();
        }

        private void InitComboBox()
        {
            for (int i = 0; i < 30; i++)
            {
                BoxDays.Items.Add(i.ToString());
            }

            for (int i = 0; i < 24; i++)
            {
                BoxHours.Items.Add(i.ToString());
            }

            for (int i = 0; i < 59; i++)
            {
                BoxMinutes.Items.Add(i.ToString());
                BoxSeconds.Items.Add(i.ToString());
            }

            BoxDays.SelectedIndex = 0;
            BoxHours.SelectedIndex = 0;
            BoxMinutes.SelectedIndex = 0;
            BoxSeconds.SelectedIndex = 0;
        }

        private void SubmitHelper()
        {
            try
            {
                time = new TimeSpan(Int32.Parse(BoxDays.Text), Int32.Parse(BoxHours.Text), Int32.Parse(BoxMinutes.Text), Int32.Parse(BoxSeconds.Text));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Title.Text + " is Invalid. Please try Again.");
            }
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

        public TimeSpan getTime()
        {
            return this.time;
        }
    }
}