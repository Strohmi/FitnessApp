using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class Datenbank
    {
        public object MyProperty { get; set; }

        public SqlConnection Connection { get; set; }

        internal void Connect()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "hs-wi.database.windows.net",
                InitialCatalog = "FitnessApp",
                UserID = "HSWI_FitnessApp",
                Password = "qU&Xv4C^QeNjN4rDTCTw!",
                ConnectTimeout = 30
            };

            Connection = new SqlConnection(builder.ConnectionString);
        }

        internal bool RunSQL(string com)
        {
            try
            {
                Connect();
                SqlCommand command = new SqlCommand(com, Connection);
                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (Connection != null)
                    if (Connection.State != System.Data.ConnectionState.Closed)
                        Connection.Close();
                return false;
            }
        }
    }
}
