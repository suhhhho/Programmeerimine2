using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class RentServiceTests : ServiceTestBase
    {
        private readonly RentService _service;

        public RentServiceTests()
        {
            _service = new RentService(DbContext);
        }

        [Fact]
        public async Task Delete_should_remove_existing_list()
        {
            // Arrange
            // Add user to database and assign it to list.User property
            var list = new Rent { Title = "Test", UserId ="UserId"  };
            DbContext.Rent.Add(list);
            DbContext.SaveChanges();


            // Act
            await _service.Delete(list.Id);

            // Assert
            var count = DbContext.Rent.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_return_if_list_was_not_found()
        {
            // Arrange
            var id = -100;

            // Act
            await _service.Delete(id);

            // Assert
            var count = DbContext.Rent.Count();
            Assert.Equal(0, count);
        }
    }
}
