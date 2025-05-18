using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.BlazorApp.Api
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rate per minute is required")]
        [Range(0.1, 100, ErrorMessage = "Rate per minute must be between 0.1 and 100")]
        public decimal rental_rate_per_minute { get; set; }

        [Required(ErrorMessage = "Rate per kilometer is required")]
        [Range(0.1, 100, ErrorMessage = "Rate per kilometer must be between 0.1 and 100")]
        public decimal rental_rate_per_km { get; set; }

        public bool is_available { get; set; }
    }
}