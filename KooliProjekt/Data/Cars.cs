﻿namespace KooliProjekt.Data;
using System.Diagnostics.CodeAnalysis;

  public class Cars
    {
    [ExcludeFromCodeCoverage]
    public int Id { get; set; }
     public Decimal rental_rate_per_minute { get; set; }
     public decimal rental_rate_per_km { get; set; }
     public Boolean is_available { get; set; }
        public string Title { get; set; }
     public string Description { get; set; }
    }

