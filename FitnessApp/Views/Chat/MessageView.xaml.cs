using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessApp.Models;
using FitnessApp.Models.General;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp.Views.Chat
{

    public partial class MessageView : ContentPage
    {
        public IList<TestUser> testUsers { get; private set; }
        public MessageView()
        {
            InitializeComponent();
            GetData();
            BindingContext = this;
        }

        public void GetData()
        {

            testUsers = new List<TestUser>();
            testUsers.Add(new TestUser
            {
                User = AllVM.Datenbank.User.GetByName("nicmis"),
                NachrichtenVorschau = "Wann treffen wir uns"

            }); ;

            testUsers.Add(new TestUser
            {
                User = AllVM.Datenbank.User.GetByName("nikeri"),
                NachrichtenVorschau = "Hi, Hai"


            });

            testUsers.Add(new TestUser
            {
                User = AllVM.Datenbank.User.GetByName("tuneke"),
                NachrichtenVorschau = "wann nächster Vino"

            });



        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TestUser selectedItem = e.SelectedItem as TestUser;
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            TestUser tappedItem = e.Item as TestUser;
        }
    }
}