using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SEG.Domain.Model;
using System.IO;
using SEG.Domain.Helpers;
using SEG.Domain;

namespace SEG.Tools
{
    public class ToolManager
    {
        public string server;
        public string database;
        public string username;
        public string password;
        public bool configLoaded;

        public void LoadConfig()
        {
            try
            {
                configLoaded = false;
                if (File.Exists("seg.install.config"))
                {
                    using (StreamReader sr = new StreamReader("seg.install.config"))
                    {
                        server = sr.ReadLine();
                        database = sr.ReadLine();
                        username = sr.ReadLine();
                        password = sr.ReadLine();
                    }
                    configLoaded = true;
                    CheckDatabase();
                }
            }
            catch(Exception ex)
            {
                configLoaded = false;
            }
        }

        public bool CheckDatabase()
        {
            bool isOk = false;
            if(configLoaded)
            {
                // Load segR
                segR = new SEGRepository(ConnString());

                // 
                using(var seg = segR.GetContext())
                {
                    if (seg.RiskAnalysisTypes.Count() != 4)
                    {
                        dbError = "Database is Empty.  Initialization error.  Please delete DB and reload";
                        configLoaded = false;
                    }
                    else
                        isOk = true;
                }
            }
            return isOk;
        }

        public void SaveConfig()
        {
            try
            {
                using(StreamWriter sw = new StreamWriter("seg.install.config"))
                {
                    sw.WriteLine(server);
                    sw.WriteLine(database);
                    sw.WriteLine(username);
                    sw.WriteLine(password);
                }
            }
            catch(Exception ex)
            {
                configLoaded = false;
            }
        }

        public string ConnString()
        {
            string s = "";

            if(configLoaded)
            {
                // Server=OFDEV01\OFDB;Database=seg;User Id=sa;Password=1;
                s = "Server=" + server + ";Database=seg;User Id=" + username + ";Password=" + password;
            }

            return s;
        }

        public string ConnStringSetup()
        {
            string s = "";

            if (configLoaded)
            {
                // Server=OFDEV01\OFDB;Database=seg;User Id=sa;Password=1;
                s = "Server=" + server + ";Database=master;User Id=" + username + ";Password=" + password;
            }

            return s;
        }

        public string dbError;

        public bool SetupDatabase()
        {
            // Try to restore a backed up database file
            bool processOk = false;

            try
            {
                var sql = new SqlHelper(ConnStringSetup());
                string sfile = @"SetupData\seg-cleandb.bak";
                // Obtain absolute path
                string sfilefull = Path.GetFullPath(sfile);

                if(sql.Connect())
                {
                    string UseMaster = "USE master";
                    sql.DoCommandExecute(UseMaster);

                    string Alter1 = @"ALTER DATABASE [seg] SET Single_User WITH Rollback Immediate";
                    sql.DoCommandExecute(Alter1);

                    string Restore = @"RESTORE DATABASE [seg] FROM DISK = N'" + sfilefull + @"' WITH REPLACE";
                    sql.DoCommandExecute(Restore);

                    string Alter2 = @"ALTER DATABASE [seg] SET Multi_User";
                    sql.DoCommandExecute(Alter2);

                    //sql.DoCommandExecute("USE MASTER RESTORE DATABASE seg FROM DISK = '" + sfilefull + "' WITH REPLACE");
                    processOk = CheckDatabase();
                }
                else
                {
                    dbError = sql.errorMessage;
                }
                // Con.ExecuteCmd();
            }
            catch(Exception e)
            {

            }

            return processOk;
        }

        public SEGRepository segR;
        public List<User> users;


        public void LoadUsers()
        {
            if(configLoaded)
            {
                using(var seg = segR.GetContext())
                {
                    users = seg.Users.ToList();
                }
            }
        }

        public void AddUser(User u)
        {
            if(configLoaded)
            {
                using (var seg = segR.GetContext())
                {
                    seg.Users.Add(u);
                    seg.SaveChanges();
                }
                LoadUsers();
            }
        }

        public void UpdateUser(User u)
        {
            if (configLoaded)
            {
                using (var seg = segR.GetContext())
                {
                    var uDB = seg.Users.FirstOrDefault(x => x.Id == u.Id);
                    uDB.Active = u.Active;
                    uDB.FullName = u.FullName;
                    uDB.IsAdmin = u.IsAdmin;
                    uDB.IsApprover = u.IsApprover;
                    uDB.IsExecutor = u.IsExecutor;
                    uDB.IsVerifier = u.IsVerifier;
                    uDB.Name = u.Name;
                    uDB.Password = u.Password;
                    seg.SaveChanges();
                }
                LoadUsers();
            }
        }

        public void DeleteUser(User u)
        {
            if (configLoaded)
            {
                using (var seg = segR.GetContext())
                {
                    var uDB = seg.Users.FirstOrDefault(x => x.Id == u.Id);
                    seg.Users.Remove(uDB);
                    seg.SaveChanges();
                }
                LoadUsers();
            }
        }
    }
}
