using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FitnessApp.Models.General;

namespace FitnessApp.Models
{
    //Autor: NiE

    public class DB_Feed
    {
        /// <summary>
        /// Ausgabe der Feed-Einträge nach einem bestimmten Nutzer und optional in einem bestimmten Zeitraum
        /// </summary>
        /// <param name="user">Benutzer</param>
        /// <param name="vonDatum">Ab wann soll selektiert werden</param>
        /// <param name="bisDatum">Bis wann soll selektiert werden</param>
        /// <returns></returns>
        public List<News> Get(User user, DateTime vonDatum = default, DateTime bisDatum = default)
        {
            try
            {
                if (bisDatum == default)
                    bisDatum = DateTime.Now;

                List<News> list = new List<News>();
                StaticDB.Connect();

                string com = $"SELECT DISTINCT base.ID, base.Beschreibung, info.ErstelltAm, info.ErstelltVon, fotos.Bild " +
                             $"FROM Feed_Base AS base " +
                             $"INNER JOIN Feed_Info AS info " +
                             $"ON info.ID = base.ID " +
                             $"INNER JOIN User_Follows AS follows " +
                             $"ON follows.Follow_ID = '{user.Nutzername}' " +
                             $"LEFT JOIN Feed_Fotos AS fotos " +
                             $"ON fotos.ID = info.ID " +
                             $"WHERE(info.ErstelltVon = follows.User_ID OR info.ErstelltVon = '{user.Nutzername}') " +
                             $"AND info.ErstelltAm >= '{vonDatum:yyyy-MM-dd}' " +
                             $"AND info.ErstelltAm <= '{bisDatum:yyyy-MM-dd HH:mm:ss}';";

                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    News news = new News()
                    {
                        ID = r.GetInt32(0),
                        Beschreibung = r.GetString(1),
                        ErstelltAm = r.GetDateTime(2),
                        Ersteller = new User() { Nutzername = r.GetString(3) },
                    };

                    if (!r.IsDBNull(4))
                        news.Foto = (byte[])r[4];

                    list.Add(news);
                }

                StaticDB.Connection.Close();

                foreach (var item in list)
                {
                    item.Liked = CheckIfLiked(item.ID);
                    item.Likes = GetLikes(item.ID);
                    item.Ersteller = AllVM.Datenbank.User.GetByName(item.Ersteller.Nutzername);
                }

                return list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }

        /// <summary>
        /// Überprüfen, ob der aktuelle Nutzer den Beitrag geliked hat.
        /// </summary>
        /// <param name="id">ID des Beitrages</param>
        /// <returns></returns>
        public bool CheckIfLiked(int id)
        {
            User user = AllVM.ConvertToUser();

            try
            {
                string com = $"SELECT Feed_ID FROM Feed_Likes WHERE Feed_ID = '{id}' AND [User] = '{user.Nutzername}'";
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                object r = command.ExecuteScalar();
                StaticDB.Connection.Close();

                if (r != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return false;
            }
        }

        /// <summary>
        /// Feed-Beiträge zu einem bestimmten Nutzer ausgeben
        /// </summary>
        /// <param name="user">Zu suchender Benutzer</param>
        /// <returns></returns>
        public List<News> GetByUser(User user)
        {
            try
            {
                List<News> list = new List<News>();
                StaticDB.Connect();

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
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
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

                StaticDB.Connection.Close();

                foreach (var item in list)
                    item.Liked = CheckIfLiked(item.ID);

                return list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }

        /// <summary>
        /// Löschen eines Feed-Beitrages
        /// </summary>
        /// <param name="news">Der zulöschende Beitrag</param>
        /// <returns></returns>
        internal bool Delete(News news)
        {
            string com = null;
            bool result = false;

            List<Like> listLikes = GetLikesWithNames(news.ID.ToString());

            if (news.IsFoto)
            {
                com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                result = StaticDB.RunSQL(com);

                if (!result)
                    return false;
            }
            else if (news.IsPlan)
            {
                //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                //result = StaticDB.RunSQL(com);

                //if (!result)
                //    return false;
            }

            com = $"DELETE FROM Feed_Likes WHERE Feed_ID = '{news.ID}'";
            result = StaticDB.RunSQL(com);

            if (!result)
            {
                if (news.IsFoto)
                {
                    com = $"INSERT INTO Feed_Fotos VALUES('{news.ID}', @bild);";
                    StaticDB.Connect();
                    SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                    command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = news.Foto;
                    StaticDB.Connection.Open();
                    int count = command.ExecuteNonQuery();
                    StaticDB.Connection.Close();
                }
                else if (news.IsPlan)
                {
                    //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                    //result = StaticDB.RunSQL(com);

                    //if (!result)
                    //    return false;
                }
                return false;
            }

            com = $"DELETE FROM Feed_Info WHERE ID = '{news.ID}'";
            result = StaticDB.RunSQL(com);

            if (!result)
            {
                foreach (var item in listLikes)
                {
                    com = $"INSERT INTO Feed_Likes VALUES('{news.ID}', '{item.User.Nutzername}')";
                    StaticDB.RunSQL(com);
                }

                if (news.IsFoto)
                {
                    com = $"INSERT INTO Feed_Fotos VALUES('{news.ID}', @bild);";
                    StaticDB.Connect();
                    SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                    command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = news.Foto;
                    StaticDB.Connection.Open();
                    int count = command.ExecuteNonQuery();
                    StaticDB.Connection.Close();
                }
                else if (news.IsPlan)
                {
                    //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                    //result = StaticDB.RunSQL(com);

                    //if (!result)
                    //    return false;
                }
                return false;
            }

            com = $"DELETE FROM Feed_Base WHERE ID = '{news.ID}'";
            result = StaticDB.RunSQL(com);

            if (!result)
            {
                com = $"INSERT INTO Feed_Info VALUES('{news.ID}', '{news.ErstelltAm:yyyy-MM-dd HH:mm:ss}', '{news.Ersteller.Nutzername}')";
                result = StaticDB.RunSQL(com);

                if (news.IsFoto)
                {
                    com = $"INSERT INTO Feed_Fotos VALUES('{news.ID}', @bild);";
                    StaticDB.Connect();
                    SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                    command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = news.Foto;
                    StaticDB.Connection.Open();
                    int count = command.ExecuteNonQuery();
                    StaticDB.Connection.Close();
                }
                else if (news.IsPlan)
                {
                    //com = $"DELETE FROM Feed_Fotos WHERE ID = '{news.ID}'";
                    //result = StaticDB.RunSQL(com);

                    //if (!result)
                    //    return false;
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Einen Beitrag liken
        /// </summary>
        /// <param name="id">ID des Beitrages</param>
        /// <param name="user">Benutzer, der den Beitrag geliked hat</param>
        /// <returns></returns>
        public bool? Like(string id, User user)
        {
            bool isdone = false;
            string com = $"SELECT Feed_ID FROM Feed_Likes WHERE Feed_ID = '{id}' AND [User] = '{user.Nutzername}'";
            int result = StaticDB.GetID(com);

            if (result == -1)
            {
                com = $"INSERT INTO Feed_Likes VALUES('{id}', '{user.Nutzername}')";
                isdone = StaticDB.RunSQL(com);
                if (isdone == true)
                    return true;
                else
                    return null;
            }
            else if (result > 0)
            {
                com = $"DELETE FROM Feed_Likes WHERE Feed_ID = '{id}' AND [User] = '{user.Nutzername}'";
                isdone = StaticDB.RunSQL(com);
                if (isdone == true)
                    return false;
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Aktuellen Like-Anzahl zu einem Beitrag ausgeben
        /// </summary>
        /// <param name="id">ID des Beitrages</param>
        /// <returns></returns>
        public int GetLikes(int id)
        {
            try
            {
                int count = 0;
                StaticDB.Connect();

                string com = "SELECT COUNT(*)" +
                             "FROM Feed_Likes " +
                            $"WHERE Feed_ID = '{id}'";

                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                object r = command.ExecuteScalar();

                if (r != null)
                    count = int.Parse(r.ToString());
                else
                    count = -1;

                StaticDB.Connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return -1;
            }
        }

        /// <summary>
        /// Likes und Nutzernamen zu einem Beitrag erhalten
        /// </summary>
        /// <param name="id">ID des Beitrages</param>
        /// <returns></returns>
        public List<Like> GetLikesWithNames(string id)
        {
            try
            {
                List<Like> list = new List<Like>();
                StaticDB.Connect();

                string com = "SELECT [User]" +
                             "FROM Feed_Likes " +
                            $"WHERE Feed_ID = '{id}'";
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                int counter = 1;
                while (r.Read())
                {
                    Like like = new Like()
                    {
                        Index = counter,
                        User = new User() { Nutzername = r.GetString(0) }
                    };
                    list.Add(like);
                    counter += 1;
                }

                StaticDB.Connection.Close();
                return list;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }
    }
}
