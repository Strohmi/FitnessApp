using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FitnessApp.Models.General;
using FitnessApp.ViewModels;

namespace FitnessApp.Models
{
    public class DB_User
    {
        internal List<User> GetList()
        {
            try
            {
                List<User> list = new List<User>();
                StaticDB.Connect();

                string com = "SELECT Nutzername " +
                             "FROM User_Base";
                SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    User user = new User()
                    {
                        Nutzername = r.GetString(0)
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

        internal bool Insert(User user)
        {
            string com = $"";
            return StaticDB.RunSQL(com);
        }

        public User GetByName(string nutzername)
        {
            try
            {
                User user = null;
                StaticDB.Connect();

                string com = "SELECT base.Nutzername, info.ErstelltAm, info.Infotext, bild.Bild " +
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
                        InfoText = r.GetString(2)
                    };

                    if (!r.IsDBNull(3))
                        user.ProfilBild = (byte[])r[3];
                }

                StaticDB.Connection.Close();

                user.AnzahlFollower = GetAnzahlFollower(user.Nutzername);
                //user.AnzahlFollower = 999999;

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

        internal bool Update(User user)
        {
            string com = $"UPDATE User_Info SET " +
                         $"GeaendertAm = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', " +
                         $"Infotext = '{user.InfoText}' " +
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

        internal bool Delete(User user)
        {
            throw new NotImplementedException();
        }

        internal bool Follow(User profil, User follower)
        {
            string com = $"INSERT INTO User_Follows VALUES ('{profil.Nutzername}', '{follower.Nutzername}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
            return StaticDB.RunSQL(com);
        }

        internal bool UnFollow(User profil, User follower)
        {
            string com = $"DELETE FROM User_Follows WHERE User_ID = '{profil.Nutzername}' AND Follow_ID = '{follower.Nutzername}'";
            return StaticDB.RunSQL(com);
        }

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
