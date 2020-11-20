using FitnessApp.Models.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FitnessApp.Models.DB
{
    public class DB_Trainingsplan
    {
        public List<Trainingsplan> GetTrainingsplaene(string Nutzername)
        {
            Trainingsplan trainingsplan = null;
            List<Trainingsplan> trainingsplaene = new List<Trainingsplan>();
            StaticDatenbank.Connect();
            string com = "SELECT TP_Base.ID, TP_Base.Titel, TP_Info.ErstelltAM, TP_Info.ErstelltVon, TP_Info.GeandertAm " +
                         "FROM TP_Base " +
                         "INNER JOIN TP_Info " +
                         "ON TP_Base.ID = TP_Info.ID " +
                         $"WHERE TP_Info.ErstelltVon = '{Nutzername}'";

            SqlCommand sqlCommand = new SqlCommand(com, StaticDatenbank.Connection);
            StaticDatenbank.Connection.Open();
            IDataReader r = sqlCommand.ExecuteReader();
            while (r.Read())
            {
                trainingsplan = new Trainingsplan()
                {
                    ID = r.GetInt32(0),
                    Titel = r.GetString(1),
                    ErstelltAm = r.GetDateTime(2),
                    User = AllVM.Datenbank.User.GetByName(r.GetString(3)),
                    GeAendertAm = r.GetDateTime(4),
                    UebungList = AllVM.Datenbank.Trainingsplan.GetUebungen(r.GetInt32(0))
                };
                trainingsplaene.Add(trainingsplan);

            }
            StaticDatenbank.Connection.Close();
            return trainingsplaene;
        }
        public List<Uebung> GetUebungen(int ID)
        {
            List<Uebung> uebungen = new List<Uebung>();
            string sqlCommand = $"SELECT * FROM TP_Uebungen WHERE ID = {ID}";
            SqlCommand command = new SqlCommand(sqlCommand, StaticDatenbank.Connection);
            StaticDatenbank.Connection.Open();
            IDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                Uebung uebung = new Uebung()
                {
                    ID = r.GetInt32(0),
                    Name = r.GetString(1),
                    Gewicht = r.GetDecimal(2),
                    Repetition = r.GetInt32(3),
                    Sets = r.GetInt32(4)
                };
                uebungen.Add(uebung);
            }
            StaticDatenbank.Connection.Close();
            return uebungen;
        }
        public bool AddTrainingsplan(Trainingsplan trainingsplan)
        {
            try
            {
                StaticDatenbank.Connect();
                string com = $"INSERT INTO TP_Base (Titel) values ('{trainingsplan.Titel}'); " +
                             "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                SqlCommand command = new SqlCommand(com, StaticDatenbank.Connection);

                StaticDatenbank.Connection.Open();
                //transaction = StaticDatenbank.Connection.BeginTransaction();
                int lastID = (int)command.ExecuteScalar();

                com = $"INSERT INTO TP_Info (ID, ErstelltAm, ErstelltVon, GeandertAm) VALUES ({lastID}, '{trainingsplan.ErstelltAm}', '{trainingsplan.User.Nutzername}', '{trainingsplan.GeAendertAm}');";
                StaticDatenbank.RunSQL(com);

                foreach (var uebung in trainingsplan.UebungList)
                {
                    com = $"INSERT INTO TP_Uebungen (ID, Name, Gewicht, Repetition, Sets) VALUES ({lastID}, '{uebung.Name}', {uebung.Gewicht}, {uebung.Repetition}, {uebung.Sets};";
                    StaticDatenbank.RunSQL(com);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public bool Edit(Trainingsplan trainingsplan)
        {


            return true;
        }
        public bool Delete(Trainingsplan trainingsplan)
        {


            return true;
        }
    }
}
