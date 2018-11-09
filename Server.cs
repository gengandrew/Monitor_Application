using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Configuration;
using System.Data.OleDb;
using System.Collections;
namespace TimeTracker
{
    class Server
    {
        public static DateTime[,] dataStructure;

        public static DateTime nullity = new DateTime(001, 1, 1);

        public static void DeleteOvenPointer(string ovenName)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand commandPointer = new OleDbCommand();
            MainWindow.connect.Open();
            commandPointer.Connection = MainWindow.connect;
            string queryPointer = "DELETE FROM OvenPointer WHERE OvenKey = " + ovenKey + ";";
            commandPointer.CommandText = queryPointer;
            int affectedPointer = commandPointer.ExecuteNonQuery();
            if (affectedPointer != 0)
            {
                MessageBox.Show("Failure to execute Drop Oven");
            }
            MainWindow.connect.Close();
        }

        public static void DeleteOven(string ovenName)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "DELETE FROM Oven WHERE OvenKey = " + ovenKey + ";";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failure to execute Drop Oven");
            }
            MainWindow.connect.Close();
        }

        public static void DeleteRecipePointer(string RecipeName)
        {
            OleDbCommand commandPointer = new OleDbCommand();
            MainWindow.connect.Open();
            commandPointer.Connection = MainWindow.connect;
            string queryPointer = "DELETE FROM RecipePointer WHERE Name = '" + RecipeName + "';";
            commandPointer.CommandText = queryPointer;
            int affectedPointer = commandPointer.ExecuteNonQuery();
            if (affectedPointer != 0)
            {
                MessageBox.Show("Failure to execute Drop Oven");
            }
            MainWindow.connect.Close();
        }

        public static void DeleteRecipes(string RecipeName)
        {
            int[] ProgressionID = Conversions.progressionIDToArray(getProgressionID(RecipeName));
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "";
            for (int i = 0; i < ProgressionID.Length; i++)
            {
                query = query + "DELETE FROM Recipes WHERE ProgressionID = " + ProgressionID[i] + ";";
            }
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failure to execute Drop Oven");
            }
            MainWindow.connect.Close();
        }

        public static void DropOvenTables()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "USE TimeTracker; DROP TABLE OvenPointer; DROP TABLE Oven;";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failure to Drop Recipes");
            }
            MainWindow.connect.Close();
        }

        public static void DropRecipesTables()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "USE TimeTracker; DROP TABLE RecipePointer; DROP TABLE Recipes;";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failure to Drop Recipes");
            }
            MainWindow.connect.Close();
        }

        public static void DropAdminDomain()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "USE TimeTracker; DROP TABLE AdminDomains;";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failure to Drop AdminDomains");
            }
            MainWindow.connect.Close();
        }

        public static void clearOvenComments(string ovenName, char row, int column)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            string query = "UPDATE Oven SET Comments = '' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            query = query + "UPDATE Oven SET ContainerNums = '' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            query = query + "UPDATE Oven SET CurrProgressionID = '0' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            query = query + "UPDATE Oven SET RecipeKey = 1 WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            command.CommandText = query;
            command.Connection = MainWindow.connect;
            int placeHolder = command.ExecuteNonQuery();
            if (placeHolder > 0)
            {
                MainWindow.connect.Close();
            }
            else
            {
                MainWindow.connect.Close();
                MessageBox.Show("Database Entry has Failed");
                Environment.Exit(1);
            }
        }

        public static void CreateAdminDomains()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "CREATE TABLE AdminDomains(DomainKey int NOT NULL IDENTITY PRIMARY KEY, AdminDomainName VARCHAR(50) NOT NULL);";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failed to create AdminDomains");
            }
            MainWindow.connect.Close();
        }

        public static void CreateOvenPointerTable()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "CREATE TABLE OvenPointer(OvenKey int NOT NULL IDENTITY PRIMARY KEY, OvenName VARCHAR(50) NOT NULL);";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failed to create Oven");
            }
            MainWindow.connect.Close();
        }

        public static void CreateOvenTable()
        {
            OleDbCommand commandCreate = new OleDbCommand();
            MainWindow.connect.Open();
            commandCreate.Connection = MainWindow.connect;
            string queryCreate = "CREATE TABLE Oven(OvenKey int NOT NULL, OvenRow Char NOT NULL, OvenColumn int NOT NULL, RecipeKey int NOT NULL, CurrProgressionID VARCHAR(50) NOT NULL, StartTime datetime2(7) NOT NULL, ContainerNums varchar(500) NOT NULL, Comments varchar(500) NOT NULL);";
            commandCreate.CommandText = queryCreate;
            int affectedCreate = commandCreate.ExecuteNonQuery();
            if (affectedCreate != 0)
            {
                MessageBox.Show("Failed to create Oven");
            }
            MainWindow.connect.Close();
        }

        public static void createRecipePointerTable()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "CREATE TABLE RecipePointer(RecipeKey int NOT NULL IDENTITY PRIMARY KEY, Name varchar(50) NOT NULL, ProgressionID VARCHAR(50) NOT NULL);";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected != 0)
            {
                MessageBox.Show("Failed to create RecipePointer");
            }
            MainWindow.connect.Close();
        }

        public static void CreateRecipesTable()
        {
            OleDbCommand commandCreate = new OleDbCommand();
            MainWindow.connect.Open();
            commandCreate.Connection = MainWindow.connect;
            string queryCreate = "CREATE TABLE Recipes(ProgressionID int NOT NULL IDENTITY PRIMARY KEY, Limit1 datetime2(7) NOT NULL, Limit2 datetime2(7) NOT NULL, Limit3 datetime2(7) NOT NULL, isOptional bit NOT NULL, lastUser varchar(50) NOT NULL);";
            commandCreate.CommandText = queryCreate;
            int affectedCreate = commandCreate.ExecuteNonQuery();
            if (affectedCreate != 0)
            {
                MessageBox.Show("Failed to create Recipes");
            }
            MainWindow.connect.Close();
        }

        public static void InitOvenPointer(string ovenName)
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "INSERT INTO OvenPointer (OvenName) VALUES ('" + ovenName + "');";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected <= 0)
            {
                MessageBox.Show("Failed to create");
            }
            MainWindow.connect.Close();
        }

        public static void InitOven(string ovenName, int rowCount, int colCount)
        {
            int OvenKey = getOvenKey(ovenName);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    OleDbCommand command = new OleDbCommand();
                    MainWindow.connect.Open();
                    command.Connection = MainWindow.connect;
                    char UniqueChar = 'A';
                    UniqueChar = (char)((int)UniqueChar + i);
                    string query = "INSERT INTO Oven(OvenKey, OvenRow, OvenColumn, RecipeKey, CurrProgressionID, StartTime, ContainerNums, Comments) VALUES(" + OvenKey + ", '" + UniqueChar + "', " + j + ", 1, '0', '0001-01-01', '', '');";
                    command.CommandText = query;
                    int affected = command.ExecuteNonQuery();
                    if (affected <= 0)
                    {
                        MessageBox.Show("Failed to create");
                    }
                    MainWindow.connect.Close();
                }
            }
        }

        public static int UpdateOven(string ovenName, char row, int column, DateTime StartTime, int RecipeKey, int[] CurrProgressionID)
        {
            int ovenKey = getOvenKey(ovenName);
            string ProgressionID = Conversions.progressionIDToString(CurrProgressionID);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            string query = "UPDATE Oven SET StartTime = '" + StartTime + "' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            //query = query + "UPDATE Oven SET ContainerNums = '" + ContainerNums + "' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            query = query + "UPDATE Oven SET RecipeKey = " + RecipeKey + " WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            query = query + "UPDATE Oven SET CurrProgressionID = '" + ProgressionID + "' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            command.CommandText = query;
            command.Connection = MainWindow.connect;
            int placeHolder = command.ExecuteNonQuery();
            if (placeHolder > 0)
            {
                MainWindow.connect.Close();
                return 1;
            }
            else
            {
                MainWindow.connect.Close();
                MessageBox.Show("Database Entry has Failed");
                Environment.Exit(1);
                return -1;
            }
        }

        public static void UpdateComments(string ovenName, char row, int column, string comments)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            string query = "UPDATE Oven SET Comments = '" + comments + "' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + ";";
            command.CommandText = query;
            command.Connection = MainWindow.connect;
            int placeHolder = command.ExecuteNonQuery();
            if (placeHolder > 0)
            {
                MainWindow.connect.Close();
            }
            else
            {
                MainWindow.connect.Close();
                MessageBox.Show("Database Entry has Failed");
                Environment.Exit(1);
            }
        }

        public static void UpdateContainerNums(string ovenName, char row, int column, string containerNums)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            string query = "UPDATE Oven SET ContainerNums = '" + containerNums + "' WHERE OvenRow = '" + row + "' AND OvenColumn = " + column + " AND OvenKey = " + ovenKey + "; ";
            command.CommandText = query;
            command.Connection = MainWindow.connect;
            int placeHolder = command.ExecuteNonQuery();
            if (placeHolder > 0)
            {
                MainWindow.connect.Close();
            }
            else
            {
                MainWindow.connect.Close();
                MessageBox.Show("Database Entry has Failed");
                Environment.Exit(1);
            }
        }

        public static DateTime getStartTime(string ovenName, char Row, int Column)
        {
            int ovenKey = getOvenKey(ovenName);
            DateTime placeHolder = new DateTime();
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM Oven;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int keyTemp = Int32.Parse(scnr["OvenKey"].ToString());
                char charTemp = char.Parse(scnr["OvenRow"].ToString());
                int intTemp = Int32.Parse(scnr["OvenColumn"].ToString());
                if (charTemp == Row && intTemp == Column && keyTemp.Equals(ovenKey))
                {
                    placeHolder = DateTime.Parse(scnr["StartTime"].ToString());
                    break;
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return placeHolder;
        }

        public static void InitRecipePointer()
        {
            /*
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "INSERT INTO RecipePointer(Name, ProgressionID) VALUES ('test0', '1');" +
                "INSERT INTO RecipePointer(Name, ProgressionID) VALUES ('test1', '1,2,3');" +
                "INSERT INTO RecipePointer(Name, ProgressionID) VALUES ('test2', '1');" +
                "INSERT INTO RecipePointer(Name, ProgressionID) VALUES ('test3', '1');" +
                "INSERT INTO RecipePointer(Name, ProgressionID) VALUES ('test4', '1');";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected <= 0)
            {
                MessageBox.Show("Failed to create");
            }
            MainWindow.connect.Close();
            */
        }

        public static void InitRecipes()     //Initlaize with real recipes
        {
            /*
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "INSERT INTO Recipes(Limit1, Limit2, Limit3, isOptional, lastUser) VALUES ('0001-01-01 00:00:02.0000000', '0001-01-01 00:00:04.0000000', '0001-01-01 00:00:05.0000000', 0, 'genga');" +
                "INSERT INTO Recipes(Limit1, Limit2, Limit3, isOptional, lastUser) VALUES ('0001-01-01 00:00:03.0000000', '0001-01-01 00:00:06.0000000', '0001-01-01 00:00:10.0000000', 0, 'genga'); " +
                "INSERT INTO Recipes(Limit1, Limit2, Limit3, isOptional, lastUser) VALUES ('0001-01-01 00:00:01.0000000', '0001-01-01 00:00:02.0000000', '0001-01-01 00:00:03.0000000', 0, 'genga');" +
                "INSERT INTO Recipes(Limit1, Limit2, Limit3, isOptional, lastUser) VALUES ('0001-01-01', '0001-01-01', '0001-01-01', 0, 'genga');" +
                "INSERT INTO Recipes(Limit1, Limit2, Limit3, isOptional, lastUser) VALUES ('0001-01-01', '0001-01-01', '0001-01-01', 0, 'genga');";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected <= 0)
            {
                MessageBox.Show("Failure to Initilize Recipes Table");
            }
            MainWindow.connect.Close();
            */
        }

        public static int InsertRecipePointer(string name, string ProgressionID)
        {
            int output = -1;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "INSERT INTO RecipePointer(Name, ProgressionID) VALUES ('" + name + "', '" + ProgressionID + "');";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected <= 0)
            {
                MessageBox.Show("Failed to Insert Into RecipePointer");
            }
            MainWindow.connect.Close();

            OleDbCommand commandSel = new OleDbCommand();
            MainWindow.connect.Open();
            commandSel.Connection = MainWindow.connect;
            string querySel = "SELECT * FROM RecipePointer;";
            commandSel.CommandText = querySel;
            OleDbDataReader scnr = commandSel.ExecuteReader();
            while (scnr.Read())
            {
                string placeHolderName = scnr["Name"].ToString();
                string placeHolderID = scnr["ProgressionID"].ToString();
                if (placeHolderName.Equals(name) && placeHolderID.Equals(ProgressionID))
                {
                    output = (int)scnr["RecipeKey"];
                }
            }
            MainWindow.connect.Close();
            return output;
        }

        public static int InsertRecipes(bool isOptional, string userName, DateTime Limit1, DateTime Limit2, DateTime Limit3)
        {
            int output = -1;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "INSERT INTO Recipes(Limit1, Limit2, Limit3, isOptional, lastUser) VALUES ('" + Limit1 + "', '" + Limit2 + "', '" + Limit3 + "', " + Conversions.boolToInt(isOptional) + ", '" + userName + "');";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected <= 0)
            {
                MessageBox.Show("Failure to add new Recipe");
            }
            MainWindow.connect.Close();

            OleDbCommand commandSel = new OleDbCommand();
            MainWindow.connect.Open();
            commandSel.Connection = MainWindow.connect;
            string querySel = "SELECT * FROM Recipes;";
            commandSel.CommandText = querySel;
            OleDbDataReader scnr = commandSel.ExecuteReader();
            while (scnr.Read())
            {
                Console.WriteLine(scnr["isOptional"].ToString());
                bool placeHolderOptional = Convert.ToBoolean(scnr["isOptional"].ToString());
                string placeHolderName = scnr["lastUser"].ToString();
                DateTime placeHolderL1 = DateTime.Parse(scnr["Limit1"].ToString());
                DateTime placeHolderL2 = DateTime.Parse(scnr["Limit2"].ToString());
                DateTime placeHolderL3 = DateTime.Parse(scnr["Limit3"].ToString());
                if (isOptional == placeHolderOptional && userName.Equals(placeHolderName) && Limit1.Equals(placeHolderL1) &&
                    Limit2.Equals(placeHolderL2) && Limit3.Equals(placeHolderL3))
                {
                    output = (int)scnr["ProgressionID"];
                }
            }
            MainWindow.connect.Close();
            return output;
        }

        public static void InsertAdminDomain(string AdminDomain)
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "INSERT INTO AdminDomains(AdminDomainName) VALUES ('" + AdminDomain + "');";
            command.CommandText = query;
            int affected = command.ExecuteNonQuery();
            if (affected <= 0)
            {
                MessageBox.Show("Failed to Insert Into AdminDomain");
            }
            MainWindow.connect.Close();
        }

        public static string getProgressionID(string RecipeName)
        {
            string output = "";
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM RecipePointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                string temp = scnr["Name"].ToString();
                if (temp.Equals(RecipeName))
                {
                    output = scnr["ProgressionID"].ToString();
                    break;
                }
            }
            MainWindow.connect.Close();
            return output;
        }

        public static int getOvenKey(string ovenName)
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            int ovenKey = 0;
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM OvenPointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                string temp = scnr["OvenName"].ToString();
                if (temp.Equals(ovenName))
                {
                    ovenKey = Int32.Parse(scnr["OvenKey"].ToString());
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return ovenKey;
        }

        public static string getOvenName(int ovenKey)
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            string ovenName = "";
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM OvenPointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int temp = Int32.Parse(scnr["OvenKey"].ToString());
                if (temp == ovenKey)
                {
                    ovenName = scnr["OvenName"].ToString();
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return ovenName;
        }

        public static int getRecipeKey(string ovenName, char Row, int Column)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            int RecipeKey = -1;
            command.Connection = MainWindow.connect;
            String query = "SELECT * FROM Oven;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int keyTemp = Int32.Parse(scnr["OvenKey"].ToString());
                char charTemp = char.Parse(scnr["OvenRow"].ToString());
                int intTemp = (int)scnr["OvenColumn"];
                if (charTemp == Row && intTemp == Column && keyTemp == ovenKey)
                {
                    RecipeKey = (int)scnr["RecipeKey"];
                    break;
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return RecipeKey;
        }

        public static string getRecipeName(int RecipeKey)
        {
            string output = "";
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM RecipePointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int temp = int.Parse(scnr["RecipeKey"].ToString());
                if (temp == RecipeKey)
                {
                    output = scnr["Name"].ToString();
                    break;
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return output;
        }

        public static string[] getRecipeName()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM RecipePointer;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            string[] output = new string[6];
            ArrayList names = new ArrayList();
            int counter = 0;
            while (scnr.Read())
            {
                string temp = scnr["Name"].ToString();
                if (counter < 6)
                {
                    output[counter] = temp;
                }
                counter++;
            }
            scnr.Close();
            MainWindow.connect.Close();
            return output;
        }

        public static string getComments(string ovenName, char Row, int Column)
        {
            int ovenKey = getOvenKey(ovenName);
            string output = "";
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM Oven;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int keytemp = Int32.Parse(scnr["OvenKey"].ToString());
                char tempRow = char.Parse(scnr["OvenRow"].ToString());
                int tempCol = int.Parse(scnr["OvenColumn"].ToString());
                if (tempRow == Row && tempCol == Column && ovenKey == keytemp)
                {
                    output = scnr["Comments"].ToString();
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return output;
        }

        public static string getLastUser(int ProgressionID)
        {
            string output = "";
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM Recipes;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int temp = Int32.Parse(scnr["ProgressionID"].ToString());
                if (temp == ProgressionID)
                {
                    output = scnr["lastUser"].ToString();
                }
            }
            MainWindow.connect.Close();
            return output;
        }

        public static DateTime[] getLimits(int ProgressionID)
        {
            DateTime[] output = new DateTime[3];
            if (ProgressionID == -1)
            {
                MessageBox.Show("Can not get Recipe");
                return null;
            }
            else
            {
                OleDbCommand command = new OleDbCommand();
                MainWindow.connect.Open();
                command.Connection = MainWindow.connect;
                String query = "SELECT * FROM Recipes;";
                command.CommandText = query;
                OleDbDataReader scnr = command.ExecuteReader();
                while (scnr.Read())
                {
                    int temp = (int)scnr["ProgressionID"];
                    if (temp == ProgressionID)
                    {
                        output[0] = DateTime.Parse(scnr["Limit1"].ToString());
                        output[1] = DateTime.Parse(scnr["Limit2"].ToString());
                        output[2] = DateTime.Parse(scnr["Limit3"].ToString());
                        break;
                    }
                }
                scnr.Close();
                MainWindow.connect.Close();
                return output;
            }
        }

        public static string getCurrProgressionID(string ovenName, char Row, int Column)
        {
            int ovenKey = getOvenKey(ovenName);
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            string ProgressionID = "";
            command.Connection = MainWindow.connect;
            String query = "SELECT * FROM Oven;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int keyTemp = Int32.Parse(scnr["OvenKey"].ToString());
                char charTemp = char.Parse(scnr["OvenRow"].ToString());
                int intTemp = Int32.Parse(scnr["OvenColumn"].ToString());
                if (charTemp == Row && intTemp == Column && keyTemp == ovenKey)
                {
                    ProgressionID = scnr["CurrProgressionID"].ToString();
                    break;
                }
            }
            scnr.Close();
            MainWindow.connect.Close();
            return ProgressionID;
        }

        public static bool getRecipesIsOptional(int ProgressionID)
        {
            bool output = false;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            String query = "SELECT * FROM Recipes;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int temp = (int)scnr["ProgressionID"];
                if (temp == ProgressionID)
                {
                    output = (bool)scnr["isOptional"];
                    break;
                }
            }
            MainWindow.connect.Close();
            return output;
        }

        public static string getContainerNums(string ovenName, char Row, int Column)
        {
            int ovenKey = getOvenKey(ovenName);
            string output = null;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM Oven;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                int keyTemp = Int32.Parse(scnr["OvenKey"].ToString());
                char charTemp = char.Parse(scnr["OvenRow"].ToString());
                int intTemp = Int32.Parse(scnr["OvenColumn"].ToString());
                if (charTemp == Row && intTemp == Column && keyTemp == ovenKey)
                {
                    output = scnr["ContainerNums"].ToString();
                    break;
                }
            }
            MainWindow.connect.Close();
            return output;
        }

        public static int getColumnSizeFromDB(string ovenName)
        {
            int ovenKey = getOvenKey(ovenName);
            int output = -1;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECt COUNT(DISTINCT OvenColumn) as count from Oven WHERE OvenKey = " + ovenKey + ";";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                output = (int)scnr["count"];
            }
            scnr.Close();
            MainWindow.connect.Close();
            return output;
        }

        public static int getRowSizeFromDB(string ovenName)
        {
            int ovenKey = getOvenKey(ovenName);
            int output = -1;
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECt COUNT(DISTINCT OvenRow) as count from Oven WHERE OvenKey = " + ovenKey + ";";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                output = (int)scnr["count"];
            }
            scnr.Close();
            MainWindow.connect.Close();
            return output;
        }

        public static void readAllFromDataBase(string ovenName)
        {
            int ovenKey = getOvenKey(ovenName);
            dataStructure = new DateTime[InitBaseGrid.columnCount, InitBaseGrid.rowCount];
            for (int i = 0; i < InitBaseGrid.rowCount; i++)
            {
                for (int j = 0; j < InitBaseGrid.columnCount; j++)
                {
                    OleDbCommand command = new OleDbCommand();
                    MainWindow.connect.Open();
                    command.Connection = MainWindow.connect;
                    char UniqueChar = 'A';
                    UniqueChar = (char)((int)UniqueChar + i);
                    string query = "SELECT * FROM Oven WHERE OvenRow = '" + UniqueChar + "' AND OvenColumn = " + j + " AND OvenKey = " + ovenKey + ";";
                    command.CommandText = query;
                    OleDbDataReader scnr = command.ExecuteReader();
                    while (scnr.Read())
                    {
                        DateTime temp = DateTime.Parse(scnr["StartTime"].ToString());
                        Console.WriteLine(j + " " + i);
                        Console.WriteLine(InitBaseGrid.columnCount + " " + InitBaseGrid.rowCount);
                        dataStructure[j, i] = temp;
                    }
                    scnr.Close();
                    MainWindow.connect.Close();
                }
            }
            MainWindow.connect.Close();
            Conversions.testPrint2DArray(dataStructure);
        }

        public static void makeOven(string ovenName, int rowCount, int colCount)
        {
            InitOvenPointer(ovenName);
            InitOven(ovenName, rowCount, colCount);
        }

        public static void resetOven(string ovenName, int rowCount, int colCount)
        {
            DeleteOven(ovenName);
            InitOven(ovenName, rowCount, colCount);
        }

        public static void DropDataBase(string ovenName)
        {
            DropOvenTables();
            DropRecipesTables();
        }

        public static void createDataBase(string ovenName, int rowCount, int colCount)
        {
            CreateOvenTable();
            CreateOvenPointerTable();
            CreateRecipesTable();
            createRecipePointerTable();
            InitOvenPointer(ovenName);
            InitOven(ovenName, rowCount, colCount);
            InitRecipePointer();
            InitRecipes();
        }
    }
}
