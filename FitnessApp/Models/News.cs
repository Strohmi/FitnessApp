using System;
namespace FitnessApp
{
    public class News : NotifyPropertyBase
    {
        private string _beschreibung;
        private byte[] _foto;
        private string _planArt;

        public string Beschreibung { get { return _beschreibung; } set { OnPropertyChanged(ref _beschreibung, value); } }

        public DateTime ErstelltAm { get; set; }
        public User Ersteller { get; set; }

        public byte[] Foto { get { return _foto; } set { OnPropertyChanged(ref _foto, value); } }
        public string PlanArt { get { return _planArt; } set { OnPropertyChanged(ref _planArt, value); } }
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
