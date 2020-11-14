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

                string com = "";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        Beschreibung = (string)r["Beschreibung"],
                        Ersteller = new User() { Nutzername = (string)r["ErstelltVon"] }
                    };

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

        public List<News> GetByUser(User user)
        {
            try
            {
                List<News> list = new List<News>();
                StaticDatenbank.Connect();

                string com = "";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        Beschreibung = (string)r["Beschreibung"],
                        Ersteller = new User() { Nutzername = (string)r["ErstelltVon"] }
                    };

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
