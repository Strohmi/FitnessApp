namespace FitnessApp
{
    public class Bewertung
    {
        public int ID { get; set; }
        public int Rating { get; set; }
        public User Bewerter { get; set; }
    }
}
