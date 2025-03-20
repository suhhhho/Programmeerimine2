using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class Rent
    {
        public int Id { get; set; }
        public DateTime śtart_time { get; set; }
        public DateTime end_time { get; set; }
        public Decimal kilometrs_driven { get; set; }
        public Boolean is_cancelled { get; set; }
        public Cars cars { get; set; }
        public int carId { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
    }
}
