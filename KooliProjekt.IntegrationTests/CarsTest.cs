﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class CarsTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public CarsTests()
        {
            _client = Factory.CreateClient();
            _context = Factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Cars");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Cars/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Cars/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_list_was_found()
        {
            // Arrange
            var list = new Cars
            {
                Title = "List 1",
                Description = "Car description for testing",  // Added Description property
                rental_rate_per_minute = 0.5m,
                rental_rate_per_km = 0.5m,
                is_available = true
            };
            _context.Cars.Add(list);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/Cars/Details/" + list.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
