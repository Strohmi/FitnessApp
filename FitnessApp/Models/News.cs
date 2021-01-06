using System;
namespace FitnessApp
{
    //Autor: Niklas Erichsen

    public class News : NotifyPropertyBase
    {
        private int _id;
        private string _beschreibung;
        private byte[] _foto;
        private string _planArt;
        private bool _liked;
        private bool _likedTimer;
        private int _likes;

        public int ID { get { return _id; } set { OnPropertyChanged(ref _id, value); } }
        public string Beschreibung { get { return _beschreibung; } set { OnPropertyChanged(ref _beschreibung, value); } }

        public DateTime ErstelltAm { get; set; }
        public User Ersteller { get; set; }

        public byte[] Foto { get { return _foto; } set { OnPropertyChanged(ref _foto, value); } }
        public string PlanArt { get { return _planArt; } set { OnPropertyChanged(ref _planArt, value); } }
        public string PlanID { get; set; }

        public bool LikedTimer { get { return _likedTimer; } set { OnPropertyChanged(ref _likedTimer, value); } }
        public bool Liked { get { return _liked; } set { OnPropertyChanged(ref _liked, value); } }
        public int Likes { get { return _likes; } set { OnPropertyChanged(ref _likes, value); } }

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
    }
}
