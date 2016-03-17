using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEG.Tools
{
    public partial class winMain : Form
    {
        public winMain()
        {
            InitializeComponent();

            labGB.Parent = pictureBoxMenu;
            labSEG.Parent = pictureBoxMenu;
            labTitle.Parent = pictureBoxMenu;

            labGB.Left = labGB.Left - pictureBoxMenu.Left;
            labSEG.Left = labSEG.Left - pictureBoxMenu.Left;
            labTitle.Left = labTitle.Left - pictureBoxMenu.Left;

            labGB.Top = labGB.Top - pictureBoxMenu.Left;
            labSEG.Top = labSEG.Top - pictureBoxMenu.Left;
            labTitle.Top = labTitle.Top - pictureBoxMenu.Left;

            man = new ToolManager();
        }

        public ToolManager man;

        public void SetupPanels()
        {
            if(man.configLoaded)
            {
                eServer.Text = man.server;
                eDB.Text = man.database;
                eDBUser.Text = man.username;
                eDBPass.Text = man.password;

                eServer.ReadOnly = true;
                eDB.ReadOnly = true;
                eDBUser.ReadOnly = true;
                eDBPass.ReadOnly = true;
                bInitDB.Enabled = true;
                bInitDB.Text = "Clear Settings";
                bInitSEG.Enabled = true;
                bAccounts.Enabled = true;

                // Load and show accounts
                man.LoadUsers();

                // Show the list
                AccUpdate();
                AccClear();
            }
            else
            {
                eServer.Text = "";
                eDB.Text = "seg";
                eDBUser.Text = "";
                eDBPass.Text = "";

                eServer.ReadOnly = false;
                eDB.ReadOnly = true;
                eDBUser.ReadOnly = false;
                eDBPass.ReadOnly = false;

                bInitDB.Enabled = true;
                bInitDB.Text = "Initialize SEG Database";
                bInitSEG.Enabled = false;
                bAccounts.Enabled = false;
            }
        }


        public int aeIdx;
        public bool aeEdit;

        public void AccUpdate()
        {
            lbAccounts.Items.Clear();
            for (int i = 0; i < man.users.Count; i++)
                lbAccounts.Items.Add(man.users[i].FullName + " (" + man.users[i].Name + ")");
        }

        public void AccClear()
        {
            aeIdx = -1;
            aeEdit = false;
            lbAccounts.SelectedIndex = -1;

            eAName.Text = "";
            eAUser.Text = "";
            eAPass.Text = "";

            bAAdd.Enabled = true;
            bAEdit.Enabled = false;
            bASave.Enabled = false;
            bADelete.Enabled = false;
        }

        public void AccSelect()
        {
            if(lbAccounts.SelectedIndex>-1)
            {
                aeIdx = lbAccounts.SelectedIndex;
                eAName.Text = man.users[aeIdx].FullName;
                eAUser.Text = man.users[aeIdx].Name;
                eAPass.Text = man.users[aeIdx].Password;
                aeEdit = false;

                bAAdd.Enabled = true;
                bAEdit.Enabled = true;
                bASave.Enabled = false;
                bADelete.Enabled = false;
            }
        }
























        private void winMain_Load(object sender, EventArgs e)
        {
            // We first load the settings file and see if data has been defined already for the toolset
            man.LoadConfig();

            SetupPanels();
            if (!man.configLoaded)
                bDB_Click(sender, e);
            else
                bAccounts_Click(sender, e);
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bDB_Click(object sender, EventArgs e)
        {
            bDB.Font = new Font("Segoe UI Black",9,FontStyle.Bold);
            bAccounts.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            pAccounts.Visible = false;
            pDB.Visible = true;
        }

        private void bAccounts_Click(object sender, EventArgs e)
        {
            bAccounts.Font = new Font("Segoe UI Black", 9, FontStyle.Bold);
            bDB.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            pAccounts.Visible = true;
            pDB.Visible = false;
        }

        private void bInitDB_Click(object sender, EventArgs e)
        {
            if(man.configLoaded)
            {
                man.configLoaded = false;
                try
                {
                    File.Delete(@"seg.install.config");
                }
                catch
                {

                }
                SetupPanels();
            }
            else
            {
                bool isOk = true;

                if (eServer.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid SERVER information");
                    isOk = false;
                }

                if (isOk && eDBUser.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid database User information");
                    isOk = false;
                }

                if (isOk && eDBPass.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid database Password information");
                    isOk = false;
                }

                if (isOk)
                {
                    man.server = eServer.Text.Trim();
                    man.database = "seg";
                    man.username = eDBUser.Text.Trim();
                    man.password = eDBPass.Text.Trim();
                    man.configLoaded = true;

                    if (man.SetupDatabase())
                    {
                        MessageBox.Show("The SEG database has been properly initialized.");
                        man.SaveConfig();
                        SetupPanels();
                    }
                    else
                    {
                        MessageBox.Show("The SEG database could not be properly initialized : " + man.dbError);
                        man.configLoaded = false;
                        SetupPanels();
                    }
                }
            }
        }

        private void bAAdd_Click(object sender, EventArgs e)
        {
            if(!aeEdit)
            {
                // Check if name is empty
                bool isOk = true;

                if (eAName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid Account information");
                    isOk = false;
                }

                if (isOk && eAUser.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid Account information");
                    isOk = false;
                }

                if (isOk && man.users.Exists(x => x.Name == eAUser.Text.Trim()))
                {
                    MessageBox.Show("Please enter a username that is not taken");
                    isOk = false;
                }

                if (isOk && eAPass.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid Account information");
                    isOk = false;
                }

                if (isOk)
                {
                    User u = new User();
                    u.FullName = eAName.Text;
                    u.Name = eAUser.Text;
                    u.Password = eAPass.Text;
                    u.IsAdmin = true;
                    u.IsApprover = true;
                    u.IsExecutor = true;
                    u.IsVerifier = true;
                    u.Active = true;
                    man.AddUser(u);

                    AccUpdate();
                    AccClear();
                }

            }
        }

        private void bAEdit_Click(object sender, EventArgs e)
        {
            if(aeIdx>-1 && !aeEdit)
            {
                aeEdit = true;
                bAAdd.Enabled = false;
                bAEdit.Enabled = false;
                bASave.Enabled = true;
                bADelete.Enabled = true;
            }
        }

        private void lbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbAccounts.SelectedIndex>-1)
            {
                AccSelect();
            }
        }

        private void bASave_Click(object sender, EventArgs e)
        {
            if (aeEdit)
            {
                // Check if name is empty
                bool isOk = true;

                if (eAName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid Account information");
                    isOk = false;
                }

                if (isOk && eAUser.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid Account information");
                    isOk = false;
                }

                if (isOk && man.users.Exists(x=>x.Name == eAUser.Text.Trim()))
                {
                    MessageBox.Show("Please enter a username that is not taken");
                    isOk = false;
                }

                if (isOk && eAPass.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter valid Account information");
                    isOk = false;
                }

                if (isOk)
                {
                    User u = man.users[aeIdx];
                    u.FullName = eAName.Text;
                    u.Name = eAUser.Text;
                    u.Password = eAPass.Text;
                    u.IsAdmin = true;
                    u.IsApprover = true;
                    u.IsExecutor = true;
                    u.IsVerifier = true;
                    u.Active = true;
                    man.UpdateUser(u);

                    AccUpdate();
                    AccClear();
                }

            }
        }

        private void bADelete_Click(object sender, EventArgs e)
        {
            if (aeEdit)
            {
                User u = man.users[aeIdx];
                man.DeleteUser(u);

                AccUpdate();
                AccClear();
            }
        }

        private void bInitSEG_Click(object sender, EventArgs e)
        {
            // We now ask for the direction of the seg configuration file

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Title = "Select the SEG Desktop configuration file"; 
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "SEG Configuration (SEG.Desktop.exe.config)|*.config";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Now we overwrite the file selected with the template one we have
                    StreamReader sr = new StreamReader(@"SetupData\SEG.Desktop.exe.config.template");
                    StreamWriter sw = new StreamWriter(openFileDialog1.FileName);
                    while(!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        // Replace the information contained within
                        if (s.Contains("[%SERVER%]"))
                            s = s.Replace("[%SERVER%]", man.server);
                        if (s.Contains("[%DATABASE%]"))
                            s = s.Replace("[%DATABASE%]", "seg");
                        if (s.Contains("[%USER%]"))
                            s = s.Replace("[%USER%]", man.username);
                        if (s.Contains("[%PASS%]"))
                            s = s.Replace("[%PASS%]", man.password);
                        sw.WriteLine(s);
                    }
                    sr.Close();
                    sw.Close();
                    MessageBox.Show("The SEG Desktop application has been configured properly.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
