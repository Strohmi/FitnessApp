using System.Collections.Generic;

namespace FitnessApp
{
    public class FavoPlansVM : NotifyPropertyBase
    {
        private List<object> _anzeigeListe;
        private List<Trainingsplan> _listTrPlan;
        private List<Ernährungsplan> _listErPlan;

        public List<FavoPlan> ListFavoPlans { get; set; }

        public List<Trainingsplan> ListTrPlan { get { return _listTrPlan; } set { OnPropertyChanged(ref _listTrPlan, value); } }
        public List<Ernährungsplan> ListErPlan { get { return _listErPlan; } set { OnPropertyChanged(ref _listErPlan, value); } }
        public List<object> AnzeigeListe { get { return _anzeigeListe; } set { OnPropertyChanged(ref _anzeigeListe, value); } }
    }

    public class FavoPlan
    {
        public int ID { get; set; }
        public string Typ { get; set; }
    }
}
