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
    public partial class PaymentNotifications : Form
    {
        public PaymentNotifications()
        {
            InitializeComponent();
        }
        //message log
        private void logmessage()
        {
            using (StreamWriter w = File.AppendText("log\\PaymentRecord.txt"))
            {
                Log(lblmsg.Text, w);
            }
            using (StreamReader r = File.OpenText("log\\PaymentRecord.txt"))
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
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaymentNotificationData();
        }
        private void PaymentNotificationData()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                string Query = "Select * from Tbl_PaymentTransactions WHERE NotificationDate  BETWEEN '" + date_start.Text + "' and '" + date_end.Text + "'and NotiflyStatus='1'";
                SqlConnection MyConn2 = new SqlConnection(comm.Connection.ConnectionString);
                SqlCommand MyCommand2 = new SqlCommand(Query, MyConn2);
                SqlDataReader MyReader2;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                if (MyReader2.HasRows)
                {
                    SqlConnection connection = new SqlConnection(comm.Connection.ConnectionString);
                    SqlDataAdapter dataadapter = new SqlDataAdapter(Query, connection);
                    DataSet ds = new DataSet();
                    connection.Open();
                    dataadapter.Fill(ds, "Tbl_PaymentTransactions");
                    connection.Close();
                    dtgrpt.DataSource = ds;
                    dtgrpt.DataMember = "Tbl_PaymentTransactions";

                    DataBaseConnection.Lconn.Close();

                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "Record are finded.start date" + date_start.Text + " end date" + date_end.Text ;
                    tabview.SelectedTab = tabrpt;

                }
                else
                {
                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "Thare no record in this input";
                    dtgrpt.DataSource = null;
                }
            }

            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Member Scaning error ." + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void dtgrpt_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txttransactionid.Text = dtgrpt.SelectedRows[0].Cells[0].Value + string.Empty;
            lblmemberid.Text = dtgrpt.SelectedRows[0].Cells[2].Value + string.Empty;
            ScanMemberListActiveRecord();
            tabview.SelectedTab = tabPage1;
        }
        private void ScanMemberListActiveRecord()
        {
            try
            {
                {

                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select * from Tbl_PaymentTransactions where ID ='" + txttransactionid.Text.ToString() + "'";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    MyReader2.Read();
                    if (MyReader2.HasRows)
                    {
                        lblsubid.Text = MyReader2["SubID"].ToString();
                        txtname.Text = MyReader2["Name"].ToString();                                             
                        txtproductname.Text = MyReader2["ProductName"].ToString();
                        txtproductammount.Text = MyReader2["ProductAmount"].ToString();
                        txtdownpayment.Text = MyReader2["DownPayment"].ToString();                        
                        txtduration.Text = MyReader2["CurrentDuration"].ToString();
                        
                        txtsettlmentresetdate.Text = MyReader2["DueDate"].ToString();
                        txtrental.Text = MyReader2["Rental"].ToString();
                        txtnotification.Text = MyReader2["NotificationDate"].ToString();

                        /*txtemail.Text = MyReader2["Name"].ToString();
                        txtnic.Text = MyReader2["NIC"].ToString();
                        txtmobile.Text = MyReader2["Mobile"].ToString(); 
                        txtbatch.Text = MyReader2["Batch"].ToString();
                        txtpaybleammount.Text = MyReader2["Address"].ToString();*/
                        MemberDataScan();

                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Scan Payment Transactions ID (" + txttransactionid.Text + ")";

                    }
                    else
                    {
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Thare no record ";
                    }
                    DataBaseConnection.Lconn.Close();
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Payment Transactions scan Error " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception Scan Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void MemberDataScan()
        {
            try
            {
                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select * from Tbl_MembersList where SystemID ='" + lblmemberid.Text.ToString() + "'";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    MyReader2.Read();
                    if (MyReader2.HasRows)
                    {
                        txtemail.Text = MyReader2["Name"].ToString();
                        txtnic.Text = MyReader2["NIC"].ToString();
                        txtmobile.Text = MyReader2["Mobile"].ToString(); 
                        txtbatch.Text = MyReader2["Batch"].ToString();
                        txtaddress.Text = MyReader2["Address"].ToString();
                        txtSalesRap.Text = MyReader2["ManageBy"].ToString();

                        PaybleAmountScan();

                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Scan Member ID (" + txttransactionid.Text + ")";

                    }
                    else
                    {
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Thare no record ";
                    }
                    DataBaseConnection.Lconn.Close();
                
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Member ID scan Error " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception Scan Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void PaybleAmountScan()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Select * from Tbl_PaymentRegistrationDetails where ID ='" + lblsubid.Text.ToString() + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                MyReader2.Read();
                if (MyReader2.HasRows)
                {
                    txtpaybleammount.Text = MyReader2["LeftPayment"].ToString();
                    txtpayedamount.Text = MyReader2["PayedAmount"].ToString();
                    txtleftamount.Text = MyReader2["LeftPayment"].ToString();
                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "Scan Member ID (" + txttransactionid.Text + ")";
                    txttransactionsid.Focus();
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Thare no record ";
                }
                DataBaseConnection.Lconn.Close();

            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Member ID scan Error " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception Scan Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }


        private void PaymentNotifications_Load(object sender, EventArgs e)
        {
            panel5.Enabled = false;
            lbluser.Text = EaseyAccess.LoginUserName;
            CheckAllowForm();
        }
        string formname;
        private void CheckAllowForm()
        {
            try
            {
                    int a = 1;
                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select * from Tbl_LoginUserRights where UserName ='" + lbluser.Text.ToString() + "' and FormName='PaymentNotificationsRecord' and FormStatus='1' ";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    MyReader2.Read();
                    if (MyReader2.HasRows)
                    {
                        formname = MyReader2["FormName"].ToString();
                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "You have authorized to access " + formname;
                        panel5.Enabled = true;
                    }
                    else
                    {
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "You have not authorized to access (" + formname + ")";
                    }
                    DataBaseConnection.Lconn.Close();                    
                
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Form Access Scaning Error " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void clearText()
        {
            txttransactionid.Text = "";
            lblsubid.Text = "";
            lblmemberid.Text = "";
            txtbatch.Text = "";
            txtname.Text = "";
            txtmobile.Text = "";
            txtemail.Text = "";
            txtnic.Text = "";
            txtSalesRap.Text = "";
            txtaddress.Text = "";
            txtproductname.Text = "";
            txtproductammount.Text = "";
            txtdownpayment.Text = "";
            txtduration.Text = "";
            txtsettlmentresetdate.Text = "";
            txtnotification.Text = "";
            txtpaybleammount.Text = "";
            txtleftamount.Text = "";
            txtrental.Text = "";
            txtpayedamount.Text = "";
            txttransactionsid.Text = "";
            txtpayamount.Text = "";
            cmbtype.Text = "";
            txtdurationleftamount.Text = "";
            txtremark.Text = "";
            

        }

        private void btnsave_Click(object sender, EventArgs e)
        {

          if (MessageBox.Show( "are you sure to confirm this payment ? insert details was correct?", "Payment update confirm ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PaymentTransactionslistInsert();           
            else
            {
                
            }

            
        }
        decimal rental, payedamount, leftAmount, currentpayedamount, tobepayingfullamount, fullpayedamount, updatePayedAmount;

      

        private void txttransactionsid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtpayamount.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void cmbtype_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtdurationleftamount.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtremark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                btnsave.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtpayamount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateRentalValueOnly();
                cmbtype.Focus();
                e.Handled = true;
            }
            else
            {

            }
        }

        private void txtdurationleftamount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
                txtremark.Focus();
                e.Handled = true;
            }
            else
            {

            }

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearText();
            tabview.SelectedTab = tabselections;
            dtgrpt.DataSource = null;
            dtgrpt.Refresh();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
            
        }
        private void CalUpdatebleValueWithUpdate()
        {
            try
            {
                UpdateRentalValueOnly();
                PaymentTransactionslistInsert();
            }
            catch (Exception)
            {

            }
        }
        private void UpdateRentalValueOnly()
        {
            try
            {
                rental = Convert.ToDecimal(txtrental.Text);
                payedamount = Convert.ToDecimal(txtpayamount.Text);
                leftAmount = (rental - payedamount);
                txtdurationleftamount.Text = Convert.ToString(leftAmount);
                //  MessageBox.Show("left amount id" + txtdurationleftamount.Text);

                currentpayedamount = Convert.ToDecimal(txtpaybleammount.Text);
                tobepayingfullamount = (payedamount + currentpayedamount);
                //   MessageBox.Show("still pending" + tobepayingfullamount.ToString());


                fullpayedamount = Convert.ToDecimal(txtpayedamount.Text);
                updatePayedAmount = (fullpayedamount + payedamount);
                //  MessageBox.Show("fully payed id" + updatePayedAmount.ToString());
            }
            catch (Exception ex)
            {

            }
        }
        private void PaymentTransactionslistInsert()
        {
            try
            {
                
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                string ProductIDnumber = DateTime.Now.ToString("yyyyMMddHHmmss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "insert into Tbl_PaymentTransactionslist(ID,SubID,MemberID,Name,ProductName,ProductAmount,AmountUnit,CurrentDuration,Rental,PayedAmount,TransactionID,DurationLeftAmount,PaymentType,Remarks,EnterDate,EnterTime,EnterBy) values('" + this.txttransactionid.Text + "', '" + this.lblsubid.Text + "','" + this.lblmemberid.Text + "','" + this.txtname.Text + "','" + this.txtproductname.Text + "','" + this.txtproductammount.Text + "','" + this.lblamountunit.Text + "','" + this.txtduration.Text + "','" + this.txtrental.Text + "','" + this.txtpayedamount.Text + "','" + this.txttransactionsid.Text + "','" + leftAmount + "','" + this.cmbtype.Text + "','" + this.txtremark.Text + "', '" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                UpdatePaymentRegistrationDetails();

                lblmsg.ForeColor = Color.Green;                
                lblmsg.Text = "Payment Record Insert successfully(" + txttransactionid.Text + ")";
                clearText();
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Payment Record Insert faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
      
       

        private void UpdatePaymentRegistrationDetails()
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
                comm.CommandText = "Update Tbl_PaymentRegistrationDetails set LeftPayment='" + tobepayingfullamount + "', PayedAmount='" + updatePayedAmount + "' where  ID ='" + this.lblsubid.Text + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                UpdatePaymetStatus();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Payment Registration Details updated Successfully Member ID (" + lblsubid.Text + ")";
                SaveToolStripMenuItem.Enabled = true;
            }
            catch (Exception Ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Payment Registration Details updated Error /" + Ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on update ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }

        }
        private void UpdatePaymetStatus()
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
                comm.CommandText = "Update Tbl_PaymentTransactions set NotiflyStatus='0' where  ID ='" + this.txttransactionid.Text + "'";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Payment Registration Details updated Successfully Member ID (" + lblsubid.Text + ")";
                SaveToolStripMenuItem.Enabled = true;
                clearText();
            }
            catch (Exception Ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Payment Registration Details updated Error /" + Ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on update ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }

        }

    }
}
