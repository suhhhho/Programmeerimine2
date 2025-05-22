using KooliProjekt.WinFormsApp.Api;
using KooliProjekt.WinFormsApp.MVP;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.WinFormsApp.UnitTests
{
    /// <summary>
    /// Tests for the MVP design pattern implementation
    /// </summary>
    public class UnitTest1
    {
        private readonly Mock<ITodoView> _mockView;
        private readonly Mock<IApiClient> _mockApiClient;
        private TodoListPresenter _presenter;

        public UnitTest1()
        {
            // Setup mocks for each test
            _mockView = new Mock<ITodoView>();
            _mockApiClient = new Mock<IApiClient>();
            _presenter = new TodoListPresenter(_mockView.Object, _mockApiClient.Object);
        }

        [Fact]
        public void Test1()
        {
            // Arrange - Setting up a car object
            var car = new Car
            {
                Id = 1,
                Title = "Test Car",
                rental_rate_per_minute = 0.5m,
                rental_rate_per_km = 1.0m,
                is_available = true
            };

            // Act - Call presenter method with the car
            _presenter.SetCurrentCar(car);

            // Assert - Verify that the view's DisplayCarDetails was called with the car
            _mockView.Verify(view => view.DisplayCarDetails(car), Moq.Times.Once);
        }

        [Fact]
        public async Task LoadCars_ShouldPopulateView_WhenApiReturnsSuccess()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = 1, Title = "Car 1", rental_rate_per_minute = 0.5m, rental_rate_per_km = 1.0m, is_available = true },
                new Car { Id = 2, Title = "Car 2", rental_rate_per_minute = 0.7m, rental_rate_per_km = 1.2m, is_available = false }
            };

            var result = Result.Ok<IList<Car>>(cars);
            _mockApiClient.Setup(client => client.List()).ReturnsAsync(result);

            // Act
            await _presenter.LoadCars();

            // Assert
            _mockView.Verify(view => view.DisplayCars(It.Is<List<Car>>(list => list.Count == 2)), Moq.Times.Once);
        }

        [Fact]
        public async Task SaveCar_ShouldUpdateCarWithViewData_BeforeSaving()
        {
            // Arrange - Setup view properties and current car
            var car = new Car { Id = 1, Title = "Original Title" };
            _presenter.SetCurrentCar(car);

            _mockView.Setup(view => view.CurrentTitle).Returns("Updated Title");
            _mockView.Setup(view => view.CurrentRatePerMinute).Returns(1.5m);
            _mockView.Setup(view => view.CurrentRatePerKm).Returns(2.0m);
            _mockView.Setup(view => view.CurrentIsAvailable).Returns(true);

            _mockApiClient.Setup(client => client.Save(It.IsAny<Car>()))
            .ReturnsAsync(Result.Ok())
                .Callback<Car>(savedCar =>
                {
                    // Verify car properties were updated from view
                    Assert.Equal("Updated Title", savedCar.Title);
                    Assert.Equal(1.5m, savedCar.rental_rate_per_minute);
                    Assert.Equal(2.0m, savedCar.rental_rate_per_km);
                    Assert.True(savedCar.is_available);
                });

            // Setup list to return after save
            _mockApiClient.Setup(client => client.List())
                .ReturnsAsync(Result.Ok<IList<Car>>(new List<Car> { car }));

            // Act
            await _presenter.SaveCar();

            // Assert
            _mockApiClient.Verify(client => client.Save(It.IsAny<Car>()), Moq.Times.Once);
            _mockApiClient.Verify(client => client.List(), Moq.Times.Once);
            _mockView.Verify(view => view.ShowMessage("Car saved successfully", "Success", MessageType.Information), Moq.Times.Once);
        }

        [Fact]
        public void CreateNewCar_ShouldSetDefaultValues()
        {
            // Act
            _presenter.CreateNewCar();

            // Assert - Verify the default values are set and displayed
            _mockView.Verify(view => view.DisplayCarDetails(It.Is<Car>(c =>
                c.Id == 0 &&
                c.Title == "New car" &&
                c.rental_rate_per_minute == 0.5m &&
                c.rental_rate_per_km == 1.0m &&
                c.is_available == true)), Moq.Times.Once);
        }

        [Fact]
        public async Task DeleteCar_ShouldNotProceed_WhenNoCarSelected()
        {
            // Act - Try to delete with no car selected
            await _presenter.DeleteCar();

            // Assert - Verify warning is shown and API is not called
            _mockView.Verify(view => view.ShowMessage("No car selected or car has not been saved yet", "Warning", MessageType.Warning), Moq.Times.Once);
            _mockApiClient.Verify(client => client.Delete(It.IsAny<int>()), Moq.Times.Never);
        }
    }
}
