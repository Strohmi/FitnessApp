using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_Status
    {
        /// <summary>
        /// Einfügen eines Status
        /// </summary>
        /// <param name="status">Der einzufügende Status</param>
        /// <returns></returns>
        public bool Insert(Status status)
        {
            string com = $"INSERT INTO Feed_Base VALUES('{status.Beschreibung}');";
            bool result = StaticDB.RunSQL(com);
            if (result == false)
                return false;

            int id = StaticDB.GetID($"SELECT ID FROM Feed_Base WHERE Beschreibung = '{status.Beschreibung}'");

            com = $"INSERT INTO Feed_Info VALUES('{id}', '{status.ErstelltAm:yyyy-MM-dd HH:mm:ss}', '{status.ErstelltVon.Nutzername}')";
            result = StaticDB.RunSQL(com);
            if (result == false)
                return false;


            if (status.Foto != null)
            {
                com = $"INSERT INTO Feed_Fotos VALUES('{id}', @bild);";

                StaticDB.Connect();
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = status.Foto;
                StaticDB.Connection.Open();
                int count = command.ExecuteNonQuery();
                StaticDB.Connection.Close();

                if (count > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }
    }
}
