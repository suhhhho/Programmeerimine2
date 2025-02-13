using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class InvoiceControllerTests
    {
        private readonly Mock<IInvoiceService> _invoiceServiceMock;
        private readonly InvoicesController _controller;

        public InvoiceControllerTests()
        {
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _controller = new InvoicesController(_invoiceServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Invoice>
            {
                new Invoice { Id = 1, Title = "Test 1" },
                new Invoice { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<Invoice> { Results = data };
            _invoiceServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
        [Fact]
        public async Task DeleteConfirmedShouldDeleteList()
        {
            // Arrange
            int id = 1;
            _invoiceServiceMock
                .Setup(x => x.Delete(id))
        .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _invoiceServiceMock.VerifyAll();
        }
            [Fact]
            public async Task DeleteConfirmed_should_delete_list()
            {
                // Arrange
                int id = 1;
                _invoiceServiceMock
                    .Setup(x => x.Delete(id))
            .Verifiable();

                // Act
                var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

                // Assert
                Assert.NotNull(result);
                _invoiceServiceMock.VerifyAll();

            }
            [Fact]
            public async Task Edit_should_return_view_with_invalid_modelstate()
            {
                // Arrange
                var invoice = new Invoice { Id = 1, Title = "Test invoice" };
                _controller.ModelState.AddModelError("key", "error");

                // Act
                var result = await _controller.Edit(invoice.Id, invoice) as ViewResult;

                // Assert
                Assert.NotNull(result);
                Assert.False(_controller.ModelState.IsValid);
                Assert.Equal(invoice, result.Model);
            }

            [Fact]
            public async Task Edit_should_redirect_when_model_is_valid()
            {
                // Arrange
                var invoice = new Invoice { Id = 1, Title = "Test invoice" };
                _invoiceServiceMock.Setup(x => x.Save(It.IsAny<Invoice>()));

                // Act
                var result = await _controller.Edit(invoice.Id, invoice) as RedirectToActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
                _invoiceServiceMock.Verify(x => x.Save(It.IsAny<Invoice>()), Times.Once);

            }
            [Fact]
            public async Task Create_should_return_view_with_invalid_modelstate()
            {
                // Arrange
                var invoice = new Invoice { Title = "Test invoice" };
                _controller.ModelState.AddModelError("key", "error");

                // Act
                var result = await _controller.Create(invoice) as ViewResult;

                // Assert       
                Assert.NotNull(result);
                Assert.False(_controller.ModelState.IsValid);
                Assert.Equal(invoice, result.Model);
            }

            [Fact]
            public async Task Create_should_redirect_when_model_is_valid()
            {
                // Arrange
                var invoice = new Invoice { Title = "Test invoice" };
                _invoiceServiceMock.Setup(x => x.Save(It.IsAny<Invoice>()));

                // Act
                var result = await _controller.Create(invoice) as RedirectToActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
                _invoiceServiceMock.Verify(x => x.Save(It.IsAny<Invoice>()), Times.Once);
            }
        }
        }