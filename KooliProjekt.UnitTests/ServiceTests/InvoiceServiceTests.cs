using KooliProjekt.Data;
using KooliProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceServiceTests : ServiceTestBase
    {
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _service = new InvoiceService(DbContext);
        }

        [Fact]
        public async Task Delete_should_remove_existing_list()
        {
            // Arrange
            var list = new Invoice { Title = "Test" };
            DbContext.Invoices.Add(list);
            DbContext.SaveChanges();

            // Act
            await _service.Delete(list.Id);

            // Assert
            var count = DbContext.Invoices.Count();
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
            var count = DbContext.Invoices.Count();
            Assert.Equal(0, count);
        }
    }
}