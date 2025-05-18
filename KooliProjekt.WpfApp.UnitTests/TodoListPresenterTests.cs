using KooliProjekt.WinFormsApp.Api;
using KooliProjekt.WinFormsApp.MVP;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.WinFormsApp.UnitTests
{
    public class TodoListPresenterTests
    {
        private readonly Mock<ITodoView> _mockView;
        private readonly Mock<IApiClient> _mockApiClient;
        private TodoListPresenter _presenter;

        public TodoListPresenterTests()
        {
            // Setup mocks for each test
            _mockView = new Mock<ITodoView>();
            _mockApiClient = new Mock<IApiClient>();
            _presenter = new TodoListPresenter(_mockView.Object, _mockApiClient.Object);
        }

        [Fact]
        public async Task LoadCars_Success_DisplaysCarsOnView()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = 1, Title = "Test Car 1", rental_rate_per_minute = 0.5m, rental_rate_per_km = 1.0m, is_available = true },
                new Car { Id = 2, Title = "Test Car 2", rental_rate_per_minute = 0.7m, rental_rate_per_km = 1.2m, is_available = false }
            };

            var result = Result.Ok<IList<Car>>(cars);
            _mockApiClient.Setup(client => client.List()).ReturnsAsync(result);

            // Act
            await _presenter.LoadCars();

            // Assert
            _mockView.Verify(view => view.DisplayCars(It.Is<List<Car>>(c => c.Count == 2)), Times.Once);
            _mockView.Verify(view => view.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageType>()), Times.Never);
        }

        [Fact]
        public async Task LoadCars_Error_ShowsErrorMessage()
        {
            // Arrange
            var errorMessage = "API Error";
            var result = Result.Fail<IList<Car>>(errorMessage);
            _mockApiClient.Setup(client => client.List()).ReturnsAsync(result);

            // Act
            await _presenter.LoadCars();

            // Assert
            _mockView.Verify(view => view.ShowMessage(It.Is<string>(s => s.Contains(errorMessage)), "Error", MessageType.Error), Times.Once);
            _mockView.Verify(view => view.DisplayCars(It.IsAny<List<Car>>()), Times.Never);
        }

        [Fact]
        public void SetCurrentCar_DisplaysCarDetails()
        {
            // Arrange
            var car = new Car { Id = 1, Title = "Test Car", rental_rate_per_minute = 0.5m, rental_rate_per_km = 1.0m, is_available = true };

            // Act
            _presenter.SetCurrentCar(car);

            // Assert
            _mockView.Verify(view => view.DisplayCarDetails(car), Times.Once);
        }

        [Fact]
        public void CreateNewCar_SetsDefaultValuesAndDisplays()
        {
            // Act
            _presenter.CreateNewCar();

            // Assert
            _mockView.Verify(view => view.DisplayCarDetails(It.Is<Car>(c =>
                c.Id == 0 &&
                c.Title == "New car" &&
                c.rental_rate_per_minute == 0.5m &&
                c.rental_rate_per_km == 1.0m &&
                c.is_available == true)), Times.Once);
        }

        [Fact]
        public async Task SaveCar_WithNoCar_ShowsWarning()
        {
            // Act
            await _presenter.SaveCar();

            // Assert
            _mockView.Verify(view => view.ShowMessage("No car selected", "Warning", MessageType.Warning), Times.Once);
            _mockApiClient.Verify(client => client.Save(It.IsAny<Car>()), Times.Never);
        }

        [Fact]
        public async Task SaveCar_Success_RefreshesListAndShowsSuccessMessage()
        {
            // Arrange
            var car = new Car { Id = 1, Title = "Test Car" };
            _presenter.SetCurrentCar(car);

            _mockView.Setup(view => view.CurrentTitle).Returns("Updated Car");
            _mockView.Setup(view => view.CurrentRatePerMinute).Returns(0.8m);
            _mockView.Setup(view => view.CurrentRatePerKm).Returns(1.5m);
            _mockView.Setup(view => view.CurrentIsAvailable).Returns(true);

            var saveResult = Result.Ok();
            _mockApiClient.Setup(client => client.Save(It.IsAny<Car>())).ReturnsAsync(saveResult);

            var listResult = Result.Ok<IList<Car>>(new List<Car> { car });
            _mockApiClient.Setup(client => client.List()).ReturnsAsync(listResult);

            // Act
            await _presenter.SaveCar();

            // Assert
            _mockApiClient.Verify(client => client.Save(It.Is<Car>(c =>
                c.Title == "Updated Car" &&
                c.rental_rate_per_minute == 0.8m &&
                c.rental_rate_per_km == 1.5m &&
                c.is_available == true)), Times.Once);

            _mockApiClient.Verify(client => client.List(), Times.Once);
            _mockView.Verify(view => view.ShowMessage("Car saved successfully", "Success", MessageType.Information), Times.Once);
        }

        [Fact]
        public async Task SaveCar_Error_ShowsErrorMessage()
        {
            // Arrange
            var car = new Car { Id = 1, Title = "Test Car" };
            _presenter.SetCurrentCar(car);

            var errorMessage = "Save Error";
            var result = Result.Fail(errorMessage);
            _mockApiClient.Setup(client => client.Save(It.IsAny<Car>())).ReturnsAsync(result);

            // Act
            await _presenter.SaveCar();

            // Assert
            _mockView.Verify(view => view.ShowMessage(It.Is<string>(s => s.Contains(errorMessage)), "Error", MessageType.Error), Times.Once);
            _mockApiClient.Verify(client => client.List(), Times.Never);
        }

        [Fact]
        public async Task DeleteCar_WithNoCar_ShowsWarning()
        {
            // Act
            await _presenter.DeleteCar();

            // Assert
            _mockView.Verify(view => view.ShowMessage("No car selected or car has not been saved yet", "Warning", MessageType.Warning), Times.Once);
            _mockApiClient.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCar_Success_RefreshesListAndShowsSuccessMessage()
        {
            // Arrange
            var car = new Car { Id = 1, Title = "Test Car" };
            _presenter.SetCurrentCar(car);

            var deleteResult = Result.Ok();
            _mockApiClient.Setup(client => client.Delete(1)).ReturnsAsync(deleteResult);

            var listResult = Result.Ok<IList<Car>>(new List<Car>());
            _mockApiClient.Setup(client => client.List()).ReturnsAsync(listResult);

            // Act
            await _presenter.DeleteCar();

            // Assert
            _mockApiClient.Verify(client => client.Delete(1), Times.Once);
            _mockApiClient.Verify(client => client.List(), Times.Once);
            _mockView.Verify(view => view.ClearFields(), Times.Once);
            _mockView.Verify(view => view.ShowMessage("Car deleted successfully", "Success", MessageType.Information), Times.Once);
        }

        [Fact]
        public async Task DeleteCar_Error_ShowsErrorMessage()
        {
            // Arrange
            var car = new Car { Id = 1, Title = "Test Car" };
            _presenter.SetCurrentCar(car);

            var errorMessage = "Delete Error";
            var result = Result.Fail(errorMessage);
            _mockApiClient.Setup(client => client.Delete(1)).ReturnsAsync(result);

            // Act
            await _presenter.DeleteCar();

            // Assert
            _mockView.Verify(view => view.ShowMessage(It.Is<string>(s => s.Contains(errorMessage)), "Error", MessageType.Error), Times.Once);
            _mockApiClient.Verify(client => client.List(), Times.Never);
            _mockView.Verify(view => view.ClearFields(), Times.Never);
        }
    }
}