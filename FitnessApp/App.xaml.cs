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
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            if (DeviceInfo.Platform.ToString() == "iOS")
                CheckPermissions();
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

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
