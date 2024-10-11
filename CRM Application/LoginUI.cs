using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_Application
{
    public partial class LoginUI : Form
    {
        public static String LoginUserName = "";
        public LoginUI()
        {
            InitializeComponent();
        }
        private void Login_A()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "select * from Tbl_LoginUserList where UserName='" + txtusername.Text + "' and Password=CONVERT(NVARCHAR(32),HashBytes('MD5','" + txtpassword.Text + "'),2)";

                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                if (MyReader2.HasRows)

                {

                    //txtusername.Text = txtusername.Text.ToUpper();
                    LoginUserName = txtusername.Text;
                    lblmsg.Text = "Login success";
                    txtusername.Text = "";
                    txtpassword.Text = "";
                    lblmsg.ForeColor = Color.Green;
                    EaseyAccess EaseyAccess = new EaseyAccess();
                    EaseyAccess.Show();
                    this.Hide();
                  
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please enter valid login credential for login";
                    txtpassword.Text = "";
                    txtusername.Text = "";

                }
                DataBaseConnection.Lconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to Exit form Stock management System ?",
                               "Stock management System Application close confirmation",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
            else
            {

            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Login_A();
            
        }

     
        private void Password_Change_Run()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "select * from Tbl_LoginUserList where UserName='" + txtusername.Text + "' and Password=CONVERT(NVARCHAR(32),HashBytes('MD5','" + txtpassword.Text + "'),2)";

                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                if (MyReader2.HasRows)
                {
                    TabControl1.SelectedTab = Reset_password;
                    txt_UserName1.Text = txtusername.Text;
                    txt_old_password.Text = txtpassword.Text;

                    lblmsg.Text = "now you can chage your login password";
                    lblmsg.ForeColor = Color.Green;
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please enter valid login credential for login";
                    txtpassword.Text = "";
                    txtusername.Text = "";

                }
                DataBaseConnection.Lconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    
        private void Password_Change()
        {
            try
            {
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Update Tbl_LoginUserList set Password=CONVERT(NVARCHAR(32),HashBytes('MD5','" + txt_confirm_new_Password.Text + "'),2),ChangeDate= '" + EnterDate + "' ,  ChangeTime= '" + EnterTime + "' WHERE UserName ='" + this.txt_UserName1.Text + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();

                txtpassword.Text = "";
                txtusername.Text = "";
                txt_confirm_new_Password.Text = "";
                txt_new_password.Text = "";
                txt_old_password.Text = "";
                txt_UserName1.Text = "";

                TabControl1.SelectedTab = Login_Page;
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Password changed";

            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Password changed Error" + ex.ToString();
                MessageBox.Show(lblmsg.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (txtusername.Text == "")
                {

                }

                else
                {
                    txtpassword.Focus();
                }
            }
        }

        private void txtpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (txtusername.Text == "")
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please enter user details for login.";
                    txtusername.Focus();
                }
                else if (txtpassword.Text == "")
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please enter user details for login.";
                    txtpassword.Focus();
                }
                else
                {

                    {
                        Login_A();
                    }
                }
            }
        }

        private void LoginUI_Load(object sender, EventArgs e)
        {
            txtusername.Focus();
            lblOrganizationName.Text = Properties.Settings.Default.NameOfOrganization;
            lblOrganizationAddress.Text = Properties.Settings.Default.OrganizationAddress;
        }

        private void btn_exit_Click_1(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to logout form CRM System ?",
                             "CRM logout confirmation messages",
                              MessageBoxButtons.OKCancel,
                              MessageBoxIcon.Question) == DialogResult.OK)
                Application.Exit();

            else
            {

            }

        }

        private void btn_reset_Click_1(object sender, EventArgs e)
        {
            Password_Change_Run();
        }

        private void lbl_password_reset_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (txt_new_password.Text == "")
            {
                txt_new_password.Focus();
            }
            else if (txt_confirm_new_Password.Text == "")
            {
                txt_confirm_new_Password.Focus();
            }

            else if (txt_new_password.Text == txt_confirm_new_Password.Text)
            {
                Password_Change();
            }

            else
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "New password not match.please recheck";
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

