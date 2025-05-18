namespace KooliProjekt.WinFormsApp.Api
{
    public class Car
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal rental_rate_per_minute { get; set; }
        public decimal rental_rate_per_km { get; set; }
        public bool is_available { get; set; }
    }
}