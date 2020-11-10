using System;
using System.Threading;
using FitnessApp.Models.General;

namespace FitnessApp.iOS.Helper
{
    public class CloseIOS : ICloseApp
    {
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
