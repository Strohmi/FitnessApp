using System;
using System.Collections.Generic;
namespace FitnessApp
{
    public class NewsFeedVM : NotifyPropertyBase
    {
        private List<News> _listNews;
        private bool _liked;

        public bool Liked { get { return _liked; } set { OnPropertyChanged(ref _liked, value); } }
        public List<News> ListNews { get { return _listNews; } set { OnPropertyChanged(ref _listNews, value); } }
    }
}
