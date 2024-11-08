using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;

public static class SeedData
{

        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (context.Rent.Any())
            {
                return;
            }

            var user = new IdentityUser
            {
                UserName = "newuser@example.com",
                Email = "newuser@example.com",
                NormalizedUserName = "NEWUSER@EXAMPLE.COM", // Optional but recommended for case-insensitivity
                NormalizedEmail = "NEWUSER@EXAMPLE.COM" // Optional but recommended
            };

            
            userManager.CreateAsync(user, "Password123!").Wait();

        var cars1 = new Cars();
        cars1.rental_rate_per_minute = 0.50m;
        cars1.rental_rate_per_km = 0.80m;
        cars1.is_available = true;
        context.Cars.Add(cars1);

        var invoice1 = new Invoice();
        invoice1.InvoiceNo = 1;
        invoice1.InvoiceDate = DateTime.Now;
        invoice1.DueDate = DateTime.Now;

        var rent1 = new Rent();
        rent1.śtart_time = DateTime.Now;
        rent1.end_time = DateTime.Now;
        rent1.kilometrs_driven = 10m;
        rent1.is_cancelled = true;





        context.SaveChanges();
    }

    internal static void Generate(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }
}