using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_Feed : Datenbank
    {
        public List<News> Get()
        {
            try
            {
                List<News> list = new List<News>();
                Connect();

                string com = "";
                SqlCommand command = new SqlCommand(com, Connection);
                Connection.Open();
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

                Connection.Close();
                return list;
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
}
