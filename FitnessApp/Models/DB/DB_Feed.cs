using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_Feed
    {
        public List<News> Get(User user)
        {
            try
            {
                List<News> list = new List<News>();
                StaticDatenbank.Connect();

                string com = $"SELECT base.ID, base.Beschreibung, " +
                             $"info.ErstelltAm, info.ErstelltVon, " +
                             $"fotos.Bild, " +
                             $"COUNT(likes.[User]) " +
                             "FROM Feed_Base AS base " +
                             "INNER JOIN Feed_Info AS info " +
                             "ON info.ID = base.ID " +
                             "LEFT JOIN Feed_Fotos AS fotos " +
                             "ON fotos.ID = info.ID " +
                             "LEFT JOIN Feed_Likes as likes " +
                             "ON likes.Feed_ID = base.ID " +
                             "GROUP BY base.ID, base.Beschreibung, info.ErstelltAm, info.ErstelltVon, fotos.Bild";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        ID = r.GetInt32(0),
                        Beschreibung = r.GetString(1),
                        ErstelltAm = r.GetDateTime(2),
                        Ersteller = new User() { Nutzername = r.GetString(3) },
                        Likes = r.GetInt32(5)
                    };

                    if (!r.IsDBNull(4))
                        news.Foto = (byte[])r[4];

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

                string com = "SELECT base.ID, base.Beschreibung, info.ErstelltAm, info.ErstelltVon, fotos.Bild, COUNT(likes.[User]) " +
                             "FROM Feed_Base AS base " +
                             "INNER JOIN Feed_Info AS info " +
                             "ON info.ID = base.ID " +
                             "LEFT JOIN Feed_Fotos AS fotos " +
                             "ON fotos.ID = info.ID " +
                             "LEFT JOIN Feed_Likes as likes " +
                             "ON likes.Feed_ID = base.ID " +
                            $"WHERE info.ErstelltVon = '{user.Nutzername}' " +
                             "GROUP BY base.ID, base.Beschreibung, info.ErstelltAm, info.ErstelltVon, fotos.Bild";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        ID = r.GetInt32(0),
                        Beschreibung = r.GetString(1),
                        ErstelltAm = r.GetDateTime(2),
                        Ersteller = new User() { Nutzername = r.GetString(3) },
                        Likes = r.GetInt32(5)
                    };

                    if (!r.IsDBNull(4))
                        news.Foto = (byte[])r[4];

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

        internal bool Delete(News news)
        {
            string com = null;
            bool result = false;

            if (news.IsFoto)
            {
                com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                result = StaticDatenbank.RunSQL(com);

                if (!result)
                    return false;
            }
            else if (news.IsPlan)
            {
                //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                //result = StaticDatenbank.RunSQL(com);

                //if (!result)
                //    return false;
            }

            com = $"DELETE FROM Feed_Info WHERE ID = '{news.ID}'";
            result = StaticDatenbank.RunSQL(com);

            if (!result)
            {
                if (news.IsFoto)
                {
                    com = $"INSERT INTO Feed_Fotos VALUES('{news.ID}', @bild);";
                    StaticDatenbank.Connect();
                    SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                    command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = news.Foto;
                    StaticDatenbank.Connection.Open();
                    int count = command.ExecuteNonQuery();
                    StaticDatenbank.Connection.Close();

                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else if (news.IsPlan)
                {
                    //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                    //result = StaticDatenbank.RunSQL(com);

                    //if (!result)
                    //    return false;
                }
            }

            com = $"DELETE FROM Feed_Base WHERE ID = '{news.ID}'";
            result = StaticDatenbank.RunSQL(com);

            if (!result)
            {
                com = $"INSERT INTO Feed_Info VALUES('{news.ID}', '{news.ErstelltAm:yyyy-MM-dd HH:mm:ss}', '{news.Ersteller.Nutzername}')";
                result = StaticDatenbank.RunSQL(com);
                if (result == false)
                    return false;

                if (news.IsFoto)
                {
                    com = $"INSERT INTO Feed_Fotos VALUES('{news.ID}', @bild);";
                    StaticDatenbank.Connect();
                    SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                    command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = news.Foto;
                    StaticDatenbank.Connection.Open();
                    int count = command.ExecuteNonQuery();
                    StaticDatenbank.Connection.Close();

                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else if (news.IsPlan)
                {
                    //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                    //result = StaticDatenbank.RunSQL(com);

                    //if (!result)
                    //    return false;
                }
            }

            return true;
        }

        public bool? Like(string id, User user)
        {
            bool isdone = false;
            string com = $"SELECT Feed_ID FROM Feed_Likes WHERE Feed_ID = '{id}' AND [User] = '{user.Nutzername}'";
            int result = StaticDatenbank.GetID(com);

            if (result == -1)
            {
                com = $"INSERT INTO Feed_Likes VALUES('{id}', '{user.Nutzername}')";
                isdone = StaticDatenbank.RunSQL(com);
                if (isdone == true)
                    return true;
                else
                    return null;
            }
            else if (result > 0)
            {
                com = $"DELETE FROM Feed_Likes WHERE Feed_ID = '{id}' AND [User] = '{user.Nutzername}'";
                isdone = StaticDatenbank.RunSQL(com);
                if (isdone == true)
                    return false;
                else
                    return null;
            }
            else
                return null;
        }

        public int GetLikes(string id)
        {
            try
            {
                int count = 0;
                StaticDatenbank.Connect();

                string com = "SELECT COUNT(*)" +
                             "FROM Feed_Likes " +
                            $"WHERE Feed_ID = '{id}'";

                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                object r = command.ExecuteScalar();

                if (r != null)
                    count = int.Parse(r.ToString());
                else
                    count = -1;

                StaticDatenbank.Connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDatenbank.Connection != null)
                    if (StaticDatenbank.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDatenbank.Connection.Close();
                return -1;
            }
        }
    }
}
