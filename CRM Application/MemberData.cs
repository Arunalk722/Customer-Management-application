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
    public partial class MemberData : Form
    {
        public MemberData()
        {
            InitializeComponent();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memberDataView();
        }
        private void memberDataView()
        {
            try
            {
                DataBaseConnection newconnection = new DataBaseConnection();
                newconnection.connection_today();
                SqlCommand comm = new SqlCommand();
                comm.Connection = DataBaseConnection.Lconn;
                string Query = "Select * from Tbl_MembersList WHERE EnterDate  BETWEEN '" + date_start.Text + "' and '" + date_end.Text + "' and Batch like '%" + this.txtbatch.Text + "%'and Name like '%" + this.txtname.Text + "%'and NIC like '%" + this.txtnic.Text + "%'  and ManageBy like '%" + this.cmbsalesrepslist.Text + "%'";
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
                    dataadapter.Fill(ds, "Tbl_MembersList");
                    connection.Close();
                    dtgrpt.DataSource = ds;
                    dtgrpt.DataMember = "Tbl_MembersList";

                    DataBaseConnection.Lconn.Close();

                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "Record are finded base on user inputs";
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
            }
        }

        private void MemberData_Load(object sender, EventArgs e)
        {

        }

        private void cmbsalesrepslist_DropDown(object sender, EventArgs e)
        {
            SalesRepSelection();
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
            }
        }

        private void selectionModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabview.SelectedTab = tabrpt;
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
