using FitnessApp.Models.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace FitnessApp.Models.DB
{
    public class DB_Ernährungsplan
    {
        public List<Ernährungsplan> GetList(string Nutzername = null)
        {
            try
            {
                StaticDB.Connect();
                string com = null;
                List<Ernährungsplan> ernährungsplaene = new List<Ernährungsplan>();

                if (string.IsNullOrWhiteSpace(Nutzername))
                {
                    com = "SELECT base.ID, base.Titel , info.ErstelltVon, info.ErstelltAm, info.GeaendertAm, info.Kategorie " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Info as info " +
                             "ON base.ID = info.ID ";
                }
                else
                {
                    com = "SELECT base.ID, base.Titel , info.ErstelltVon, info.ErstelltAm, info.GeaendertAm, info.Kategorie " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Info as info " +
                             "ON base.ID = info.ID " +
                             $"WHERE info.ErstelltVon = '{Nutzername}'";
                }

                StaticDB.Connection.Open();
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    Ernährungsplan ernährungsplan = new Ernährungsplan()
                    {
                        ID = r.GetInt32(0),
                        Titel = r.GetString(1),
                        Ersteller = new User() { Nutzername = r.GetString(2) },
                        ErstelltAm = r.GetDateTime(3),
                        Kategorie = r.GetString(5),
                    };
                    ernährungsplaene.Add(ernährungsplan);
                    if (!r.IsDBNull(4))
                        ernährungsplan.GeAendertAm = r.GetDateTime(4);
                }
                StaticDB.Connection.Close();
                foreach (var item in ernährungsplaene)
                {
                    item.Ersteller = AllVM.Datenbank.User.GetByName(item.Ersteller.Nutzername);
                    item.Bewertungen = AllVM.Datenbank.Ernährungsplan.GetBewertungen(item.ID);
                    item.MahlzeitenList = AllVM.Datenbank.Ernährungsplan.GetMahlzeiten(item.ID);
                    item.DurchBewertung = AllVM.Datenbank.Ernährungsplan.GetAvgBewertung(item.ID);
                }
                return ernährungsplaene;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }
        internal Ernährungsplan GetByID(int iD)
        {
            try
            {
                StaticDB.Connect();
                var ePlan = new Ernährungsplan();
                string com = "SELECT base.ID, base.Titel , info.ErstelltVon, info.ErstelltAm, info.GeaendertAm, info.Kategorie " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Info as info " +
                             "ON base.ID = info.ID " +
                             $"WHERE base.ID = '{iD}'";
                StaticDB.Connection.Open();
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    ePlan = new Ernährungsplan()
                    {
                        ID = r.GetInt32(0),
                        Titel = r.GetString(1),
                        Ersteller = new User() { Nutzername = r.GetString(2) },
                        ErstelltAm = r.GetDateTime(3),
                        Kategorie = r.GetString(5)
                    };

                    if (!r.IsDBNull(4))
                        ePlan.GeAendertAm = r.GetDateTime(4);
                }
                StaticDB.Connection.Close();

                ePlan.Ersteller = AllVM.Datenbank.User.GetByName(ePlan.Ersteller.Nutzername);
                ePlan.Bewertungen = AllVM.Datenbank.Ernährungsplan.GetBewertungen(ePlan.ID);
                ePlan.MahlzeitenList = AllVM.Datenbank.Ernährungsplan.GetMahlzeiten(ePlan.ID);
                ePlan.DurchBewertung = AllVM.Datenbank.Ernährungsplan.GetAvgBewertung(ePlan.ID);

                return ePlan;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }

        public List<Mahlzeiten> GetMahlzeiten(int ID)
        {
            try
            {
                StaticDB.Connect();
                List<Mahlzeiten> mahlzeitenList = new List<Mahlzeiten>();
                string com = "select mahl.ID, mahl.Nahrungsmittel, mahl.Menge, mahl.Einheit " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Link_BaseMahlzeiten as link " +
                             "ON base.ID = link.ID_Base " +
                             "INNER JOIN EP_Mahlzeiten as mahl " +
                             "ON link.ID_Mahlzeit = mahl.ID " +
                             $"WHERE base.ID = {ID}";
                StaticDB.Connection.Open();
                SqlCommand sqlcommand = new SqlCommand(com, StaticDB.Connection);
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
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }

        }

        public bool AddErnährungsplan(Ernährungsplan ernährungsplan)
        {
            try
            {
                StaticDB.Connect();
                StaticDB.Connection.Open();
                string com = $"INSERT INTO EP_Base (Titel) VALUES ('{ernährungsplan.Titel}'); SELECT CAST(SCOPE_IDENTITY() AS INT)";
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                int ID = (int)sqlCommand.ExecuteScalar();
                StaticDB.Connection.Close();

                if (ernährungsplan.GeAendertAm != default)
                {
                    com = $"INSERT INTO EP_Info (ID, ErstelltAm, ErstelltVon, GeaendertAm, Kategorie) VALUES ({ID}, '{ernährungsplan.ErstelltAm:yyyy-dd-MM HH:mm:ss}', '{ernährungsplan.Ersteller.Nutzername}', '{ernährungsplan.GeAendertAm:yyyy-dd-MM HH:mm:ss}', '{ernährungsplan.Kategorie}');";
                }
                else
                {
                    com = $"INSERT INTO EP_Info (ID, ErstelltAm, ErstelltVon, Kategorie) VALUES ({ID}, '{ernährungsplan.ErstelltAm:yyyy-dd-MM HH:mm:ss}', '{ernährungsplan.Ersteller.Nutzername}', '{ernährungsplan.Kategorie}');";
                }

                bool result = StaticDB.RunSQL(com);
                if (result == false)
                {
                    com = $"DELETE FROM EP_Base WHERE ID = '{ID}'";
                    StaticDB.RunSQL(com);
                    return false;
                }

                foreach (var mahlzeit in ernährungsplan.MahlzeitenList)
                {
                    string checkEx = $"SELECT * FROM EP_Mahlzeiten WHERE Nahrungsmittel='{mahlzeit.Nahrungsmittel}' AND Menge={mahlzeit.Menge.ToString().Replace(",", ".")} AND Einheit='{mahlzeit.Einheit}'";
                    if (StaticDB.CheckExistenz(checkEx) == true)
                    {
                        int mahlID = StaticDB.GetID(checkEx);
                        string comEpLink = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES({ID}, {mahlID})";
                        StaticDB.RunSQL(comEpLink);
                    }
                    else
                    {
                        com = $"INSERT INTO EP_Mahlzeiten (Nahrungsmittel, Menge, Einheit) VALUES ('{mahlzeit.Nahrungsmittel}', {mahlzeit.Menge.ToString().Replace(",", ".")}, '{mahlzeit.Einheit}'); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                        SqlCommand insertMahl = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int lastMahlID = (int)insertMahl.ExecuteScalar();
                        StaticDB.Connection.Close();
                        string comTpLink = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES({ID}, {lastMahlID})";
                        StaticDB.RunSQL(comTpLink);
                    }
                }
                StaticDB.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
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
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return false;
            }
        }
        public bool Edit(Ernährungsplan ernährungsplan)
        {
            try
            {
                StaticDB.Connect();
                string editEP_Base = $"UPDATE EP_Base SET Titel='{ernährungsplan.Titel}' WHERE ID={ernährungsplan.ID}";
                StaticDB.RunSQL(editEP_Base);
                string delLink = $"DELETE FROM EP_Link_BaseMahlzeiten WHERE ID_Base={ernährungsplan.ID}";
                StaticDB.RunSQL(delLink);
                foreach (var item in ernährungsplan.MahlzeitenList)
                {
                    string com = $"SELECT * FROM EP_Mahlzeiten WHERE Nahrungsmittel='{item.Nahrungsmittel}' AND Menge='{item.Menge.ToString().Replace(",", ".")}' AND Einheit='{item.Einheit}'";
                    if (StaticDB.CheckExistenz(com) == true)
                    {
                        SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int ID = (int)sqlCommand.ExecuteScalar();
                        StaticDB.Connection.Close();

                        com = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES('{ernährungsplan.ID}', '{ID}')";
                        StaticDB.RunSQL(com);
                    }
                    else
                    {
                        com = $"INSERT INTO EP_Mahlzeiten (Nahrungsmittel, Menge, Einheit) VALUES ('{item.Nahrungsmittel}', '{item.Menge.ToString().Replace(",", ".")}', '{item.Einheit}');" +
                              $"SELECT CAST(SCOPE_IDENTITY() AS INT)";
                        StaticDB.Connection.Open();
                        SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                        int ID = (int)sqlCommand.ExecuteScalar();
                        StaticDB.Connection.Close();
                        com = $"INSERT INTO EP_Link_BaseMahlzeiten (ID_Base, ID_Mahlzeit) VALUES('{ernährungsplan.ID}', '{ID}')";
                        StaticDB.RunSQL(com);
                    }
                }
                string editEP_Info = $"UPDATE EP_Info SET GeaendertAm='{DateTime.Now}' WHERE ID={ernährungsplan.ID}";
                StaticDB.RunSQL(editEP_Info);

                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return false;
            }
        }
        public List<Bewertung> GetBewertungen(int ID)
        {
            try
            {
                StaticDB.Connect();
                List<Bewertung> bewertungsList = new List<Bewertung>();
                string com = "SELECT bew.ID, bew.[User], Bewertung " +
                             "FROM EP_Base as base " +
                             "INNER JOIN EP_Link_BaseBewertung as link " +
                             "ON base.ID = link.ID_EP_Base " +
                             "INNER JOIN EP_Bewertung as bew " +
                             "ON bew.ID = link.ID_EP_Bewertung " +
                            $"WHERE base.ID = {ID}";

                StaticDB.Connection.Open();
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);

                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    Bewertung bewertung = new Bewertung()
                    {
                        ID = r.GetInt32(0),
                        Bewerter = new User() { Nutzername = r.GetString(1) },
                        Rating = r.GetInt32(2)
                    };
                    bewertungsList.Add(bewertung);
                }
                StaticDB.Connection.Close();
                foreach (var item in bewertungsList)
                {
                    item.Bewerter = AllVM.Datenbank.User.GetByName(item.Bewerter.Nutzername);
                }
                return bewertungsList;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }

        public decimal GetAvgBewertung(int ID)
        {
            try
            {
                string durcchBew = "SELECT AVG(bew.Bewertung) " +
                                   "FROM EP_Base as base " +
                                   "INNER JOIN EP_Link_BaseBewertung as link " +
                                   "ON base.ID = link.ID_EP_Base " +
                                   "INNER JOIN EP_Bewertung as bew " +
                                   "ON bew.ID = link.ID_EP_Bewertung " +
                                   $"WHERE base.ID = {ID}";
                SqlCommand command = new SqlCommand(durcchBew, StaticDB.Connection);
                StaticDB.Connection.Open();
                var x = command.ExecuteScalar();
                StaticDB.Connection.Close();

                if (x != null && x.GetType() != typeof(System.DBNull))
                    return decimal.Parse(x.ToString());
                else
                    return -1;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return -2;
            }
        }
        public bool DeleteBewertung(Bewertung bewertung)
        {
            try
            {
                string delBew = $"DELETE FROM EP_Bewertung WHERE ID={bewertung.ID}";
                StaticDB.RunSQL(delBew);
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return false;
            }
        }
        public bool AddBewertung(Bewertung bewertung, Ernährungsplan ernährungsplan)
        {
            try
            {
                StaticDB.Connect();
                string insertBew = $"INSERT INTO EP_Bewertung ([User], Bewertung) VALUES ('{bewertung.Bewerter.Nutzername}', '{bewertung.Rating}');" +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                StaticDB.Connection.Open();
                SqlCommand command = new SqlCommand(insertBew, StaticDB.Connection);
                int lastID = (int)command.ExecuteScalar();
                StaticDB.Connection.Close();
                string insertLink = $"INSERT INTO EP_Link_BaseBewertung (ID_EP_Base, ID_EP_Bewertung) VALUES ({ernährungsplan.ID}, {lastID})";
                StaticDB.RunSQL(insertLink);

                ernährungsplan.Bewertungen.Add(bewertung);
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return false;
            }
        }
        public List<string> GetCategories()
        {
            try
            {
                StaticDB.Connect();
                List<string> categories = new List<string>();
                string com = "SELECT * FROM EP_Kategorien";
                StaticDB.Connection.Open();
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    categories.Add(r.GetString(0));
                }
                StaticDB.Connection.Close();
                return categories;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }
        public List<string> GetUnits()
        {
            try
            {
                List<string> units = new List<string>();
                string com = "SELECT * FROM EP_Einheiten";
                StaticDB.Connect();
                StaticDB.Connection.Open();
                SqlCommand sql = new SqlCommand(com, StaticDB.Connection);
                var r = sql.ExecuteReader();
                while (r.Read())
                {
                    units.Add(r.GetString(0));
                }
                StaticDB.Connection.Close();
                return units;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                if (StaticDB.Connection != null)
                    if (StaticDB.Connection.State != ConnectionState.Closed)
                        StaticDB.Connection.Close();
                return null;
            }
        }
    }
}
