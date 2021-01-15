using System;
using System.Data.SqlClient;

namespace FitnessApp
{
    public static class StaticDB
    {
        public static SqlConnection Connection { get; set; }

        /// <summary>
        /// Statische Methode zum Verbinden mit der Datenbank
        /// </summary>
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

        /// <summary>
        /// Statische Methode zum Ausführen einer einfachen SQL-Anweisung mit einem boolschen Rückgabewert
        /// </summary>
        /// <param name="com">SQL-Answeisung</param>
        /// <returns></returns>
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

        /// <summary>
        /// Prüfen, ob Element in der Datenbank existiert
        /// </summary>
        /// <param name="com">SQL-Anweisung - wichtig: SELECT *...</param>
        /// <returns></returns>
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
                _ = ex.Message;
                if (Connection != null)
                    if (Connection.State != System.Data.ConnectionState.Closed)
                        Connection.Close();
                return null;
            }
        }

        /// <summary>
        /// ID zu einem Element aus der Datenbank erhalten
        /// </summary>
        /// <param name="com">SQL-Answeisung - wichtig: SELECT ID...</param>
        /// <returns></returns>
        public static int GetID(string com)
        {
            try
            {
                Connect();
                SqlCommand command = new SqlCommand(com, Connection);
                Connection.Open();
                object result = command.ExecuteScalar();
                Connection.Close();

                if (result != null)
                    return (int)result;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (Connection != null)
                    if (Connection.State != System.Data.ConnectionState.Closed)
                        Connection.Close();
                return -2;
            }
        }

        public static byte ConvertBoolToByte(bool _input)
        {
            if (_input)
                return 1;
            else
                return 0;
        }

        public static bool ConvertByteToBool(byte _input)
        {
            if (_input == 1)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Zusammenfassung aller Datenbank-Kategorien zur besseren Übersicht
    /// </summary>
    public class Datenbank
    {
        public DB_Feed Feed { get; set; } = new DB_Feed();
        public DB_User User { get; set; } = new DB_User();
        public DB_Trainingsplan Trainingsplan { get; set; } = new DB_Trainingsplan();
        public DB_Status Status { get; set; } = new DB_Status();
        public DB_Ernährungsplan Ernährungsplan { get; set; } = new DB_Ernährungsplan();
    }
}
