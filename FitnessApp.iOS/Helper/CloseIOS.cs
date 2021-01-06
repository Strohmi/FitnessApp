using System;
using System.Threading;
using FitnessApp.Models.General;

namespace FitnessApp.iOS.Helper
{
    //Autor: Niklas Erichsen

    public class CloseIOS : ICloseApp
    {
        /// <summary>
        /// App schließen
        /// </summary>
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
