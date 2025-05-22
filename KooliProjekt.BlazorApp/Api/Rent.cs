// KooliProjekt.BlazorApp/Api/Rent.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.BlazorApp.Api
{
    public class Rent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime Start_time { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime End_time { get; set; }

        [Required(ErrorMessage = "Kilometers driven is required")]
        public decimal Kilometers_driven { get; set; }

        public bool Is_cancelled { get; set; }

        // Car relation
        public int CarId { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}
