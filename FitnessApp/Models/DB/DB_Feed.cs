using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_Feed
    {
        public List<News> Get()
        {
            try
            {
                List<News> list = new List<News>();
                StaticDatenbank.Connect();

                string com = "SELECT base.Beschreibung, info.ErstelltAm, info.ErstelltVon, fotos.Bild " +
                             "FROM Feed_Base AS base " +
                             "INNER JOIN Feed_Info AS info " +
                             "ON info.ID = base.ID " +
                             "LEFT JOIN Feed_Fotos AS fotos " +
                             "ON fotos.ID = info.ID";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        Beschreibung = r.GetString(0),
                        ErstelltAm = r.GetDateTime(1),
                        Ersteller = new User() { Nutzername = r.GetString(2) }
                    };

                    if (!r.IsDBNull(3))
                        news.Foto = (byte[])r[3];

                    list.Add(news);
                }

                StaticDatenbank.Connection.Close();
                return list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDatenbank.Connection != null)
                    if (StaticDatenbank.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDatenbank.Connection.Close();
                return null;
            }
        }

        public List<News> GetByUser(User user)
        {
            try
            {
                List<News> list = new List<News>();
                StaticDatenbank.Connect();

                string com = "SELECT base.Beschreibung, info.ErstelltAm, info.ErstelltVon, fotos.Bild " +
                             "FROM Feed_Base AS base " +
                             "INNER JOIN Feed_Info AS info " +
                             "ON info.ID = base.ID " +
                             "LEFT JOIN Feed_Fotos AS fotos " +
                             "ON fotos.ID = info.ID " +
                             $"WHERE info.ErstelltVon = '{user.Nutzername}'";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        Beschreibung = r.GetString(0),
                        ErstelltAm = r.GetDateTime(1),
                        Ersteller = new User() { Nutzername = r.GetString(2) }
                    };

                    if (!r.IsDBNull(3))
                        news.Foto = (byte[])r[3];

                    list.Add(news);
                }

                StaticDatenbank.Connection.Close();
                return list;
            }
            catch (Exception ex)
            {
                if (StaticDatenbank.Connection != null)
                    if (StaticDatenbank.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDatenbank.Connection.Close();
                return null;
            }
        }
    }
}
