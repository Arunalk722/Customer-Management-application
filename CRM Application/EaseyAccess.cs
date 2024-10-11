using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_Application
{
    public partial class EaseyAccess : Form
    {
        public EaseyAccess()
        {
            InitializeComponent();
        }
        public static String LoginUserName = "";
        private void EaseyAccess_Load(object sender, EventArgs e)
        {
           lbluser.Text =  LoginUI.LoginUserName;
            TreeLoad();
            ScanMemberListActiveRecord();


         
        }

        private void ScanMemberListActiveRecord()
        {
            try
            {
               
               

                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select * from Tbl_LoginUserList where UserName ='" + lbluser.Text.ToString() + "'";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    MyReader2.Read();
                    if (MyReader2.HasRows)
                    {
                    LoginUserName = MyReader2["UserName"].ToString();
                        lbluser.Text = MyReader2["UserName"].ToString();
                    }
                    DataBaseConnection.Lconn.Close();
                
            }
            catch (Exception ex)
            {
            }
        }

        private void TreeLoad()
        {
           
                try
                {
                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    SqlCommand sc = new SqlCommand("Select * from Tbl_LoginUserRights where UserName='"+ lbluser.Text +"'", DataBaseConnection.Lconn);
                    SqlDataReader rdr;
                    rdr = sc.ExecuteReader();
                while (rdr.Read())
                {
                    TreeNode node = new TreeNode((string)rdr["FormName"]);
                 // node.Nodes.Add((string)rdr["FormName"]);
                    treeallowframlist.Nodes.Add(node);
                }           
                    DataBaseConnection.Lconn.Close();
                }
                catch (Exception ex)
                {
                    lblmsg.Text = "Sales rep selection error " + ex.ToString();
                    lblmsg.ForeColor = Color.Red;
                    MessageBox.Show(lblmsg.Text, "Exception on Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

       

        private void memberRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberRegistration memberRegistration = new MemberRegistration();
            memberRegistration.Show();
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        string SelectedFormName;
        private void treeallowframlist_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = treeallowframlist.SelectedNode;
            SelectedFormName = node.Text;         
            CheckAllowForm();

        }
        string formname;
        private void CheckAllowForm()
        {
            try
            {
                if (SelectedFormName == string.Empty)
                {

                }
                else
                {
                    int a = 1;
                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select * from Tbl_LoginUserRights where UserName ='" + lbluser.Text.ToString() + "' and FormName='"+ SelectedFormName + "' ";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    MyReader2.Read();
                    if (MyReader2.HasRows)
                    {                      
                        formname = MyReader2["FormName"].ToString();
                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "You have authorized to access " + formname;
                        openform();
                    }
                    else
                    {
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "You have not authorized to access (" + SelectedFormName + ")";
                    }
                    DataBaseConnection.Lconn.Close();
                    SelectedFormName = "";
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Form Access Scaning Error " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openform()
        {
           if(formname == string.Empty)
            {

            }
            else if(formname == "LoginUI")
            {
                LoginUI loginUI = new LoginUI();
                loginUI.Show();
            }
            else if (formname == "MemberData")
            {
                MemberData memberData = new MemberData();
                memberData.Show();
            }
            else if (formname == "MemberRegistration")
            {
                MemberRegistration memberRegistration = new MemberRegistration();
                memberRegistration.Show();
            }
            else if (formname == "PaymentNotifications")
            {
                PaymentNotifications frm = new PaymentNotifications();
                frm.Show();
            }
            else if (formname == "PaymentRegistration")
            {
                PaymentRegistration frm2 = new PaymentRegistration();
                frm2.Show();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        { 
            
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
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
    }
}
