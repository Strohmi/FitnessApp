using FitnessApp.Models.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FitnessApp.Models.DB
{
    public class DB_Ernährungsplan
    {
        public List<Ernährungsplan> GetErnährungsplaene(string Nutzername)
        {
            try
            {
                List<Ernährungsplan> ernährungsplaene = new List<Ernährungsplan>();
                StaticDB.Connect();
                string com = "SELECT base.ID, base.Titel , info.ErstelltVon, info.ErstelltAm, info.GeaendertAm, info.Kategorie " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Info as info " +
                             "ON base.ID = info.ID " +
                             $"WHERE info.ErstelltVon = '{Nutzername}'";
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    Ernährungsplan ernährungsplan = new Ernährungsplan()
                    {
                        ID = r.GetInt32(0),
                        Titel = r.GetString(1),
                        User = AllVM.Datenbank.User.GetByName(r.GetString(2)),
                        ErstelltAm = r.GetDateTime(3),
                        GeAendertAm = r.GetDateTime(4),
                        Bewertungen = AllVM.Datenbank.Ernährungsplan.GetBewertungen(r.GetInt32(0)),
                        MahlzeitenList = AllVM.Datenbank.Ernährungsplan.GetMahlzeiten(r.GetInt32(0)),
                        Kategorie = r.GetString(5)
                    };
                    ernährungsplaene.Add(ernährungsplan);
                }
                StaticDB.Connection.Close();
                return ernährungsplaene;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<Mahlzeiten> GetMahlzeiten(int ID)
        {
            try
            {
                List<Mahlzeiten> mahlzeitenList = new List<Mahlzeiten>();
                string com = "select mahl.ID, mahl.Nahrungsmittel, mahl.Menge, mahl.Einheit " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Link_BaseMahlzeiten as link " +
                             "ON base.ID = link.ID_Base " +
                             "INNER JOIN EP_Mahlzeiten as mahl " +
                             "ON link.ID_Mahlzeit = mahl.ID " +
                             $"WHERE base.ID = {ID}";
                SqlCommand sqlcommand = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = sqlcommand.ExecuteReader();
                while (r.Read())
                {
                    Mahlzeiten mahlzeit = new Mahlzeiten()
                    {
                        ID = r.GetInt32(0),
                        Nahrungsmittel = r.GetString(1),
                        Menge = r.GetDecimal(2),
                        Einheit = r.GetString(3)
                    };
                    mahlzeitenList.Add(mahlzeit);
                }
                StaticDB.Connection.Close();
                return mahlzeitenList;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool AddErnährungsplan(Ernährungsplan ernährungsplan)
        {
            try
            {
                StaticDB.Connection.Open();
                string com = $"INSERT INTO EP_Base (Titel) VALUES ('{ernährungsplan.Titel}'); SELECT CAST(SCOPE_IDENTITY() AS INT)";
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                int ID = (int)sqlCommand.ExecuteScalar();

                com = $"INSERT INTO EP_Info (ID, ErstelltAm, ErstelltVon, GeaendertAm) VALUES ({ID}, '{ernährungsplan.ErstelltAm}', '{ernährungsplan.User.Nutzername}', '{ernährungsplan.GeAendertAm}');";
                StaticDB.RunSQL(com);

                foreach (var mahlzeit in ernährungsplan.MahlzeitenList)
                {
                    string checkEx = $"SELECT * FROM EP_Mahlzeiten WHERE Nahrungsmittel='{mahlzeit.Nahrungsmittel}' AND Menge={mahlzeit.Menge.ToString().Replace(",", ".")} AND Einheit={mahlzeit.Einheit}";
                    if (StaticDB.CheckExistenz(checkEx) == true)
                    {
                        int mahlID = StaticDB.GetID(checkEx);
                        string comEpLink = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES({ID}, {mahlID})";
                        StaticDB.RunSQL(comEpLink);
                    }
                    else
                    {
                        com = $"INSERT INTO EP_Mahlzeiten (Nahrungsmittel, Menge, Einheit) VALUES ('{mahlzeit.Nahrungsmittel}', {mahlzeit.Menge.ToString().Replace(",", ".")}, {mahlzeit.Einheit}); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                        SqlCommand insertMahl = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int lastMahlID = (int)insertMahl.ExecuteScalar();
                        string comTpLink = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES({ID}, {lastMahlID})";
                        StaticDB.RunSQL(comTpLink);
                    }
                }
                StaticDB.Connection.Close();
                return true;
            }
            catch (Exception)
            {
                StaticDB.Connection.Close();
                return false;
            }

        }

        public bool Delete(Ernährungsplan ernährungsplan)
        {
            try
            {
                foreach (var item in ernährungsplan.Bewertungen)
                {
                    string command = $"DELETE FROM EP_Bewertung WHERE ID={item.ID}";
                    StaticDB.RunSQL(command);
                }
                string com = $"DELETE FROM EP_Base WHERE ID={ernährungsplan.ID}";
                StaticDB.RunSQL(com);
                return true;
            }
            catch (Exception)
            {
                StaticDB.Connection.Close();
                return false;
            }
        }
        public bool Edit(Ernährungsplan ernährungsplan)
        {
            try
            {
                string editEP_Base = $"UPDATE EP_Base SET Titel='{ernährungsplan.Titel}' WHERE ID={ernährungsplan.ID}";
                StaticDB.RunSQL(editEP_Base);
                foreach (var item in ernährungsplan.MahlzeitenList)
                {
                    string com = $"SELECT * FROM EP_Mahlzeiten WHERE Nahrungsmittel='{item.Nahrungsmittel}' AND Menge='{item.Menge.ToString().Replace(",", ".")}' AND Einheit='{item.Einheit}'";
                    if (StaticDB.CheckExistenz(com) == true)
                    {
                        SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int ID = (int)sqlCommand.ExecuteScalar();
                        if (StaticDB.CheckExistenz($"SELECT * FROM EP_Link_BaseMahlzeiten WHERE ID_Base={ernährungsplan.ID} AND ID_Mahlzeit={ID}") == true)
                        {
                            continue;
                        }
                        else
                        {
                            com = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES('{ernährungsplan.ID}', '{ID}')";
                            StaticDB.RunSQL(com);
                        }
                    }
                    else
                    {
                        com = $"INSERT INTO EP_Mahlzeiten (Nahrungsmittel, Menge, Einheit) VALUES ('{item.Nahrungsmittel}', '{item.Menge.ToString().Replace(",", ".")}', '{item.Einheit}')";
                        SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int ID = (int)sqlCommand.ExecuteScalar();
                        com = $"INSERT INTO TP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES('{ernährungsplan.ID}', '{ID}')";
                        StaticDB.RunSQL(com);
                    }
                }
                string editEP_Info = $"UPDATE EP_Info SET GeaendertAm='{DateTime.Now}' WHERE ID={ernährungsplan.ID}";
                StaticDB.RunSQL(editEP_Info);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<BewertungErnährungsplan> GetBewertungen(int ID)
        {
            try
            {
                List<BewertungErnährungsplan> bewertungsList = new List<BewertungErnährungsplan>();
                string com = "SELECT bew.ID, bew.[User], Bewertung " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Link_BaseBewertung as link " +
                             "ON base.ID = link.ID_EP_Base " +
                             "INNER JOIN EP_Bewertung as bew " +
                             "ON bew.ID = link.ID_EP_Bewertung " +
                            $"WHERE base.ID = {ID}";
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    BewertungErnährungsplan bewertung = new BewertungErnährungsplan()
                    {
                        ID = r.GetInt32(0),
                        Bewerter = AllVM.Datenbank.User.GetByName(r.GetString(1)),
                        Bewertung = r.GetString(2)
                    };
                    bewertungsList.Add(bewertung);
                }
                return bewertungsList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteBewertung(BewertungErnährungsplan bewertung)
        {
            try
            {
                string delBew = $"DELETE FROM EP_Bewertung WHERE ID={bewertung.ID}";
                StaticDB.RunSQL(delBew);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool AddBewertung(BewertungErnährungsplan bewertung, Ernährungsplan ernährungsplan)
        {
            try
            {
                StaticDB.Connect();
                string insertBew = $"INSERT INTO EP_Bewertung ([User], Bewertung) VALUES ('{bewertung.Bewerter.Nutzername}', '{bewertung.Bewertung}');" +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                StaticDB.Connection.Open();
                SqlCommand command = new SqlCommand(insertBew, StaticDB.Connection);
                int lastID = (int)command.ExecuteScalar();
                string insertLink = $"INSERT INTO EP_Link_BaseBewertung (ID_EP_Base, ID_EP_Bewertung) VALUES ({ernährungsplan.ID}, {lastID})";
                StaticDB.RunSQL(insertLink);

                ernährungsplan.Bewertungen.Add(bewertung);
                StaticDB.Connection.Close();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
