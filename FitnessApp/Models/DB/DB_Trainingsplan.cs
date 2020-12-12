﻿using FitnessApp.Models.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FitnessApp.Models.DB
{
    public class DB_Trainingsplan
    {
        /// <summary>
        /// Liest die Trainingspläne für einen Nutzer aus der Datenbank aus.
        /// </summary>
        /// <param name="Nutzername">Nutzername von dem Trainingspläne geladen werden sollen (string)</param>
        /// <returns>Liste von Trainingsplänen</returns>
        public List<Trainingsplan> GetList(string Nutzername = null)
        {
            try
            {
                string com = null;
                Trainingsplan trainingsplan = null;
                List<Trainingsplan> trainingsplaene = new List<Trainingsplan>();
                StaticDB.Connect();

                if (string.IsNullOrWhiteSpace(Nutzername))
                {
                    com = "SELECT TP_Base.ID, TP_Base.Titel, TP_Info.ErstelltAM, TP_Info.ErstelltVon, TP_Info.GeaendertAm, TP_Info.Kategorie " +
                          "FROM TP_Base " +
                          "INNER JOIN TP_Info " +
                          "ON TP_Base.ID = TP_Info.ID ";
                }
                else
                {
                    com = "SELECT TP_Base.ID, TP_Base.Titel, TP_Info.ErstelltAM, TP_Info.ErstelltVon, TP_Info.GeaendertAm, TP_Info.Kategorie " +
                          "FROM TP_Base " +
                          "INNER JOIN TP_Info " +
                          "ON TP_Base.ID = TP_Info.ID " +
                         $"WHERE TP_Info.ErstelltVon = '{Nutzername}'";
                }

                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                StaticDB.Connection.Open();
                IDataReader r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    trainingsplan = new Trainingsplan()
                    {
                        ID = r.GetInt32(0),
                        Titel = r.GetString(1),
                        ErstelltAm = r.GetDateTime(2),
                        User = new User() { Nutzername = r.GetString(3) },
                        Kategorie = r.GetString(5),
                    };

                    if (!r.IsDBNull(4))
                        trainingsplan.GeAendertAm = r.GetDateTime(4);

                    trainingsplaene.Add(trainingsplan);

                }
                StaticDB.Connection.Close();

                foreach (var item in trainingsplaene)
                {
                    item.User = AllVM.Datenbank.User.GetByName(item.User.Nutzername);
                    item.UebungList = AllVM.Datenbank.Trainingsplan.GetUebungen(item.ID);
                    item.Bewertungen = AllVM.Datenbank.Trainingsplan.GetBewertungen(item.ID);
                    item.DurchBewertung = AllVM.Datenbank.Trainingsplan.GetAvgBewertung(item.ID);
                }
                return trainingsplaene;
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

        /// <summary>
        /// Lädt die zu einem Trainingsplan zugehörigen Übungen aus der Datenbank.
        /// </summary>
        /// <param name="ID">ID des Trainingsplans (Integer)</param>
        /// <returns>Liste der Übungen</returns>
        public List<Uebung> GetUebungen(int ID)
        {
            try
            {
                List<Uebung> uebungen = new List<Uebung>();
                string sqlCommand = "SELECT ueb.ID, ueb.Name, ueb.Gewicht, ueb.Repetition, ueb.Sets " +
                                    "FROM TP_Base as base " +
                                    "INNER JOIN TP_Link_BaseUebung as link " +
                                    "ON base.ID = link.ID_Base " +
                                    "INNER JOIN TP_Uebungen as ueb " +
                                    "ON ueb.ID = link.ID_Uebung " +
                                   $"WHERE base.ID = {ID}";
                StaticDB.Connection.Open();
                SqlCommand command = new SqlCommand(sqlCommand, StaticDB.Connection);
                IDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    Uebung uebung = new Uebung()
                    {
                        ID = r.GetInt32(0),
                        Name = r.GetString(1),
                        Gewicht = r.GetDecimal(2),
                        Wiederholungen = r.GetInt32(3),
                        Sätze = r.GetInt32(4)
                    };
                    uebungen.Add(uebung);
                }

                StaticDB.Connection.Close();
                return uebungen;
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

        /// <summary>
        /// Fügt einen Trainingsplan zu der Datenbank hinzu.
        /// </summary>
        /// <param name="trainingsplan">Objekt vom Typ Trainingsplan</param>
        /// <returns>Gibt bei erfolgreichem Ausführen true zurück und bei einem Fehler false</returns>
        public bool AddTrainingsplan(Trainingsplan trainingsplan)
        {
            try
            {
                StaticDB.Connect();
                string com = $"INSERT INTO TP_Base (Titel) values ('{trainingsplan.Titel}'); " +
                             "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                SqlCommand command = new SqlCommand(com, StaticDB.Connection);

                StaticDB.Connection.Open();

                int lastID = (int)command.ExecuteScalar();
                StaticDB.Connection.Close();
                com = $"INSERT INTO TP_Info (ID, ErstelltAm, ErstelltVon, GeaendertAm, Kategorie) VALUES ({lastID}, '{trainingsplan.ErstelltAm}', '{trainingsplan.User.Nutzername}', '{trainingsplan.GeAendertAm}', '{trainingsplan.Kategorie}');";
                StaticDB.RunSQL(com);

                foreach (var uebung in trainingsplan.UebungList)
                {
                    string checkEx = $"SELECT * FROM TP_Uebungen WHERE Name='{uebung.Name}' AND Gewicht={uebung.Gewicht.ToString().Replace(",", ".")} AND Repetition={uebung.Wiederholungen} AND Sets={uebung.Sätze}";
                    if (StaticDB.CheckExistenz(checkEx) == true)
                    {
                        int uebID = StaticDB.GetID(checkEx);
                        string comTpLink = $"INSERT INTO TP_Link_BaseUebung (ID_Base, ID_Uebung) VALUES({lastID}, {uebID})";
                        StaticDB.RunSQL(comTpLink);
                    }
                    else
                    {
                        com = $"INSERT INTO TP_Uebungen (Name, Gewicht, Repetition, Sets) VALUES ('{uebung.Name}', {uebung.Gewicht.ToString().Replace(",", ".")}, {uebung.Wiederholungen}, {uebung.Sätze}); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                        SqlCommand insertUeb = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int lastUebID = (int)insertUeb.ExecuteScalar();
                        StaticDB.Connection.Close();
                        string comTpLink = $"INSERT INTO TP_Link_BaseUebung (ID_Base, ID_Uebung) VALUES({lastID}, {lastUebID})";
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

        // in arbeit
        /// <summary>
        /// Ändert einen Trainingsplan in der Datenbank
        /// </summary>
        /// <param name="trainingsplan">Nimmt den bearbeiteten Trainingsplan entgegen (Typ Trainingsplan)</param>
        /// <returns>Gibt bei erfolgreichem Ausführen true zurück und bei einem Fehler false</returns>
        public bool Edit(Trainingsplan trainingsplan)
        {
            try
            {
                StaticDB.Connect();
                string editTP_Base = $"UPDATE TP_Base SET Titel = '{trainingsplan.Titel}' WHERE ID={trainingsplan.ID}";
                StaticDB.RunSQL(editTP_Base);
                foreach (var item in trainingsplan.UebungList)
                {
                    string com = $"SELECT * FROM TP_Uebungen WHERE Name='{item.Name}' AND Gewicht='{item.Gewicht.ToString().Replace(",", ".")}' AND Repetition={item.Wiederholungen} AND Sets={item.Sätze}";
                    if (StaticDB.CheckExistenz(com) == true)
                    {
                        SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                        StaticDB.Connection.Open();
                        int ID = (int)sqlCommand.ExecuteScalar();
                        StaticDB.Connection.Close();
                        if (StaticDB.CheckExistenz($"SELECT * FROM TP_Link_BaseUebung WHERE ID_Base='{trainingsplan.ID}' AND ID_Uebung='{ID}'") == true)
                        {
                            continue;
                        }
                        else
                        {
                            com = $"INSERT INTO TP_Link_BaseUebung (ID_Base, ID_Uebung) VALUES('{trainingsplan.ID}', '{ID}')";
                            StaticDB.RunSQL(com);
                        }
                    }
                    else
                    {
                        com = $"INSERT INTO TP_Uebungen (Name, Gewicht, Repetition, Sets) VALUES ('{item.Name}', '{item.Gewicht.ToString().Replace(",", ".")}', '{item.Wiederholungen}', '{item.Sätze}'); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                        StaticDB.Connection.Open();
                        SqlCommand command = new SqlCommand(com, StaticDB.Connection);
                        int ID = (int)command.ExecuteScalar();
                        StaticDB.Connection.Close();
                        com = $"INSERT INTO TP_Link_BaseUebung (ID_Base, ID_Uebung) VALUES('{trainingsplan.ID}', '{ID}')";
                        StaticDB.RunSQL(com);
                    }
                }
                string editTP_Info = $"UPDATE TP_Info SET GeaendertAm='{DateTime.Now}' WHERE ID={trainingsplan.ID}";
                StaticDB.RunSQL(editTP_Info);
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

        /// <summary>
        /// Fügt eine Bewertung zu einem Trainingsplan in der Datenbank hinzu
        /// </summary>
        /// <param name="bewertung">Hinzuzufügende Bewertung (Typ BewertungTrainingsplan)</param>
        /// <param name="trainingsplan">Zur Bewertung zugehöriger Trainingsplan (Typ Trainingsplan)</param>
        /// <returns>Gibt bei erfolgreichem Ausführen true zurück und bei einem Fehler false</returns>
        public bool AddBewertung(BewertungTrainingpsplan bewertung, Trainingsplan trainingsplan)
        {
            try
            {
                StaticDB.Connect();
                string insertBew = $"INSERT INTO TP_Bewertung ([User], Bewertung) VALUES ('{bewertung.Bewerter.Nutzername}', '{bewertung.Bewertung}');" +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                StaticDB.Connection.Open();
                SqlCommand command = new SqlCommand(insertBew, StaticDB.Connection);
                int lastID = (int)command.ExecuteScalar();
                StaticDB.Connection.Close();
                string insertLink = $"INSERT INTO TP_Link_BaseBewertung (ID_TP_Base, ID_TP_Bewertung) VALUES ({trainingsplan.ID}, {lastID})";
                StaticDB.RunSQL(insertLink);

                trainingsplan.Bewertungen.Add(bewertung);
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

        /// <summary>
        /// Löscht eine Bewertung aus der Datenbank.
        /// </summary>
        /// <param name="bewertung">Zu löschende Bewertung (Typ BewertungTrainingsplan)</param>
        /// <returns>Gibt bei erfolgreichem Ausführen true zurück und bei einem Fehler false</returns>
        public bool DeleteBewertung(BewertungTrainingpsplan bewertung)
        {
            try
            {
                string delBew = $"DELETE FROM TP_Bewertung WHERE ID={bewertung.ID}";
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
        /// <summary>
        /// Lädt eine Liste der Bewertungen zu einem Trainingsplan aus der Datenbank.
        /// </summary>
        /// <param name="ID">Nimmt die ID eines Trainingsplans entgegen (Integer)</param>
        /// <returns>Gibt eine Liste von Bewertungen zurück</returns>
        public List<BewertungTrainingpsplan> GetBewertungen(int ID)
        {
            try
            {
                StaticDB.Connect();
                List<BewertungTrainingpsplan> bewertungsList = new List<BewertungTrainingpsplan>();
                string com = "SELECT bew.ID, bew.[User], Bewertung " +
                             "FROM TP_Base as base " +
                             "INNER JOIN TP_Link_BaseBewertung as link " +
                             "ON base.ID = link.ID_TP_Base " +
                             "INNER JOIN TP_Bewertung as bew " +
                             "ON bew.ID = link.ID_TP_Bewertung " +
                             $"WHERE base.ID = {ID}";

                StaticDB.Connection.Open();
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);

                var r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    BewertungTrainingpsplan bewertung = new BewertungTrainingpsplan()
                    {
                        ID = r.GetInt32(0),
                        Bewerter = new User() { Nutzername = r.GetString(1) },
                        Bewertung = r.GetInt32(2)
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
                StaticDB.Connect();
                string durcchBew = "SELECT AVG(bew.Bewertung) " +
                                   "FROM TP_Base as base " +
                                   "INNER JOIN TP_Link_BaseBewertung as link " +
                                   "ON base.ID = link.ID_TP_Base " +
                                   "INNER JOIN TP_Bewertung as bew " +
                                   "ON bew.ID = link.ID_TP_Bewertung " +
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

        /// <summary>
        /// Löscht einen Trainingsplan, inklusive der Bewertungen, aus der Datenbank.
        /// </summary>
        /// <param name="trainingsplan">Nimmt den zu löschenden Trainingsplan entgegen (Typ Trainingsplan)</param>
        /// <returns>Gibt bei erfolgreichem Ausführen true zurück und bei einem Fehler false</returns>
        public bool Delete(Trainingsplan trainingsplan)
        {
            try
            {
                foreach (var item in trainingsplan.Bewertungen)
                {
                    string command = $"DELETE FROM TP_Bewertung WHERE ID={item.ID}";
                    StaticDB.RunSQL(command);
                }
                string com = $"DELETE FROM TP_Base WHERE ID={trainingsplan.ID}";
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

        public Trainingsplan GetByID(int ID)
        {
            try
            {
                StaticDB.Connect();
                Trainingsplan trainingsplan = new Trainingsplan();
                string com = "SELECT TP_Base.ID, TP_Base.Titel, TP_Info.ErstelltAM, TP_Info.ErstelltVon, TP_Info.GeaendertAm, TP_Info.Kategorie " +
                             "FROM TP_Base " +
                             "INNER JOIN TP_Info " +
                             "ON TP_Base.ID = TP_Info.ID " +
                             $"WHERE TP_Info.ID = '{ID}'";

                StaticDB.Connection.Open();
                SqlCommand sqlCommand = new SqlCommand(com, StaticDB.Connection);
                IDataReader r = sqlCommand.ExecuteReader();
                while (r.Read())
                {
                    trainingsplan = new Trainingsplan()
                    {
                        ID = r.GetInt32(0),
                        Titel = r.GetString(1),
                        ErstelltAm = r.GetDateTime(2),
                        User = new User() { Nutzername = r.GetString(3) },
                        GeAendertAm = r.GetDateTime(4),
                        Kategorie = r.GetString(5),
                    };
                }

                StaticDB.Connection.Close();

                trainingsplan.User = AllVM.Datenbank.User.GetByName(trainingsplan.User.Nutzername);
                trainingsplan.UebungList = AllVM.Datenbank.Trainingsplan.GetUebungen(trainingsplan.ID);
                trainingsplan.Bewertungen = AllVM.Datenbank.Trainingsplan.GetBewertungen(trainingsplan.ID);
                trainingsplan.DurchBewertung = AllVM.Datenbank.Trainingsplan.GetAvgBewertung(trainingsplan.ID);
                return trainingsplan;
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
        public List<string> GetCategories()
        {
            try
            {
                List<string> categories = new List<string>();
                string com = "SELECT * FROM TP_Kategorien";
                StaticDB.Connect();
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
    }
}
