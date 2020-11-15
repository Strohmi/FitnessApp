using System;
namespace FitnessApp
{
    public class News : NotifyPropertyBase
    {
        private string _beschreibung;
        public string Beschreibung { get { return _beschreibung; } set { OnPropertyChanged(ref _beschreibung, value); } }

        public DateTime ErstelltAm { get; set; }
        public User Ersteller { get; set; }

        public byte[] Foto { get; set; }
        public string PlanArt { get; set; }
        public string PlanID { get; set; }

        public bool IsText
        {
            get
            {
                if (Foto == null && PlanArt == null && Beschreibung != null)
                    return true;
                else
                    return false;
            }
        }

        public bool IsFoto
        {
            get
            {
                if (Foto != null)
                    return true;
                else
                    return false;
            }
        }

        public bool IsPlan
        {
            get
            {
                if (PlanArt != null)
                    return true;
                else
                    return false;
            }
        }
    }
}
