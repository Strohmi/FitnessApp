using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FitnessApp.Models
{
    public class DB_User
    {
        public User Get(string nutzername)
        {
            try
            {
                User user = null;
                StaticDatenbank.Connect();

                string com = "SELECT base.Nutzername, info.ErstelltAm, info.Infotext, info.Status " +
                             "FROM User_Base AS base " +
                             "INNER JOIN User_Info AS info " +
                             "ON info.ID = base.Nutzername " +
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
                        InfoText = r.GetString(2),
                        Status = r.GetString(3)
                    };
                }

                StaticDatenbank.Connection.Close();
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
                        com = "UPDATE User_Bild SET Bild = @bildBytes WHERE ID = '{user.Nutzername}'";
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
