using System;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<FakeStartup> Factory { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();

            // Убедимся, что база данных готова перед началом тестов
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Сбрасываем соединения перед удалением
            dbContext.Database.CloseConnection();
            dbContext.Database.EnsureDeleted();

            Factory?.Dispose();
        }
    }
}