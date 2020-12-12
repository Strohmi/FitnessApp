using System;
using System.Threading;
using FitnessApp.Models.General;

namespace FitnessApp.iOS.Helper
{
    //Autor: NiE

    public class CloseIOS : ICloseApp
    {
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
