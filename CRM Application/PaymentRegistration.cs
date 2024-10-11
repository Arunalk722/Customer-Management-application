using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CRM_Application
{
    public partial class PaymentRegistration : Form
    {
        public PaymentRegistration()
        {
            InitializeComponent();
        }
      
        decimal DownPayment, LeftAmmount, ProductAmmount;

        //duration
        decimal Duration, Rental;
       
        string ProductRegID, PaymentTID, DueDate, subid, NotiflyDate, ProductID;
        int currentDuration = 1;

        private void logmessage()
        {
            using (StreamWriter w = File.AppendText("log\\PaymentRegistrationExceLog.txt"))
            {
                Log(lblmsg.Text, w);
            }
            using (StreamReader r = File.OpenText("log\\PaymentRegistrationExceLog.txt"))
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
        private void txtproductammount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(txtproductammount.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void txtdownpayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(txtdownpayment.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void txtduration_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(txtduration.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void txtnotification_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(txtnotification.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void txtsettlmentresetdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(txtsettlmentresetdate.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void txtmemberID_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtmemberID.Text == string.Empty)
            {

            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string fmt = "000000.##";
                    decimal Result_With_Zero;
                    Result_With_Zero = Convert.ToDecimal(txtmemberID.Text);
                    lblmemberid.Text = Result_With_Zero.ToString(Properties.Settings.Default.MemberHashIDFrmt + fmt);
                    ScanMemberListActiveRecord();
                    e.Handled = true;
                }
                else
                {

                }
            }
        }

        private void MemberPurchaseDetailsView()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                string Query = "Select * from Tbl_PaymentTransactions where MemberID='"+ this.lblmemberid.Text + "'";
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
                    lblmsg.Text = "View old Purchase";
                   

                }
                else
                {
                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "There no registered record in this member";
                    dtgrpt.DataSource = null;
                }
            }

            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "registered record record scaning error ." + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void ScanMemberListActiveRecord()
        {
            try
            {
                if (txtmemberID.Text == string.Empty)
                {

                }
                else
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
                        txtbatch.Text = MyReader2["Batch"].ToString();
                        txtemail.Text = MyReader2["Email"].ToString();
                        txtmobile.Text = MyReader2["Mobile"].ToString();
                        txtname.Text = MyReader2["Name"].ToString();
                        txtnic.Text = MyReader2["NIC"].ToString();
                        txtaddress.Text = MyReader2["Address"].ToString();
                        txtSalesRap.Text = MyReader2["ManageBy"].ToString();
                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Scan Member ID (" + txtmemberID.Text + ")";
                        MemberPurchaseDetailsView();
                        txtproductname.Focus();
                    }
                    else
                    {
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Please Check Member ID (" + txtmemberID.Text + ")";
                        clearText();
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
        private void txtproductname_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtproductname.Text == string.Empty)
            {

            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtproductammount.Focus();
                    e.Handled = true;
                }
            }
        }
        private void txtproductammount_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtproductammount.Text == string.Empty)
            {

            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtpaybleammount.Text = txtproductammount.Text;
                    deliverydate.Focus();
                    e.Handled = true;
                }
            }
        }
        private void txtdownpayment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtdownpayment.Text == string.Empty)
                {

                }
                else
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        ProductAmmount = Convert.ToDecimal(txtproductammount.Text);
                        DownPayment = Convert.ToDecimal(txtdownpayment.Text);
                        LeftAmmount = (ProductAmmount - DownPayment);
                        txtleftamount.Text = Convert.ToString(LeftAmmount);
                        txtduration.Focus();
                        e.Handled = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void txtduration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtduration.Text == string.Empty)
                {

                }
                else
                {
                    if (e.KeyCode == Keys.Enter)
                    {



                        Duration = Convert.ToDecimal(txtduration.Text);
                        Rental = (LeftAmmount / Duration);
                        txtrental.Text = Convert.ToString(Rental);
                        txtsettlmentresetdate.Focus();
                        e.Handled = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void txtsettlmentresetdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtsettlmentresetdate.Text == string.Empty)
            {

            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtnotification.Focus();
                    e.Handled = true;
                }
            }
        }
        private void PaymentRegistration_Load(object sender, EventArgs e)
        {
            lbluser.Text = EaseyAccess.LoginUserName;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure to confirm this payment registration ? insert details was correct?", "Payment Registration confirm ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                if(lblmemberid.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Member Details");
                }
                else
                {
                    ProductNameID();
                }
            else
            {

            }

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearText();
        }

        private void ProductNameID()
        {
            try            
                {
                    int a;
                    DataBaseConnection newconnection = new DataBaseConnection();
                    newconnection.connection_today();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = DataBaseConnection.Lconn;
                    comm.CommandText = "Select Max(ID) from Tbl_ProductNameList";
                    SqlDataReader MyReader2;
                    MyReader2 = comm.ExecuteReader();
                    while (MyReader2.Read())
                    {
                        string val = MyReader2[0].ToString();
                        if (val == "")
                        {
                        ProductID = "1";
                        ProductNameInsert();
                        }
                        else
                        {
                            a = Convert.ToInt32(MyReader2[0].ToString());
                            a = a + 1;
                            ProductID = a.ToString();
                            ProductNameInsert();
                        }                
                    }   
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Product ID Create faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
        private void ProductNameInsert()
        {
            try
            {
               
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "insert into Tbl_ProductNameList(ID,MemberID,ProductName,Amount,Unit,EnterDate,EnterTime,EnterBy) values('" + ProductID + "', '" + this.lblmemberid.Text + "','" + this.txtproductname.Text + "','" + this.txtproductammount.Text + "', '" + this.lblmemberid.Text + "','" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Product Name Insert successfully(" + txtmemberID.Text + ")";
                PaymentRegistrationDetailsID();
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Product Name Insert faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
        
        private void PaymentRegistrationDetailsID()
        {
            try
            {
                int a;
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Select Max(ID) from Tbl_PaymentRegistrationDetails";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                while (MyReader2.Read())
                {
                    string val = MyReader2[0].ToString();
                    if (val == "")
                    {
                        ProductRegID = "1";
                        PaymentRegistrationDetailsInsert();
                    }
                    else
                    {
                        a = Convert.ToInt32(MyReader2[0].ToString());
                        a = a + 1;
                        ProductRegID = a.ToString();
                        PaymentRegistrationDetailsInsert();
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Product Reg ID Create faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

        private void PaymentRegistrationDetailsInsert()
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
                comm.CommandText = "insert into Tbl_PaymentRegistrationDetails(ID,MemberID,Name,NIC,Email,MemberBatch,ProductName,ProductAmount,AmountUnit,FullAmount,DownPayment,LeftPayment,PayedAmount,ResetDate,Rental,DeliveryDate,Notification,EnterDate,EnterTime,EnterBy) values('" + ProductRegID + "', '" + this.lblmemberid.Text + "','" + this.txtname.Text + "','" + this.txtnic.Text + "','" + this.txtemail.Text + "','" + this.txtbatch.Text + "','" + this.txtproductname.Text + "','" + this.txtproductammount.Text + "','" + this.lblamountunit.Text + "','" + this.txtpaybleammount.Text + "','" + this.txtdownpayment.Text + "','-" + this.txtleftamount.Text + "','" + this.txtdownpayment.Text + "','" + this.txtsettlmentresetdate.Text + "','" + this.txtrental.Text + "','" + deliverydate.Text + "','" + this.txtnotification.Text + "', '" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Product Name Insert successfully(" + txtmemberID.Text + ")";
                LoopCreate();
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Product Name Insert faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }

      
        private void LoopCreate()
        {
            try
            {
                
                int LoopLeft = Convert.ToInt16(txtduration.Text);
                int A = 0;
                currentDuration = 1;
                int DateCount = Convert.ToInt16(txtsettlmentresetdate.Text);
                int NotiflyDatecount = Convert.ToInt16(txtnotification.Text);
                for (int i = 0; i < LoopLeft; i++)
                {
                    DateTime nextPaybleDate =Convert.ToDateTime(deliverydate).AddDays(DateCount + A);
                    var DateOnly = nextPaybleDate.ToString("MM/dd/yyyy");

                    DateTime today = DateTime.Today;
                    DateTime NotiflyDateSet = today.AddDays(DateCount + A - NotiflyDatecount);
                    var NotiflyDateOnly = NotiflyDateSet.ToString("MM/dd/yyyy");

                    DueDate = DateOnly;
                    NotiflyDate = NotiflyDateOnly;

                    MessageBox.Show("Payment Due Date " + DueDate + " Payment Notifly " + NotiflyDateOnly);

                    PaymentTransactionsIDCreate();
                    A = (DateCount + A);
                    currentDuration++;
                }
                clearText();

                /* int LoopLeft = Convert.ToInt16(txtduration.Text);
                 int A = 0;
                 currentDuration = 1;
                 DateTime today = deliverydate.Value;
                 int DateCount = Convert.ToInt16(txtsettlmentresetdate.Text);
                 int NotiflyDatecount = Convert.ToInt16(txtnotification.Text);
                 for (int i = 0; i < LoopLeft; i++)
                 {
                     DateTime nextPaybleDate = today.AddDays(DateCount + A);
                     var DateOnly = nextPaybleDate.ToString("MM/dd/yyyy");

                     DateTime NotiflyDateSet = today.AddDays(DateCount + A - NotiflyDatecount);
                     var NotiflyDateOnly = NotiflyDateSet.ToString("MM/dd/yyyy");

                     DueDate = DateOnly;
                     NotiflyDate = NotiflyDateOnly;


                     MessageBox.Show("Payment Due Date " + DueDate + "Payment Notifly " + NotiflyDateOnly);

                     PaymentTransactionsIDCreate();
                     A = (DateCount + A);
                     currentDuration++;
                 }
                 clearText();*/
            }
            catch (Exception)
            {

            }
        }
        private void clearText()
        {
            txtmemberID.Text = "";
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
            dtgrpt.DataSource = null;
            dtgrpt.Refresh();

        }
        private void PaymentTransactionsIDCreate()
        {
            try
            {
                int a;
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "Select Max(ID) from Tbl_PaymentTransactions";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                while (MyReader2.Read())
                {
                    string val = MyReader2[0].ToString();
                    if (val == "")
                    {
                        PaymentTID = "1";
                        PaymentTransactionsInsert();
                    }
                    else
                    {
                        a = Convert.ToInt32(MyReader2[0].ToString());
                        a = a + 1;
                        PaymentTID = a.ToString();
                        PaymentTransactionsInsert();
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Payment Transactions ID Create faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
       
        private void PaymentTransactionsInsert()
        {
            try
            {
              
                string Status = "1";
                string EnterDate = DateTime.Now.ToString("yyyy.MM.dd");
                string EnterTime = DateTime.Now.ToString("HH:mm:ss");
                string ProductIDnumber = DateTime.Now.ToString("yyyyMMddHHmmss");
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                comm.CommandText = "insert into Tbl_PaymentTransactions(ID,SubID,MemberID,Name,ProductName,ProductAmount,AmountUnit,DownPayment,OutstandingPayment,DurationCount,CurrentDuration,Rental,PendingPayment,DueDate,NotificationDate,NotiflyStatus,EnterDate,EnterTime,EnterBy) values('" + PaymentTID + "', '" + ProductRegID + "','" + this.lblmemberid.Text + "','" + this.txtname.Text + "','" + this.txtproductname.Text + "','" + this.txtproductammount.Text + "','" + this.lblamountunit.Text + "','" + this.txtdownpayment.Text + "','" + this.txtleftamount.Text + "','" + this.txtduration.Text + "','" + currentDuration + "','" + this.txtrental.Text + "','-" + this.txtrental.Text + "','" + DueDate + "','" + NotiflyDate + "','" + Status + "', '" + EnterDate + "', '" + EnterTime + "', '" + this.lbluser.Text + "'); ";
                SqlDataReader MyReader2;
                MyReader2 = comm.ExecuteReader();
                DataBaseConnection.Lconn.Close();
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Product Name Insert successfully(" + txtmemberID.Text + ")";
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Product Name Insert faild " + ex.ToString();
                MessageBox.Show(lblmsg.Text, "Exception on error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logmessage();
            }
        }
    }
}
