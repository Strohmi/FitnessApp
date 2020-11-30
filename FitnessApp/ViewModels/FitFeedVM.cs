using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FitnessApp
{
    public class FitFeedVM : NotifyPropertyBase
    {
        private ObservableCollection<News> _listNews;
        private bool _liked;

        public bool Liked { get { return _liked; } set { OnPropertyChanged(ref _liked, value); } }
        public ObservableCollection<News> ListNews { get { return _listNews; } set { OnPropertyChanged(ref _listNews, value); } }

        public FitFeedVM()
        {
            this.ListNews = new ObservableCollection<News>();
        }
    }
}
