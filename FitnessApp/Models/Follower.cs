using System;

namespace FitnessApp
{
    public class Follower
    {
        public int Index { get; set; }
        public User User { get; set; }
        public DateTime GefolgtAm { get; set; }
    }

    public class Like
    {
        public User User { get; set; }
        public bool IsSub { get; set; }
        public bool IsUser { get; set; }
    }
}
