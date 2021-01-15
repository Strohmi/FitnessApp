using System.Threading;

namespace FitnessApp.iOS
{
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
