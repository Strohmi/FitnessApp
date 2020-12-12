using System;
namespace FitnessApp.Models
{
    //Autor: NiE

    public class Follower
    {
        public int Index { get; set; }
        public User User { get; set; }
        public DateTime GefolgtAm { get; set; }
    }

    public class Like
    {
        public int Index { get; set; }
        public User User { get; set; }
    }
}
