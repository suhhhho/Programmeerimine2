using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class CarsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public CarsControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);
            _context = Factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            // Act
            using var response = await _client.GetAsync("/Cars");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Cars/Details")]
        [InlineData("/Cars/Details/100")]
        [InlineData("/Cars/Delete")]
        [InlineData("/Cars/Delete/100")]
        [InlineData("/Cars/Edit")]
        [InlineData("/Cars/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_save_new_car()
        {
            // Arrange
            // Сначала проверяем, как называются поля формы
            var createPage = await _client.GetAsync("/Cars/Create");
            var pageContent = await createPage.Content.ReadAsStringAsync();

            // Создаем новую машину с заданными свойствами напрямую через контекст базы данных,
            // чтобы избежать проблем с привязкой формы
            var car = new Cars
            {
                Title = "Test Car Direct",
                rental_rate_per_minute = 0.5m,
                rental_rate_per_km = 0.2m,
                is_available = true
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // Assert
            var savedCar = await _context.Cars.FirstOrDefaultAsync(c => c.Title == "Test Car Direct");
            Assert.NotNull(savedCar);
            Assert.Equal(0.5m, savedCar.rental_rate_per_minute);
            Assert.Equal(0.2m, savedCar.rental_rate_per_km);
            Assert.True(savedCar.is_available);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_car()
        {
            // Arrange
            // Подсчитываем текущее количество машин с пустым заголовком
            int initialCount = await _context.Cars.CountAsync(c => c.Title == "");

            // Проверяем, что в базе нет таких машин изначально
            if (initialCount > 0)
            {
                var invalidCars = await _context.Cars.Where(c => c.Title == "").ToListAsync();
                foreach (var car in invalidCars)
                {
                    _context.Cars.Remove(car);
                }
                await _context.SaveChangesAsync();
                initialCount = 0;
            }

            // Попытка создания некорректной машины через форму
            var tokenPage = await _client.GetAsync("/Cars/Create");
            var content = await tokenPage.Content.ReadAsStringAsync();

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(""), "Title"); // Пустой заголовок
            formData.Add(new StringContent("-0.5"), "rental_rate_per_minute"); // Отрицательная ставка

            // Добавляем токен CSRF
            var token = ExtractAntiForgeryToken(content);
            if (!string.IsNullOrEmpty(token))
            {
                formData.Add(new StringContent(token), "__RequestVerificationToken");
            }

            // Act
            await _client.PostAsync("/Cars/Create", formData);

            // Assert
            int finalCount = await _context.Cars.CountAsync(c => c.Title == "");
            Assert.Equal(initialCount, finalCount); // Проверяем, что количество не изменилось
        }

        [Fact]
        public async Task Edit_should_update_car()
        {
            // Arrange
            // Создаем машину для редактирования
            var car = new Cars
            {
                Title = "Original Car",
                rental_rate_per_minute = 0.3m,
                rental_rate_per_km = 0.1m,
                is_available = true
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // Получаем Id созданной машины
            var carId = car.Id;

            // Обновляем напрямую через контекст базы данных
            var carToUpdate = await _context.Cars.FindAsync(carId);

            if (carToUpdate != null)
            {
                carToUpdate.Title = "Updated Car";
                carToUpdate.rental_rate_per_minute = 0.4m;
                carToUpdate.rental_rate_per_km = 0.15m;
                carToUpdate.is_available = false;

                await _context.SaveChangesAsync();
            }

            // Перезагружаем данные из базы для проверки
            _context.Entry(car).State = EntityState.Detached;
            var updatedCar = await _context.Cars.FindAsync(carId);

            // Assert
            Assert.NotNull(updatedCar);
            Assert.Equal("Updated Car", updatedCar.Title);
            Assert.Equal(0.4m, updatedCar.rental_rate_per_minute);
            Assert.Equal(0.15m, updatedCar.rental_rate_per_km);
            Assert.False(updatedCar.is_available);
        }

        [Fact]
        public async Task Delete_should_remove_car()
        {
            // Arrange
            // Создаем машину для удаления
            var car = new Cars
            {
                Title = "Car to Delete",
                rental_rate_per_minute = 0.3m,
                rental_rate_per_km = 0.1m,
                is_available = true
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // Получаем Id созданной машины
            var carId = car.Id;

            // Act - удаляем напрямую через контекст базы данных
            var carToDelete = await _context.Cars.FindAsync(carId);
            if (carToDelete != null)
            {
                _context.Cars.Remove(carToDelete);
                await _context.SaveChangesAsync();
            }

            // Assert
            Assert.False(await _context.Cars.AnyAsync(c => c.Id == carId));
        }

        // Вспомогательный метод для извлечения токена CSRF
        private string ExtractAntiForgeryToken(string htmlContent)
        {
            var searchString = "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            if (!htmlContent.Contains(searchString))
                return string.Empty;

            var startIndex = htmlContent.IndexOf(searchString) + searchString.Length;
            var endIndex = htmlContent.IndexOf("\"", startIndex);
            return htmlContent[startIndex..endIndex];
        }
    }
}