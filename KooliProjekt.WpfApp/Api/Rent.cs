namespace KooliProjekt.WpfApp.Api
{
    public class Rent
    {
        public int Id { get; set; }
        public DateTime śtart_time { get; set; }
        public DateTime end_time { get; set; }
        public Decimal kilometrs_driven { get; set; }
        public Boolean is_cancelled { get; set; }
        public int carId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
    }
}