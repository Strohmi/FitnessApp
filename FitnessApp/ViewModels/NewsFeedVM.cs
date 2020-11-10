using System;
using System.Collections.Generic;
namespace FitnessApp
{
    public class NewsFeedVM : NotifyPropertyBase
    {
        private List<News> _listNews;
        public List<News> ListNews { get { return _listNews; } set { OnPropertyChanged(ref _listNews, value); } }
    }
}
