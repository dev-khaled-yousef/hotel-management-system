﻿using Guna.UI2.WinForms;
using Hotel.GlobalClasses;
using Hotel.MainMenu;
using Hotel_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel.Login
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void ValidatingOfTextBoxes(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((Guna2TextBox)sender).Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(((Guna2TextBox)sender), "This field is required!");
            }
            else
            {
                errorProvider1.SetError(((Guna2TextBox)sender), null);
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            ((Guna2TextBox)sender).BorderColor = Color.FromArgb(26, 83, 92);
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            ((Guna2TextBox)sender).BorderColor = Color.Silver;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string HashedPassword = clsGlobal.ComputeHash(txtPassword.Text.Trim());

            if (!clsUser.DoesUserExist(txtUsername.Text.Trim(), HashedPassword))
            {
                txtUsername.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            clsUser User = clsUser.FindBy(txtUsername.Text.Trim(), HashedPassword);

            if (User == null)
            {
                txtUsername.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (chkRememberMe.Checked)
            {
                //store username and password
                clsGlobal.RememberUsernameAndPassword
                    (txtUsername.Text.Trim(), clsGlobal.Encrypt(txtPassword.Text.Trim()));
            }
            else
            {
                //remove username and password
                clsGlobal.RemoveStoredCredential();
            }

            if (!User.IsActive)
            {
                txtUsername.Focus();

                MessageBox.Show("Your account is not Active, Contact Admin.",
                    "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsGlobal.CurrentUser = User;
            this.Hide();
            frmMainMenu OpenMainMenu = new frmMainMenu(this);
            OpenMainMenu.ShowDialog();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUsername.Text = UserName;
                txtPassword.Text = clsGlobal.Decrypt(Password);
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblSignUp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet!");
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet!");
        }

        private void llOpenMyProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify the URL you want to open
            string url = "https://github.com/dev-khaled-yousef";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(url);
        }
    }
}
