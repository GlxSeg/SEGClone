namespace SEG.Tools
{
    partial class winMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(winMain));
            this.pictureBoxMenu = new System.Windows.Forms.PictureBox();
            this.pDB = new System.Windows.Forms.Panel();
            this.bInitSEG = new System.Windows.Forms.Button();
            this.bInitDB = new System.Windows.Forms.Button();
            this.eDB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.eDBPass = new System.Windows.Forms.TextBox();
            this.eDBUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.eServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labGB = new System.Windows.Forms.Label();
            this.labSEG = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labTitle = new System.Windows.Forms.Label();
            this.bDB = new System.Windows.Forms.Button();
            this.bAccounts = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.pAccounts = new System.Windows.Forms.Panel();
            this.bASave = new System.Windows.Forms.Button();
            this.bAEdit = new System.Windows.Forms.Button();
            this.bAAdd = new System.Windows.Forms.Button();
            this.eAPass = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.eAUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bADelete = new System.Windows.Forms.Button();
            this.eAName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lbAccounts = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMenu)).BeginInit();
            this.pDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pAccounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxMenu
            // 
            this.pictureBoxMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxMenu.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMenu.Image")));
            this.pictureBoxMenu.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxMenu.Name = "pictureBoxMenu";
            this.pictureBoxMenu.Size = new System.Drawing.Size(227, 379);
            this.pictureBoxMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMenu.TabIndex = 0;
            this.pictureBoxMenu.TabStop = false;
            // 
            // pDB
            // 
            this.pDB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pDB.Controls.Add(this.bInitSEG);
            this.pDB.Controls.Add(this.bInitDB);
            this.pDB.Controls.Add(this.eDB);
            this.pDB.Controls.Add(this.label5);
            this.pDB.Controls.Add(this.label4);
            this.pDB.Controls.Add(this.eDBPass);
            this.pDB.Controls.Add(this.eDBUser);
            this.pDB.Controls.Add(this.label3);
            this.pDB.Controls.Add(this.eServer);
            this.pDB.Controls.Add(this.label2);
            this.pDB.Controls.Add(this.label1);
            this.pDB.Location = new System.Drawing.Point(245, 12);
            this.pDB.Name = "pDB";
            this.pDB.Size = new System.Drawing.Size(527, 437);
            this.pDB.TabIndex = 1;
            // 
            // bInitSEG
            // 
            this.bInitSEG.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bInitSEG.Location = new System.Drawing.Point(19, 169);
            this.bInitSEG.Name = "bInitSEG";
            this.bInitSEG.Size = new System.Drawing.Size(242, 28);
            this.bInitSEG.TabIndex = 10;
            this.bInitSEG.Text = "Initialize SEG Desktop";
            this.bInitSEG.UseVisualStyleBackColor = true;
            this.bInitSEG.Click += new System.EventHandler(this.bInitSEG_Click);
            // 
            // bInitDB
            // 
            this.bInitDB.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bInitDB.Location = new System.Drawing.Point(19, 135);
            this.bInitDB.Name = "bInitDB";
            this.bInitDB.Size = new System.Drawing.Size(242, 28);
            this.bInitDB.TabIndex = 9;
            this.bInitDB.Text = "Initialize SEG Database";
            this.bInitDB.UseVisualStyleBackColor = true;
            this.bInitDB.Click += new System.EventHandler(this.bInitDB_Click);
            // 
            // eDB
            // 
            this.eDB.Enabled = false;
            this.eDB.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eDB.Location = new System.Drawing.Point(369, 57);
            this.eDB.Name = "eDB";
            this.eDB.Size = new System.Drawing.Size(140, 25);
            this.eDB.TabIndex = 8;
            this.eDB.Text = "seg";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(297, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Database :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(292, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password :";
            // 
            // eDBPass
            // 
            this.eDBPass.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eDBPass.Location = new System.Drawing.Point(369, 88);
            this.eDBPass.Name = "eDBPass";
            this.eDBPass.Size = new System.Drawing.Size(140, 25);
            this.eDBPass.TabIndex = 5;
            // 
            // eDBUser
            // 
            this.eDBUser.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eDBUser.Location = new System.Drawing.Point(121, 88);
            this.eDBUser.Name = "eDBUser";
            this.eDBUser.Size = new System.Drawing.Size(140, 25);
            this.eDBUser.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Username (sa) :";
            // 
            // eServer
            // 
            this.eServer.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eServer.Location = new System.Drawing.Point(70, 57);
            this.eServer.Name = "eServer";
            this.eServer.Size = new System.Drawing.Size(191, 25);
            this.eServer.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "SEG Database Configuration and Setup .-";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(12, 397);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(227, 52);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // labGB
            // 
            this.labGB.AutoSize = true;
            this.labGB.BackColor = System.Drawing.Color.Transparent;
            this.labGB.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labGB.ForeColor = System.Drawing.Color.White;
            this.labGB.Location = new System.Drawing.Point(25, 30);
            this.labGB.Name = "labGB";
            this.labGB.Size = new System.Drawing.Size(122, 40);
            this.labGB.TabIndex = 3;
            this.labGB.Text = "GLBSYS";
            this.labGB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labSEG
            // 
            this.labSEG.AutoSize = true;
            this.labSEG.BackColor = System.Drawing.Color.Transparent;
            this.labSEG.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSEG.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(85)))), ((int)(((byte)(110)))));
            this.labSEG.Location = new System.Drawing.Point(140, 30);
            this.labSEG.Name = "labSEG";
            this.labSEG.Size = new System.Drawing.Size(69, 40);
            this.labSEG.TabIndex = 4;
            this.labSEG.Text = "SEG";
            this.labSEG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(27, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 2);
            this.panel2.TabIndex = 5;
            // 
            // labTitle
            // 
            this.labTitle.AutoSize = true;
            this.labTitle.BackColor = System.Drawing.Color.Transparent;
            this.labTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTitle.ForeColor = System.Drawing.Color.Black;
            this.labTitle.Location = new System.Drawing.Point(30, 78);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(159, 21);
            this.labTitle.TabIndex = 6;
            this.labTitle.Text = "Management Toolbox";
            this.labTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bDB
            // 
            this.bDB.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bDB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bDB.Location = new System.Drawing.Point(27, 129);
            this.bDB.Name = "bDB";
            this.bDB.Size = new System.Drawing.Size(200, 30);
            this.bDB.TabIndex = 7;
            this.bDB.Text = "Database";
            this.bDB.UseVisualStyleBackColor = true;
            this.bDB.Click += new System.EventHandler(this.bDB_Click);
            // 
            // bAccounts
            // 
            this.bAccounts.Location = new System.Drawing.Point(27, 165);
            this.bAccounts.Name = "bAccounts";
            this.bAccounts.Size = new System.Drawing.Size(200, 30);
            this.bAccounts.TabIndex = 8;
            this.bAccounts.Text = "User Accounts";
            this.bAccounts.UseVisualStyleBackColor = true;
            this.bAccounts.Click += new System.EventHandler(this.bAccounts_Click);
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(27, 349);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(200, 30);
            this.bExit.TabIndex = 9;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // pAccounts
            // 
            this.pAccounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pAccounts.Controls.Add(this.bASave);
            this.pAccounts.Controls.Add(this.bAEdit);
            this.pAccounts.Controls.Add(this.bAAdd);
            this.pAccounts.Controls.Add(this.eAPass);
            this.pAccounts.Controls.Add(this.label8);
            this.pAccounts.Controls.Add(this.eAUser);
            this.pAccounts.Controls.Add(this.label7);
            this.pAccounts.Controls.Add(this.bADelete);
            this.pAccounts.Controls.Add(this.eAName);
            this.pAccounts.Controls.Add(this.label6);
            this.pAccounts.Controls.Add(this.lbAccounts);
            this.pAccounts.Controls.Add(this.label10);
            this.pAccounts.Location = new System.Drawing.Point(245, 12);
            this.pAccounts.Name = "pAccounts";
            this.pAccounts.Size = new System.Drawing.Size(527, 437);
            this.pAccounts.TabIndex = 10;
            // 
            // bASave
            // 
            this.bASave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bASave.Location = new System.Drawing.Point(267, 357);
            this.bASave.Name = "bASave";
            this.bASave.Size = new System.Drawing.Size(242, 28);
            this.bASave.TabIndex = 19;
            this.bASave.Text = "Save Account";
            this.bASave.UseVisualStyleBackColor = true;
            this.bASave.Click += new System.EventHandler(this.bASave_Click);
            // 
            // bAEdit
            // 
            this.bAEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bAEdit.Location = new System.Drawing.Point(267, 323);
            this.bAEdit.Name = "bAEdit";
            this.bAEdit.Size = new System.Drawing.Size(242, 28);
            this.bAEdit.TabIndex = 18;
            this.bAEdit.Text = "Edit Account";
            this.bAEdit.UseVisualStyleBackColor = true;
            this.bAEdit.Click += new System.EventHandler(this.bAEdit_Click);
            // 
            // bAAdd
            // 
            this.bAAdd.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bAAdd.Location = new System.Drawing.Point(267, 288);
            this.bAAdd.Name = "bAAdd";
            this.bAAdd.Size = new System.Drawing.Size(242, 28);
            this.bAAdd.TabIndex = 17;
            this.bAAdd.Text = "Add Account";
            this.bAAdd.UseVisualStyleBackColor = true;
            this.bAAdd.Click += new System.EventHandler(this.bAAdd_Click);
            // 
            // eAPass
            // 
            this.eAPass.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eAPass.Location = new System.Drawing.Point(267, 185);
            this.eAPass.Name = "eAPass";
            this.eAPass.Size = new System.Drawing.Size(242, 25);
            this.eAPass.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(267, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Password:";
            // 
            // eAUser
            // 
            this.eAUser.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eAUser.Location = new System.Drawing.Point(267, 128);
            this.eAUser.Name = "eAUser";
            this.eAUser.Size = new System.Drawing.Size(242, 25);
            this.eAUser.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(267, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Username:";
            // 
            // bADelete
            // 
            this.bADelete.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bADelete.Location = new System.Drawing.Point(267, 391);
            this.bADelete.Name = "bADelete";
            this.bADelete.Size = new System.Drawing.Size(242, 28);
            this.bADelete.TabIndex = 12;
            this.bADelete.Text = "Delete Account";
            this.bADelete.UseVisualStyleBackColor = true;
            this.bADelete.Click += new System.EventHandler(this.bADelete_Click);
            // 
            // eAName
            // 
            this.eAName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eAName.Location = new System.Drawing.Point(267, 71);
            this.eAName.Name = "eAName";
            this.eAName.Size = new System.Drawing.Size(242, 25);
            this.eAName.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(267, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Full Name:";
            // 
            // lbAccounts
            // 
            this.lbAccounts.FormattingEnabled = true;
            this.lbAccounts.Location = new System.Drawing.Point(19, 51);
            this.lbAccounts.Name = "lbAccounts";
            this.lbAccounts.Size = new System.Drawing.Size(242, 368);
            this.lbAccounts.TabIndex = 1;
            this.lbAccounts.SelectedIndexChanged += new System.EventHandler(this.lbAccounts_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(135, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "SEG User Accounts .-";
            // 
            // winMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bAccounts);
            this.Controls.Add(this.bDB);
            this.Controls.Add(this.labTitle);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.labSEG);
            this.Controls.Add(this.labGB);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBoxMenu);
            this.Controls.Add(this.pDB);
            this.Controls.Add(this.pAccounts);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "winMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SEG.Tools v1.0b";
            this.Load += new System.EventHandler(this.winMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMenu)).EndInit();
            this.pDB.ResumeLayout(false);
            this.pDB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pAccounts.ResumeLayout(false);
            this.pAccounts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMenu;
        private System.Windows.Forms.Panel pDB;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labGB;
        private System.Windows.Forms.Label labSEG;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labTitle;
        private System.Windows.Forms.Button bDB;
        private System.Windows.Forms.Button bAccounts;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.TextBox eDB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox eDBPass;
        private System.Windows.Forms.TextBox eDBUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox eServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bInitDB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pAccounts;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox eAPass;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox eAUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bADelete;
        private System.Windows.Forms.TextBox eAName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lbAccounts;
        private System.Windows.Forms.Button bASave;
        private System.Windows.Forms.Button bAEdit;
        private System.Windows.Forms.Button bAAdd;
        private System.Windows.Forms.Button bInitSEG;
    }
}

