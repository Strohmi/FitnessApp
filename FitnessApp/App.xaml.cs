using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using FitnessApp.Models.General;
using FitnessApp.Models;
using System.Collections.Generic;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace FitnessApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            if (DeviceInfo.Platform.ToString() == "iOS")
                CheckPermissions();

            AutoLogin();
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }

        private void AutoLogin()
        {
            string user = null, passwort = null;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    if (Current.Properties.ContainsKey("userid"))
                        if (Current.Properties["userid"] != null)
                            user = Current.Properties["userid"].ToString();

                    if (Current.Properties.ContainsKey("userpw"))
                        if (Current.Properties["userpw"] != null)
                            passwort = Current.Properties["userpw"].ToString();

                    if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(passwort))
                    {
                        bool? exists = AllVM.Datenbank.User.Exists(user);
                        if (exists == true)
                        {
                            string pw = AllVM.Datenbank.User.GetPasswort(user);

                            if (user != null && (passwort == pw))
                            {
                                AllVM.Initial(user);
                                MainPage = new AppShell();
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().ShortAlert("Passwort stimmt nicht");
                                MainPage = new NavigationPage(new Login());
                            }
                        }
                        else if (exists == false)
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Nutzer existiert nicht");
                            MainPage = new NavigationPage(new Login());
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Fehler beim Prüfen");
                            MainPage = new NavigationPage(new Login());
                        }
                    }
                    else
                    {
                        MainPage = new NavigationPage(new Login());
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                MainPage = new NavigationPage(new Login());
            }
        }

        private async void CheckPermissions()
        {
            try
            {
                List<Permission> cache = new List<Permission>();
                Permission[] permissions = new Permission[]
                {
                    Permission.Camera,
                    Permission.Photos
                };

                foreach (var item in permissions)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(item);

                    if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        cache.Add(item);
                }

                if (cache.Count > 0)
                {
                    Permission[] array = cache.ToArray();
                    var result = await CrossPermissions.Current.RequestPermissionsAsync(array);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}
