using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FitnessApp.Models.General;
using FitnessApp.ViewModels;

namespace FitnessApp.Models
{
    //Autor: NiE

    public class DB_User
    {
        /// <summary>
        /// Alle Benutzer als Liste ausgeben
        /// </summary>
        /// <returns></returns>
        internal List<User> GetList()
        {
            try
            {
                List<User> list = new List<User>();
                StaticDB.Connect();

                string com = "SELECT base.Nutzername, info.CustomName " +
                             "FROM User_Base AS base " +
                             "INNER JOIN User_Info AS info " +
                             "ON info.Nutzername =  base.Nutzername;";
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    User user = new User()
                    {
                        Nutzername = r.GetString(0),
                        CustomName = r.GetString(1)
                    };
                    list.Add(user);
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

        internal bool? Exists(string user)
        {
            string com = $"SELECT Nutzername FROM User_Base WHERE Nutzername = '{user}'";
            return StaticDB.CheckExistenz(com);
        }

        internal string GetPasswort(string nutzername)
        {
            try
            {
                StaticDB.Connect();

                string com = $"SELECT Password FROM User_Password WHERE Nutzername = '{nutzername}'";

                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                object r = command.ExecuteScalar();
                StaticDB.Connection.Close();

                if (r != null)
                    return r.ToString();
                else
                    return null;
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

        internal bool ChangePW(string nutzername, string hpw)
        {
            string com = $"UPDATE User_Password " +
                         $"SET Password = '{hpw}' " +
                         $"WHERE Nutzername = '{nutzername}';";
            return StaticDB.RunSQL(com);
        }

        /// <summary>
        /// Favorisierte Pläne (Training und Ernährung) aus der Datenbank ermitteln
        /// </summary>
        /// <returns></returns>
        internal List<FavoPlan> GetFavoPlans()
        {
            try
            {
                List<FavoPlan> list = new List<FavoPlan>();
                StaticDB.Connect();

                string com = $"SELECT Plan_ID, Typ FROM User_Favo WHERE Nutzername = '{AllVM.User.Nutzername}'";

                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    FavoPlan favo = new FavoPlan()
                    {
                        ID = r.GetInt32(0),
                        Typ = r.GetString(1)
                    };
                    list.Add(favo);
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

        /// <summary>
        /// Einen neuen Benutzer hinzufügen
        /// </summary>
        /// <param name="user">einzufügender Benutzer</param>
        /// <returns></returns>
        internal bool Insert(User user)
        {
            string com = $"INSERT INTO User_Base VALUES('{user.Nutzername}');";
            bool result = StaticDB.RunSQL(com);

            if (result == false)
                return false;

            com = $"INSERT INTO User_Info (Nutzername, ErstelltAm, Infotext, OnlyCustomName, CustomName) VALUES('{user.Nutzername}', '{user.ErstelltAm:yyyy-MM-dd HH:mm:ss}', '{user.InfoText}', '0', '{user.Nutzername}')";
            result = StaticDB.RunSQL(com);

            if (result == false)
                return false;

            com = $"INSERT INTO User_Password VALUES('{user.Nutzername}', '{user.Passwort}')";
            result = StaticDB.RunSQL(com);

            if (result == false)
                return false;


            result = UploadProfilBild(user, user.ProfilBild);
            if (result == false)
                return false;

            return true;
        }

        /// <summary>
        /// Zusätzliche Informationen zu einem Benutzer aufgrund eines Nutzernames erhalten
        /// </summary>
        /// <param name="nutzername">Nutzername des zu suchenden Benutzers</param>
        /// <returns></returns>
        public User GetByName(string nutzername)
        {
            try
            {
                User user = null;
                StaticDB.Connect();

                string com = "SELECT base.Nutzername, info.ErstelltAm, info.Infotext, info.OnlyCustomName, info.CustomName, bild.Bild " +
                             "FROM User_Base AS base " +
                             "INNER JOIN User_Info AS info " +
                             "ON info.Nutzername = base.Nutzername " +
                             "LEFT JOIN User_Bild AS bild " +
                             "ON bild.Nutzername = base.Nutzername " +
                             $"WHERE base.Nutzername = '{nutzername}';";
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    user = new User()
                    {
                        Nutzername = r.GetString(0),
                        ErstelltAm = r.GetDateTime(1),
                        InfoText = r.GetString(2),
                        OnlyCustomName = StaticDB.ConvertByteToBool(r.GetByte(3)),
                        CustomName = r.GetString(4)
                    };

                    if (!r.IsDBNull(5))
                        user.ProfilBild = (byte[])r[5];
                }

                StaticDB.Connection.Close();
                user.AnzahlFollower = GetAnzahlFollower(user.Nutzername);

                return user;
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
        /// Anzahl der Follower ermitteln
        /// </summary>
        /// <param name="nutzername">Zu suchender Benutzer</param>
        /// <returns></returns>
        private int GetAnzahlFollower(string nutzername)
        {
            try
            {
                string com = $"SELECT COUNT(*) FROM User_Follows WHERE User_ID = '{nutzername}'";
                StaticDB.Connect();
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                object result = command.ExecuteScalar();
                StaticDB.Connection.Close();

                if (result != null)
                    return (int)result;
                else
                    return -1;
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
        /// Plan aus Favoritenliste löschen
        /// </summary>
        /// <param name="classid">Bezeichner aus MenuItem.ClassID (Typ;Id)</param>
        /// <param name="user">Benutzer</param>
        /// <returns></returns>
        internal bool DeleteFavo(string classid, User user)
        {
            string[] keys = classid.Split(';');
            string com = $"DELETE FROM User_Favo WHERE Nutzername = '{user.Nutzername}' AND Typ = '{keys[0]}' AND Plan_ID = '{keys[1]}'";
            return StaticDB.RunSQL(com);
        }

        /// <summary>
        /// Aktualisieren der neuen Werte für einen Benutzer
        /// </summary>
        /// <param name="user">zu aktualisierender Benutzer</param>
        /// <returns></returns>
        internal bool Update(User user)
        {
            string com = $"UPDATE User_Info SET " +
                         $"GeaendertAm = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', " +
                         $"Infotext = '{user.InfoText}'," +
                         $"OnlyCustomName = '{StaticDB.ConvertBoolToByte(user.OnlyCustomName)}'," +
                         $"CustomName = '{user.CustomName}' " +
                         $"WHERE Nutzername = '{user.Nutzername}';";

            bool result = StaticDB.RunSQL(com);

            if (result == false)
                return result;

            try
            {
                com = $"UPDATE User_Bild SET " +
                      $"Bild = @bild " +
                      $"WHERE Nutzername = '{user.Nutzername}';";

                StaticDB.Connect();
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = user.ProfilBild;
                command.ExecuteNonQuery();
                StaticDB.Connection.Close();
                return true;
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
        /// Liste der Follower zu einem Benutzer ausgeben
        /// </summary>
        /// <param name="profil">Benutzername</param>
        /// <returns></returns>
        internal List<Follower> GetFollows(string profil)
        {
            try
            {
                List<Follower> list = new List<Follower>();
                StaticDB.Connect();

                string com = "SELECT Follow_ID, GefolgtAm " +
                             "FROM User_Follows " +
                             $"WHERE User_ID = '{profil}';";
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                int counter = 1;
                while (r.Read())
                {
                    Follower follower = new Follower()
                    {
                        Index = counter,
                        User = new User() { Nutzername = r.GetString(0) },
                        GefolgtAm = r.GetDateTime(1)
                    };
                    list.Add(follower);
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

        /// <summary>
        /// Löschen des Benutzers
        /// </summary>
        /// <param name="user">Benutzer</param>
        /// <returns></returns>
        internal bool Delete(User user)
        {
            try
            {
                string com = $"DELETE FROM User_Info WHERE Nutzername = '{user.Nutzername}';" +
                             $"DELETE FROM User_Bild WHERE Nutzername = '{user.Nutzername}';" +
                             $"DELETE FROM User_Favo WHERE Nutzername = '{user.Nutzername}';" +
                             $"DELETE FROM User_Password WHERE Nutzername = '{user.Nutzername}';" +
                             $"DELETE FROM User_Follows WHERE User_ID = '{user.Nutzername}' OR Follow_ID = '{user.Nutzername}';" +
                             $"DELETE FROM User_Base WHERE Nutzername = '{user.Nutzername}';";

                bool result = StaticDB.RunSQL(com);
                if (result == false)
                    return result;


                List<News> news = AllVM.Datenbank.Feed.GetByUser(user);
                List<Trainingsplan> tlist = AllVM.Datenbank.Trainingsplan.GetList(user.Nutzername);
                List<Ernährungsplan> elist = AllVM.Datenbank.Ernährungsplan.GetList(user.Nutzername);

                foreach (var item in news)
                {
                    AllVM.Datenbank.Feed.Delete(item);
                }

                foreach (var item in tlist)
                {
                    item.User = new User() { Nutzername = "fitness_bot" };
                    item.GeAendertAm = DateTime.Now;
                    AllVM.Datenbank.Trainingsplan.Edit(item);
                }

                foreach (var item in elist)
                {
                    item.User = new User() { Nutzername = "fitness_bot" };
                    item.GeAendertAm = DateTime.Now;
                    AllVM.Datenbank.Ernährungsplan.Edit(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Benutzer verfolgen
        /// </summary>
        /// <param name="profil">Benutzer, dem zu folgen ist</param>
        /// <param name="follower">Benutzer, der folgen möchte</param>
        /// <returns></returns>
        internal bool Follow(User profil, User follower)
        {
            string com = $"INSERT INTO User_Follows VALUES ('{profil.Nutzername}', '{follower.Nutzername}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
            return StaticDB.RunSQL(com);
        }

        /// <summary>
        /// Entfolgen
        /// </summary>
        /// <param name="profil">Benutzer, dem zu entfolgen ist</param>
        /// <param name="follower">Benutzer, der entfolgen möchte</param>
        /// <returns></returns>
        internal bool UnFollow(User profil, User follower)
        {
            string com = $"DELETE FROM User_Follows WHERE User_ID = '{profil.Nutzername}' AND Follow_ID = '{follower.Nutzername}'";
            return StaticDB.RunSQL(com);
        }

        /// <summary>
        /// Profilbild hochladen
        /// </summary>
        /// <param name="user">Benutzer</param>
        /// <param name="bild">Profilbild</param>
        /// <returns></returns>
        internal bool UploadProfilBild(User user, byte[] bild)
        {
            try
            {
                StaticDB.Connect();
                string com = $"SELECT * FROM User_Bild WHERE Nutzername = '{user.Nutzername}'";
                bool? existenz = StaticDB.CheckExistenz(com);

                if (existenz == null)
                {
                    return false;
                }
                else
                {
                    if (existenz == true)
                        com = $"UPDATE User_Bild SET Bild = @bildBytes WHERE Nutzername = '{user.Nutzername}'";
                    else if (existenz == false)
                        com = $"INSERT INTO User_Bild VALUES('{user.Nutzername}', @bildBytes)";

                    SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                    StaticDB.Connection.Open();
                    command.Parameters.Add("@bildBytes", System.Data.SqlDbType.VarBinary).Value = bild;
                    command.ExecuteNonQuery();
                    StaticDB.Connection.Close();
                    return true;
                }
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
    }
}
