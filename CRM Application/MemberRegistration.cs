using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_Application
{
    public partial class MemberRegistration : Form
    {
        public MemberRegistration()
        {
            InitializeComponent();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("are you sure to confirm this Member registration ? insert details was correct?", "Member Registration confirm ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

                ManualIDStringCheck();
            
            else
            {

            }
        }

        private void ManualIDStringCheck()
        {
            if (checkBox1.Checked == true)
            {
                MemberIDCreate();
            }
            else
            {                
                string fmt = "000000.##";
                decimal Result_With_Zero;
                Result_With_Zero = Convert.ToDecimal(txtmanualid.Text);
                txtmemberID.Text = Result_With_Zero.ToString(Properties.Settings.Default.MemberHashIDFrmt + fmt);
                checkDublicateID();
            }
        }
        private void checkDublicateID()
        {
            try
            {
                
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Select * from Tbl_MemberIDList where IDNumber ='" + txtmanualid.Text + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                MyReader2.Read();
                if (MyReader2.HasRows)
                {                    
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "System Already using this ID Number (" + txtmemberID.Text + ")";

                    if (MessageBox.Show("Ops....that member ID already using on the system . Do you need to see view record?", "Member Registration Member View ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        ScanMemberListActiveRecord();                    
                    else
                    {
                        MemberIDCreate();
                    }
                   
                }
                else
                {
                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "New ID....(" + txtmemberID.Text + ")";
                    MemberIDCreate();
                }
                DataBaseConnection.Lconn.Close();
            }
            catch (Exception ex)
            {
                logmessage();
            }

        }

        string MemberID;
        private void MemberIDCreate()
        {
            try
            {
                if (checkBox1.Checked == true)
                {
                    int a;
                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select Max(IDNumber) from Tbl_MemberIDList";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    while (MyReader2.Read())
                    {
                        string val = MyReader2[0].ToString();
                        if (val == "")
                        {
                            MemberID = "1";

                        }
                        else
                        {
                            a = Convert.ToInt32(MyReader2[0].ToString());
                            a = a + 1;
                            MemberID = a.ToString();

                        }
                        checkDublicate();

                    }
                }
                else
                {
                    MemberID = Convert.ToString(txtmanualid.Text);
                    checkDublicate();
                    //   MemberHashIDFrmtConvert();
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Member Registratin ID Create faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
        private void checkDublicate()
        {
            try
            {


                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Select * from Tbl_MemberIDList where IDNumber ='" + MemberID + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                MyReader2.Read();
                if (MyReader2.HasRows)
                {
                    //
                    string fmt = "000000.##";
                    decimal Result_With_Zero;
                    Result_With_Zero = Convert.ToDecimal(MemberID);
                    txtmemberID.Text = Result_With_Zero.ToString(Properties.Settings.Default.MemberHashIDFrmt + fmt);
                    //message display
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "System Already using this ID Number (" + txtmemberID.Text + ")";

                }
                else
                {
                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "Saving Data....(" + txtmemberID.Text + ")";
                    MemberHashIDFrmtConvert();
                }
                DataBaseConnection.Lconn.Close();
            }
            catch (Exception ex)
            {
                logmessage();
            }

        }
        private void MemberHashIDFrmtConvert()
        {
            try
            {
                string fmt = "000000.##";
                decimal Result_With_Zero;
                Result_With_Zero = Convert.ToDecimal(MemberID);
                txtmemberID.Text = Result_With_Zero.ToString(Properties.Settings.Default.MemberHashIDFrmt + fmt);
                MemberIDRecord();
            }
            catch (Exception)
            {

            }
        }


        private void MemberIDRecord()
        {
            try
            {
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "insert into Tbl_MemberIDList(IDNumber,MemberID,MemberName,EnterDate,EnterTime,EnterBy) values('" + MemberID + "', '" + this.txtmemberID.Text + "','" + this.txtname.Text + "', '" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "New Members ID Successfully added(" + txtmemberID.Text + ")";
                MemberDataRecord();
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "New Members adding faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
        private void MemberDataRecord()
        {
            try
            {
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "insert into Tbl_MembersList(ID,SystemID,Batch,Name,Mobile,Email,NIC,ManageBy,Address,EnterDate,EnterTime,EnterBy) values('" + MemberID + "', '" + this.txtmemberID.Text + "','" + this.txtbatch.Text + "', '" + this.txtname.Text + "', '" + this.txtmobile.Text + "', '" + this.txtemail.Text + "', '" + this.txtnic.Text + "', '" + this.cmbsalesrepslist.Text + "','" + this.txtaddress.Text + "', '" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                MemberListAllRecordInsert();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "New Members Successfully added(" + txtmemberID.Text + ")";
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "New Members adding faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
        private void MemberListAllRecordInsert()
        {
            try
            {
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "insert into Tbl_MembersListAllRecords(ID,SystemID,Batch,Name,Mobile,Email,NIC,ManageBy,Address,EnterDate,EnterTime,EnterBy) values('" + MemberID + "', '" + this.txtmemberID.Text + "','" + this.txtbatch.Text + "', '" + this.txtname.Text + "', '" + this.txtmobile.Text + "', '" + this.txtemail.Text + "', '" + this.txtnic.Text + "','" + this.cmbsalesrepslist.Text + "', '" + this.txtaddress.Text + "', '" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "New Members Successfully added(" + txtmemberID.Text + ")";
                clearAll();
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "New Members adding faild All record " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void MemberRegistration_Load(object sender, EventArgs e)
        {
            lbluser.Text = EaseyAccess.LoginUserName;
            manualidpanel.Visible = false;
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearAll();
            txtmemberID.Text = "";
        }
        private void clearAll()
        {

            txtaddress.Text = "";
            txtbatch.Text = "";
            txtemail.Text = "";
            txtmobile.Text = "";
            txtname.Text = "";
            txtnic.Text = "";
            cmbsalesrepslist.Text = "";
            txtmanualid.Text = "";
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure to confirm this Member update ? insert details was correct?", "Member Registration Update ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                UpdateMemberDetails();
            else
            {

            }
            
        }
        private void UpdateMemberDetails()
        {
            try
            {
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                string enteruser = lbluser.Text;
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Update Tbl_MembersList set ManageBy='" + this.cmbsalesrepslist.Text + "', Batch='" + this.txtbatch.Text + "',Name='" + this.txtname.Text + "',Mobile='" + this.txtmobile.Text + "',Email='" + this.txtemail.Text + "',NIC='" + this.txtnic.Text + "',Address='" + this.txtaddress.Text + "',EnterDate='" + EnterDate + "',EnterTime='" + EnterTime + "',EnterBy='" + this.lbluser.Text + "' where  SystemID ='" + this.txtmemberID.Text + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                MemberListAllRecordInsert();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Member Details Successfully updated Member ID (" + txtmemberID.Text + ")";
                SaveToolStripMenuItem.Enabled = true;
            }
            catch (Exception Ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Member Details Update Error /" + Ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on update ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }

        }

        private void txtmemberID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                ScanMemberListActiveRecord();
            }
            else
            {

            }
        }

        private void ScanMemberListActiveRecord()
        {
            try {
                if (txtmemberID.Text == string.Empty)
                {

                }
                else
                {

                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select * from Tbl_MembersList where SystemID ='" + txtmemberID.Text.ToString() + "'";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    MyReader2.Read();
                    if (MyReader2.HasRows)
                    {
                        txtbatch.Text = MyReader2["Batch"].ToString();
                        txtemail.Text = MyReader2["Email"].ToString();
                        txtmobile.Text = MyReader2["Mobile"].ToString();
                        txtname.Text = MyReader2["Name"].ToString();
                        txtnic.Text = MyReader2["NIC"].ToString();
                        txtaddress.Text = MyReader2["Address"].ToString();
                        cmbsalesrepslist.Text = MyReader2["ManageBy"].ToString();

                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Scan Member ID (" + txtmemberID.Text + ")";

                    }
                    else
                    {
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Please Check Member ID (" + txtmemberID.Text + ")";
                    }
                    DataBaseConnection.Lconn.Close();
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Member Scaning Error " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on Member Scan Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }



        private void SalesRepSelection()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                SqlCommand sc = new SqlCommand("Select * from Tbl_SalesRepslist", DataBaseConnection.Lconn);
                SqlDataReader rdr;
                rdr = sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Load(rdr);
                cmbsalesrepslist.ValueMember = "Name";
                cmbsalesrepslist.DataSource = dt;
                DataBaseConnection.Lconn.Close();
            }
            catch (Exception ex)
            {
                lblmsg.Text = "Sales rep selection error " + ex.ToString();
                lblmsg.ForeColor = Color.Red;
                MessageBox.Show(lblmsg.Text, "Exception on Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void cmbsalesrepslist_DrawItem(object sender, DrawItemEventArgs e)
        {
            SalesRepSelection();
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

        private void cmbsalesrepslist_DropDown(object sender, EventArgs e)
        {
            SalesRepSelection();
        }

        private void txtbatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtname.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtmobile.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtmobile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtemail.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtemail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtnic.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtnic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtaddress.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkboxvalidate();
        }
        private void checkboxvalidate()
        {
            if (checkBox1.Checked == true)
            {
                txtmemberID.Enabled = true;
                txtmanualid.Enabled = false;
                manualidpanel.Visible = false;
            }
            else
            {
                txtmemberID.Enabled = false;
                txtmanualid.Enabled = true;
                manualidpanel.Visible = true;
            }
        }


        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
                        
        }
//message log
        private void logmessage()
        {
            using (StreamWriter w = File.AppendText("log\\MemberRegExceptionLog.txt"))
            {
                Log(lblmsg.Text, w);
            }
            using (StreamReader r = File.OpenText("log\\MemberRegExceptionLog.txt"))
            {
                DumpLog(r);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("*--------------END-----------------*");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

    }
}
