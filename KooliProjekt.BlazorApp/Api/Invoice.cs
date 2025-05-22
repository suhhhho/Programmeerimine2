// KooliProjekt.BlazorApp/Api/Invoice.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.BlazorApp.Api
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Invoice number is required")]
        public int InvoiceNo { get; set; }

        [Required(ErrorMessage = "Invoice date is required")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        // Relationship with Rent
        public int? RentId { get; set; }

        // Reference to Rent object for dropdown selection
        public Rent? Rent { get; set; }
    }
}
