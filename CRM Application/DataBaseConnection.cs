using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_Application
{
    class DataBaseConnection
    {
        public static SqlConnection Lconn = null;
        public void connection_today()
        {
            
           Lconn = new SqlConnection("Data Source=.;Initial Catalog=" + Properties.Settings.Default.DatabaseName + ";Persist Security Info=True;User ID=sa;Password=Passwrod;Max Pool Size=1024;Pooling=true");
                             
            if (Lconn.State == System.Data.ConnectionState.Closed)
            {

                Lconn.Open();
            }
            else
            {
               
            }
        }
     
    }
}
