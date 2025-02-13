﻿using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Runtime;
using Android.OS;
using Android.Util;
using Xamarin.Forms;
using FitnessApp.Droid;

[assembly: Dependency(typeof(CloseDROID))]
[assembly: Dependency(typeof(MessageDROID))]
[assembly: Dependency(typeof(ImageManagerDROID))]
namespace FitnessApp.Droid
{
    [Activity(Label = "FitnessApp", Icon = "@mipmap/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            InitFontSize();

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        //Autor: Niklas Erichsen
        /// <summary>
        /// Schriftgröße vereinheitlichen
        /// </summary>
        public void InitFontSize()
        {
            Configuration config = Resources.Configuration;
            config.FontScale = (float)0.85;
            DisplayMetrics metrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(metrics);
            metrics.ScaledDensity = config.FontScale * metrics.Density;
            BaseContext.Resources.UpdateConfiguration(config, metrics);
        }

        /// <summary>
        /// Entsprechenden Berechtigungen anfragen
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}