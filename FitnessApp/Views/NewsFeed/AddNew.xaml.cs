using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    public partial class AddNew : ContentPage
    {
        public AddNewVM AddNewVM { get; set; }

        public AddNew()
        {
            InitializeComponent();
            AddNewVM = new AddNewVM();
            Start();
            BindingContext = AddNewVM;
        }

        private void Loaded(object sender, EventArgs e)
        {
            listOptions.SelectedItem = null;
        }

        private void Start()
        {
            Title = "Hinzufügen";
        }

        async void Selection(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            string msg = null;
            ListView listView = (sender as ListView);

            if (listView != null && listView.SelectedItem != null)
            {
                KeyValuePair<int, string> result = (KeyValuePair<int, string>)listView.SelectedItem;
                msg = result.ToString();

                switch (result.Value)
                {
                    case "Status":
                        await this.Navigation.PushAsync(new StatusNew());
                        break;
                    case "Trainingsplan":
                        //TEST();
                        break;
                    default:
                        await DisplayAlert("Auswahl:", msg, "OK");
                        break;
                }
                listView.SelectedItem = null;
            }
        }

        //private void TEST()
        //{
        //    var list = GetData();
        //    _ = list;
        //}

        //private SqlConnection Connection;
        //private List<User> GetData()
        //{
        //    try
        //    {
        //        List<User> Liste = new List<User>();
        //        string conString = "server=goliath.wi.fh-flensburg.de;Database=WS2021_FitnessApp;user=WS2021_FitnessApp;password=kpes_2120";
        //        Connection = new SqlConnection(conString);

        //        string com = "SELECT * FROM User";
        //        SqlCommand command = new SqlCommand(com, Connection);
        //        Connection.Open();
        //        var reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            User user = new User()
        //            {
        //                Vorname = reader.GetString(1),
        //                Nachname = reader.GetString(2)
        //            };
        //            Liste.Add(user);
        //        }

        //        Connection.Close();
        //        return Liste;
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = ex.Message;
        //        if (Connection != null)
        //            if (Connection.State != System.Data.ConnectionState.Closed)
        //                Connection.Close();
        //        return null;
        //    }
        //}
    }
}