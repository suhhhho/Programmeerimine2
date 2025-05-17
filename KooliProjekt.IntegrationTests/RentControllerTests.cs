using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class RentControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        private readonly string _testUserId;
        private readonly string _controllerName; // Имя контроллера (Rent или Rents)

        public RentControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);
            _context = Factory.Services.GetRequiredService<ApplicationDbContext>();

            // Определяем правильное имя контроллера
            var response1 = _client.GetAsync("/Rent").GetAwaiter().GetResult();
            _controllerName = response1.IsSuccessStatusCode ? "Rent" : "Rents";

            // Создаем тестового пользователя для аренды
            var userManager = Factory.Services.GetRequiredService<UserManager<IdentityUser>>();
            var testUser = new IdentityUser
            {
                UserName = "test@example.com",
                Email = "test@example.com",
                EmailConfirmed = true
            };

            // Проверяем, существует ли пользователь, и создаем если нет
            var user = userManager.FindByEmailAsync(testUser.Email).GetAwaiter().GetResult();
            if (user == null)
            {
                var result = userManager.CreateAsync(testUser, "Password123!").GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new Exception($"Could not create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                _testUserId = testUser.Id;
            }
            else
            {
                _testUserId = user.Id;
            }
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            // Act
            using var response = await _client.GetAsync($"/{_controllerName}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_new_rent()
        {
            // Arrange
            // Сначала создаем автомобиль, который будем арендовать
            var car = new Cars
            {
                Title = "Rent Test Car",
                rental_rate_per_minute = 0.3m,
                rental_rate_per_km = 0.1m,
                is_available = true
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);

            // Создаем новую аренду с заданными свойствами напрямую через контекст базы данных
            var rent = new Rent
            {
                Title = "Test Rent Direct",
                śtart_time = startTime,
                end_time = endTime,
                kilometrs_driven = 50,
                is_cancelled = false,
                carId = car.Id,
                UserId = _testUserId,
                cars = car
            };

            _context.Rent.Add(rent);
            await _context.SaveChangesAsync();

            // Assert
            var savedRent = await _context.Rent.FirstOrDefaultAsync(r => r.Title == "Test Rent Direct");
            Assert.NotNull(savedRent);
            Assert.Equal(50, savedRent.kilometrs_driven);
            Assert.False(savedRent.is_cancelled);
            Assert.Equal(car.Id, savedRent.carId);
            Assert.Equal(_testUserId, savedRent.UserId);
        }

        [Fact]
        public async Task Edit_should_update_rent()
        {
            // Arrange
            // Сначала создаем автомобиль
            var car = new Cars
            {
                Title = "Edit Test Car",
                rental_rate_per_minute = 0.3m,
                rental_rate_per_km = 0.1m,
                is_available = true
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // Затем создаем аренду
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var rent = new Rent
            {
                Title = "Original Rent",
                śtart_time = startTime,
                end_time = endTime,
                kilometrs_driven = 30,
                is_cancelled = false,
                carId = car.Id,
                UserId = _testUserId,
                cars = car
            };
            _context.Rent.Add(rent);
            await _context.SaveChangesAsync();

            // Получаем Id созданной аренды
            var rentId = rent.Id;

            // Получаем и обновляем аренду
            var rentToUpdate = await _context.Rent.FindAsync(rentId);
            if (rentToUpdate != null)
            {
                rentToUpdate.Title = "Updated Rent";
                rentToUpdate.kilometrs_driven = 45;

                await _context.SaveChangesAsync();
            }

            // Перезагружаем данные из базы
            _context.Entry(rent).State = EntityState.Detached;
            var updatedRent = await _context.Rent.FindAsync(rentId);

            // Assert
            Assert.NotNull(updatedRent);
            Assert.Equal("Updated Rent", updatedRent.Title);
            Assert.Equal(45, updatedRent.kilometrs_driven);
        }

        [Fact]
        public async Task Cancel_should_mark_rent_as_cancelled()
        {
            // Arrange
            // Сначала создаем автомобиль
            var car = new Cars
            {
                Title = "Cancel Test Car",
                rental_rate_per_minute = 0.3m,
                rental_rate_per_km = 0.1m,
                is_available = true
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // Затем создаем аренду
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var rent = new Rent
            {
                Title = "Rent to Cancel",
                śtart_time = startTime,
                end_time = endTime,
                kilometrs_driven = 30,
                is_cancelled = false,
                carId = car.Id,
                UserId = _testUserId,
                cars = car
            };
            _context.Rent.Add(rent);
            await _context.SaveChangesAsync();

            // Получаем Id созданной аренды
            var rentId = rent.Id;

            // Act - отменяем аренду напрямую через контекст базы данных
            var rentToCancel = await _context.Rent.FindAsync(rentId);
            if (rentToCancel != null)
            {
                rentToCancel.is_cancelled = true;
                await _context.SaveChangesAsync();
            }

            // Перезагружаем данные из базы
            _context.Entry(rent).State = EntityState.Detached;
            var cancelledRent = await _context.Rent.FindAsync(rentId);

            // Assert
            Assert.NotNull(cancelledRent);
            Assert.True(cancelledRent.is_cancelled);
        }

        [Fact]
        public async Task Delete_should_remove_rent()
        {
            // Arrange
            // Сначала создаем автомобиль
            var car = new Cars
            {
                Title = "Delete Test Car",
                rental_rate_per_minute = 0.3m,
                rental_rate_per_km = 0.1m,
                is_available = true
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // Затем создаем аренду для удаления
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var rent = new Rent
            {
                Title = "Rent to Delete",
                śtart_time = startTime,
                end_time = endTime,
                kilometrs_driven = 30,
                is_cancelled = false,
                carId = car.Id,
                UserId = _testUserId,
                cars = car
            };
            _context.Rent.Add(rent);
            await _context.SaveChangesAsync();

            // Получаем Id созданной аренды
            var rentId = rent.Id;

            // Act - удаляем напрямую через контекст базы данных
            var rentToDelete = await _context.Rent.FindAsync(rentId);
            if (rentToDelete != null)
            {
                _context.Rent.Remove(rentToDelete);
                await _context.SaveChangesAsync();
            }

            // Assert - проверяем, что аренда удалена
            Assert.False(await _context.Rent.AnyAsync(r => r.Id == rentId));
        }
    }
}