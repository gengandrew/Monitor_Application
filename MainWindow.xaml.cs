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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;
using System.Configuration;
using System.Data.OleDb;
namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static OleDbConnection connect = new OleDbConnection();

        private int Counter;
        public static string username;
        public static string groupID;
        public static bool isAdmin;

        public MainWindow()
        {
            connect.ConnectionString = ConfigurationManager.ConnectionStrings["connectString"].ToString();
            Counter = 0;
            InitializeComponent();
        }

        private void LoginHelper()
        {
            if (Box_Username.Text.Length > 3 && Box_Password.Password.Length != 0)
            {
                if (Box_Username.Text.Equals("operator") && Box_Password.Password.Equals("pass4me"))
                {
                    username = Box_Username.Text;
                    groupID = "operator";
                    isAdmin = false;
                    Watcher watcher = new Watcher();
                    this.Hide();
                    watcher.ShowDialog();
                    Box_Password.Password = "";
                    this.Show();
                }
                else
                {
                    String Username = Box_Username.Text;
                    User myUser = new User(Username, Box_Password.Password);
                    //bool ifExist = myUser.checkIfExist();
                    bool ifExist = myUser.checkIfExistOnDomain();
                    if (ifExist)
                    {
                        String write = Username + " logged on successful\r\n";
                        System.IO.File.AppendAllText(@"M:\Andrew Geng\Logs\Week1_Log.txt", write);
                        username = Username;
                        groupID = myUser.getGroupID();
                        isAdmin = myUser.checkIfAdmin();
                        Watcher watcher = new Watcher();
                        this.Hide();
                        watcher.ShowDialog();
                        Box_Password.Password = "";
                        this.Show();
                        //System.IO.File.AppendAllText(@"C:\Users\andre\OneDrive\Desktop\Login Results\Log.txt", write);
                    }
                    else
                    {
                        MessageBox.Show("Username or Password was Invalid, Please Try Again.");
                        String write = Username + " Failure to login. Attempt " + Counter;
                        System.IO.File.AppendAllText(@"M:\Andrew Geng\Logs\Week1_Log.txt", write);
                        System.IO.File.AppendAllText(@"M:\Andrew Geng\Logs\Week1_Log.txt", "\r\n");
                        Box_Password.Password = "";
                        //System.IO.File.AppendAllText(@"C:\Users\andre\OneDrive\Desktop\Login Results\Log.txt", write);
                        //System.IO.File.AppendAllText(@"C:\Users\andre\OneDrive\Desktop\Login Results\Log.txt", "\r\n");
                    }
                    Counter++;
                }
            }
            else
            {
                MessageBox.Show("Username or Password was Invalid, Please Try Again.");
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void DragEvent(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            LoginHelper();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Server.DropOvenTables();
            //Server.DropRecipesTables();
            //Server.createDataBase("Oven1");
            //Server.makeOven("Oven3", 3, 6);
            //Server.DeleteOven("Oven2");
            //Server.createDataBase("Oven", 3, 6);
            Server.CreateOvenPointerTable();
            Server.CreateOvenTable();
            Server.createRecipePointerTable();
            Server.CreateRecipesTable();
            Server.CreateAdminDomains();
            Server.InsertAdminDomain("GDTNA BSC STP MFGAUTO");
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoginHelper();
            }
        }
    }

    public partial class App : Application
    {
        private Mutex mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            mutex = new Mutex(true, @"Global\TimeTracker0.4.2", out createdNew);  //changing this to become unique 
            if (!createdNew)
            {
                mutex = null;
                MessageBox.Show("An Instance of this exe already exists. Please close it and restart the application.");
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
            }
            base.OnExit(e);
        }
    }
}
