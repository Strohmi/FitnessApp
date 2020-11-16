﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_User
    {
        internal List<User> GetList()
        {
            try
            {
                List<User> list = new List<User>();
                StaticDatenbank.Connect();

                string com = "SELECT Nutzername " +
                             "FROM User_Base";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    User user = new User()
                    {
                        Nutzername = r.GetString(0)
                    };
                    list.Add(user);
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

        public User GetByName(string nutzername)
        {
            try
            {
                User user = null;
                StaticDatenbank.Connect();

                string com = "SELECT base.Nutzername, info.ErstelltAm, info.Infotext, bild.Bild " +
                             "FROM User_Base AS base " +
                             "INNER JOIN User_Info AS info " +
                             "ON info.ID = base.Nutzername " +
                             "LEFT JOIN User_Bild AS bild " +
                             "ON bild.ID = base.Nutzername " +
                             $"WHERE base.Nutzername = '{nutzername}';";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
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

                StaticDatenbank.Connection.Close();

                user.AnzahlFollower = GetAnzahlFollower(user.Nutzername);
                //user.AnzahlFollower = 999999;

                return user;
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

        private int GetAnzahlFollower(string nutzername)
        {
            try
            {
                string com = $"SELECT COUNT(*) FROM User_Follows WHERE User_ID = '{nutzername}'";
                StaticDatenbank.Connect();
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                object result = command.ExecuteScalar();
                StaticDatenbank.Connection.Close();

                if (result != null)
                    return (int)result;
                else
                    return -1;
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

        internal bool Update(User user)
        {
            string com = $"UPDATE User_Info SET " +
                         $"GeandertAm = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', " +
                         $"Infotext = '{user.InfoText}' " +
                         $"WHERE ID = '{user.Nutzername}';";

            bool result = StaticDatenbank.RunSQL(com);

            if (result == false)
                return result;

            try
            {
                com = $"UPDATE User_Bild SET " +
                      $"Bild = @bild " +
                      $"WHERE ID = '{user.Nutzername}';";

                StaticDatenbank.Connect();
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                command.Parameters.Add("@bild", System.Data.SqlDbType.VarBinary).Value = user.ProfilBild;
                command.ExecuteNonQuery();
                StaticDatenbank.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDatenbank.Connection != null)
                    if (StaticDatenbank.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDatenbank.Connection.Close();
                return false;
            }
        }

        internal List<User> GetFollows(string profil)
        {
            try
            {
                List<User> list = new List<User>();
                StaticDatenbank.Connect();

                string com = "SELECT Follow_ID " +
                             "FROM User_Follows " +
                             $"WHERE User_ID = '{profil}';";
                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                StaticDatenbank.Connection.Open();
                var r = command.ExecuteReader();

                while (r.Read())
                {
                    User user = new User() { Nutzername = r.GetString(0) };
                    list.Add(user);
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

        internal bool Delete(User user)
        {
            throw new NotImplementedException();
        }

        internal bool Follow(User profil, User follower)
        {
            string com = $"INSERT INTO User_Follows VALUES ('{profil.Nutzername}', '{follower.Nutzername}')";
            return StaticDatenbank.RunSQL(com);
        }

        internal bool UnFollow(User profil, User follower)
        {
            string com = $"DELETE FROM User_Follows WHERE User_ID = '{profil.Nutzername}' AND Follow_ID = '{follower.Nutzername}'";
            return StaticDatenbank.RunSQL(com);
        }

        internal bool UploadProfilBild(User user, byte[] bild)
        {
            try
            {
                StaticDatenbank.Connect();
                string com = $"SELECT * FROM User_Bild WHERE ID = '{user.Nutzername}'";
                bool? existenz = StaticDatenbank.CheckExistenz(com);

                if (existenz == null)
                {
                    return false;
                }
                else
                {
                    if (existenz == true)
                        com = $"UPDATE User_Bild SET Bild = @bildBytes WHERE ID = '{user.Nutzername}'";
                    else if (existenz == false)
                        com = $"INSERT INTO User_Bild VALUES('{user.Nutzername}', @bildBytes)";

                    SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);
                    StaticDatenbank.Connection.Open();
                    command.Parameters.Add("@bildBytes", System.Data.SqlDbType.VarBinary).Value = bild;
                    command.ExecuteNonQuery();
                    StaticDatenbank.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDatenbank.Connection != null)
                    if (StaticDatenbank.Connection.State != System.Data.ConnectionState.Closed)
                        StaticDatenbank.Connection.Close();
                return false;
            }
        }
    }
}
