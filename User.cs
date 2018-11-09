using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Collections;
using System.DirectoryServices.AccountManagement;
namespace TimeTracker
{
    class User
    {
        private ArrayList adminDomains;
        private string username;
        private string password;
        private string domain;
        private string groupID;

        public User(string Name, string Pass)
        {
            adminDomains = new ArrayList();
            this.domain = "company domain";
            this.username = Name;
            this.password = Pass;
            this.groupID = getADGroup();
            InitAdminDomains();
        }

        private void InitAdminDomains()
        {
            OleDbCommand command = new OleDbCommand();
            MainWindow.connect.Open();
            command.Connection = MainWindow.connect;
            string query = "SELECT * FROM AdminDomains;";
            command.CommandText = query;
            OleDbDataReader scnr = command.ExecuteReader();
            while (scnr.Read())
            {
                adminDomains.Add(scnr["AdminDomainName"].ToString());
            }
            MainWindow.connect.Close();
        }

        public bool checkIfExistOnDomain()
        {
            /*
            //ldap ask someone ----------------------------------------------------------
            string ou = "ou=users, ou=system";  //organizational unit
            string su = @"uid=admin, ou=system";  //user id for admin
            string suPass = "pass4me";            //user pass for admin
            //---------------------------------------------------------------------------
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, "bsci.bossci.com", ou, su, suPass);
            */
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain);
            bool output = pc.ValidateCredentials(this.username, this.password);
            return output;
        }

        private string getADGroup()
        {
            string[] output = null;
            using (var ctx = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(ctx, this.username))
            {
                if (user != null)
                {
                    output = user.GetGroups().Select(x => x.SamAccountName).ToArray();
                }
            }
            if (output == null)
            {
                return "";
            }
            else
            {
                return output[1];
            }
        }

        public bool checkIfAdmin()
        {
            for (int i = 0; i < adminDomains.Count; i++)
            {
                if (adminDomains[i].ToString().Equals(this.groupID))
                {
                    return true;
                }
            }
            return false;
        }

        public bool setPassword(string input)
        {
            if (input == this.password)
            {
                return false;
            }
            else
            {
                this.password = input;
                return true;
            }
        }

        public string getUsername()
        {
            return this.username;
        }

        public void setUsername(string input)
        {
            this.username = input;
        }

        public string getGroupID()
        {
            return this.groupID;
        }
    }
}
