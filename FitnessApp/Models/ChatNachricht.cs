using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Models
{
   public class ChatNachricht
    {
        public User Receiver { get; set; }
        public User Sender { get; set; }
        public DateTime GesendetAm { get; set; }
        public string Text { get; set; }



    }
}
