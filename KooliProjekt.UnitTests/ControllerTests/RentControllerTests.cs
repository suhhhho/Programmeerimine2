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
    public class RentControllerTests
    {
        private readonly Mock<IRentService> _rentServiceMock;
        private readonly RentsController _controller;

        public RentControllerTests()
        {
            _rentServiceMock = new Mock<IRentService>();
            _controller = new RentsController(_rentServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Rent>
            {
                new Rent { Id = 1, Title = "Test 1" },
                new Rent { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<Rent> { Results = data };
            _rentServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

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
            _rentServiceMock
                .Setup(x => x.Delete(id))
        .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _rentServiceMock.VerifyAll();
        }
            [Fact]
            public async Task DeleteConfirmed_should_delete_list()
            {
                // Arrange
                int id = 1;
                _rentServiceMock
                    .Setup(x => x.Delete(id))
            .Verifiable();

                // Act
                var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

                // Assert
                Assert.NotNull(result);
                _rentServiceMock.VerifyAll();

            }
            [Fact]
            public async Task Edit_should_return_view_with_invalid_modelstate()
            {
                // Arrange
                var rent = new Rent { Id = 1, Title = "Test rent" };
                _controller.ModelState.AddModelError("key", "error");

                // Act
                var result = await _controller.Edit(rent.Id, rent) as ViewResult;

                // Assert
                Assert.NotNull(result);
                Assert.False(_controller.ModelState.IsValid);
                Assert.Equal(rent, result.Model);
            }

            [Fact]
            public async Task Edit_should_redirect_when_model_is_valid()
            {
                // Arrange
                var rent = new Rent { Id = 1, Title = "Test rent" };
                _rentServiceMock.Setup(x => x.Save(It.IsAny<Rent>()));

                // Act
                var result = await _controller.Edit(rent.Id, rent) as RedirectToActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
                _rentServiceMock.Verify(x => x.Save(It.IsAny<Rent>()), Times.Once);

            }
            [Fact]
            public async Task Create_should_return_view_with_invalid_modelstate()
            {
                // Arrange
                var rent = new Rent { Title = "Test rent" };
                _controller.ModelState.AddModelError("key", "error");

                // Act
                var result = await _controller.Create(rent) as ViewResult;

                // Assert       
                Assert.NotNull(result);
                Assert.False(_controller.ModelState.IsValid);
                Assert.Equal(rent, result.Model);
            }

            [Fact]
            public async Task Create_should_redirect_when_model_is_valid()
            {
                // Arrange
                var rent = new Rent { Title = "Test rent" };
                _rentServiceMock.Setup(x => x.Save(It.IsAny<Rent>()));

                // Act
                var result = await _controller.Create(rent) as RedirectToActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
                _rentServiceMock.Verify(x => x.Save(It.IsAny<Rent>()), Times.Once);
            }
        }
        }
  
