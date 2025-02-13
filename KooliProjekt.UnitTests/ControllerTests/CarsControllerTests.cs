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
    public class CarControllerTests
    {
        private readonly Mock<ICarsService> _carsServiceMock;
        private readonly CarsController _controller;

        public CarControllerTests()
        {
            _carsServiceMock = new Mock<ICarsService>();
            _controller = new CarsController(_carsServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Cars>
            {
                new Cars { Id = 1, Title = "Test 1" },
                new Cars { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<Cars> { Results = data };
            _carsServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
        [Fact]
        public async Task DeleteConfirmed_should_delete_list()
        {
            // Arrange
            int id = 1;
            _carsServiceMock
                .Setup(x => x.Delete(id))
        .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _carsServiceMock.VerifyAll();

        }
        [Fact]
        public async Task Edit_should_return_view_with_invalid_modelstate()
        {
            // Arrange
            var car = new Cars { Id = 1, Title = "Test Car" };
            _controller.ModelState.AddModelError("key", "error");

            // Act
            var result = await _controller.Edit(car.Id, car) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(car, result.Model);
        }

        [Fact]
        public async Task Edit_should_redirect_when_model_is_valid()
        {
            // Arrange
            var car = new Cars { Id = 1, Title = "Test Car" };
            _carsServiceMock.Setup(x => x.Save(It.IsAny<Cars>()));

            // Act
            var result = await _controller.Edit(car.Id, car) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _carsServiceMock.Verify(x => x.Save(It.IsAny<Cars>()), Times.Once);

        }
        [Fact]
        public async Task Create_should_return_view_with_invalid_modelstate()
        {
            // Arrange
            var car = new Cars { Title = "Test Car" };
            _controller.ModelState.AddModelError("key", "error");

            // Act
            var result = await _controller.Create(car) as ViewResult;

            // Assert       
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(car, result.Model);
        }

        [Fact]
        public async Task Create_should_redirect_when_model_is_valid()
        {
            // Arrange
            var car = new Cars { Title = "Test Car" };
            _carsServiceMock.Setup(x => x.Save(It.IsAny<Cars>()));

            // Act
            var result = await _controller.Create(car) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _carsServiceMock.Verify(x => x.Save(It.IsAny<Cars>()), Times.Once);
        }



    }
}