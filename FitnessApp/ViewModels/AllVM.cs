using System;
namespace FitnessApp.Models.General
{
    public class AllVM
    {
        public static User User { get; set; } = new User();
        public static Datenbank Datenbank { get; set; } = new Datenbank();
    }
}
