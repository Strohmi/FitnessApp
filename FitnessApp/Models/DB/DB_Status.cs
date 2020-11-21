﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_Status
    {
        public bool Insert(Status status)
        {
            string com = $"INSERT INTO Feed_Base VALUES('{status.Beschreibung}');";
            bool result = StaticDatenbank.RunSQL(com);
            if (result == false)
                return false;

            int id = StaticDatenbank.GetID($"SELECT ID FROM Feed_Base WHERE Beschreibung = '{status.Beschreibung}'");

            com = $"INSERT INTO Feed_Info VALUES('{id}', '{status.ErstelltAm:yyyy-MM-dd HH:mm:ss}', '{status.ErstelltVon.Nutzername}')";
            result = StaticDatenbank.RunSQL(com);
            if (result == false)
                return false;


            if (status.Foto != null)
            {
                com = $"INSERT INTO Feed_Fotos VALUES('{id}', @bild);";

                StaticDatenbank.Connect();
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = status.Foto;
                StaticDatenbank.Connection.Open();
                int count = command.ExecuteNonQuery();
                StaticDatenbank.Connection.Close();

                if (count > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }
    }
}