using System;
using System.Linq;
using System.Collections.Generic;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Util;
using Android.Views;

namespace FitnessApp.Droid.Helper
{
    //Autor: NiE

    public class HelperDROID
    {
        public void CheckPermissions(MainActivity context)
        {
            List<string> permissions = new List<string>();

            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.Camera) != (int)Permission.Granted)
                permissions.Add(Manifest.Permission.Camera);

            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
                permissions.Add(Manifest.Permission.ReadExternalStorage);

            if (permissions.Count != 0)
                ActivityCompat.RequestPermissions(context, permissions.ToArray<string>(), 0);
        }
    }
}
