using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public static class StaticDatenbank
    {
        public static SqlConnection Connection { get; set; }

        //Kommentar

        public static void Connect()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "hs-wi.database.windows.net",
                InitialCatalog = "FitnessApp",
                UserID = "HSWI_FitnessApp",
                Password = "qU&Xv4C^QeNjN4rDTCTw",
                ConnectTimeout = 30
            };

            Connection = new SqlConnection(builder.ConnectionString);
        }

        internal static bool RunSQL(string com)
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
                _ = ex.Message;
                if (Connection != null)
                    if (Connection.State != System.Data.ConnectionState.Closed)
                        Connection.Close();
                return false;
            }
        }

        internal static bool? CheckExistenz(string com)
        {
            try
            {
                int rows = 0;
                Connect();
                SqlCommand command = new SqlCommand(com, Connection);
                Connection.Open();
                var r = command.ExecuteReader();
                while (r.Read())
                {
                    rows += 1;
                }
                Connection.Close();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (Connection != null)
                    if (Connection.State != System.Data.ConnectionState.Closed)
                        Connection.Close();
                return null;
            }
        }
    }

    public class Datenbank
    {
        public DB_Feed Feed { get; set; } = new DB_Feed();
        public DB_User User { get; set; } = new DB_User();
    }
}
