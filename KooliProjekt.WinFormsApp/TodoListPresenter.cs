using KooliProjekt.WinFormsApp.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.WinFormsApp.MVP
{
    public class TodoListPresenter
    {
        private readonly ITodoView _view;
        private readonly IApiClient _apiClient;
        private Car _currentCar;
        private List<Car> _cars;

        public TodoListPresenter(ITodoView view, IApiClient apiClient)
        {
            _view = view;
            _apiClient = apiClient;
            _cars = new List<Car>();
        }

        public async Task LoadCars()
        {
            var result = await _apiClient.List();
            if (result.Success)
            {
                _cars = new List<Car>(result.Value);
                _view.DisplayCars(_cars);
            }
            else
            {
                _view.ShowMessage($"Error loading cars: {result.Error}", "Error", MessageType.Error);
            }
        }

        public void SetCurrentCar(Car car)
        {
            _currentCar = car;
            if (_currentCar != null)
            {
                _view.DisplayCarDetails(_currentCar);
            }
        }

        public void CreateNewCar()
        {
            _currentCar = new Car
            {
                Id = 0,
                Title = "New car",
                rental_rate_per_minute = 0.5m,
                rental_rate_per_km = 1.0m,
                is_available = true
            };

            _view.DisplayCarDetails(_currentCar);
        }

        public async Task SaveCar()
        {
            if (_currentCar == null)
            {
                _view.ShowMessage("No car selected", "Warning", MessageType.Warning);
                return;
            }

            // Update car with form data
            _currentCar.Title = _view.CurrentTitle;
            _currentCar.rental_rate_per_minute = _view.CurrentRatePerMinute;
            _currentCar.rental_rate_per_km = _view.CurrentRatePerKm;
            _currentCar.is_available = _view.CurrentIsAvailable;

            var result = await _apiClient.Save(_currentCar);
            if (result.Success)
            {
                await LoadCars(); // Refresh the list
                _view.ShowMessage("Car saved successfully", "Success", MessageType.Information);
            }
            else
            {
                _view.ShowMessage($"Error saving car: {result.Error}", "Error", MessageType.Error);
            }
        }

        public async Task DeleteCar()
        {
            if (_currentCar == null || _currentCar.Id == 0)
            {
                _view.ShowMessage("No car selected or car has not been saved yet", "Warning", MessageType.Warning);
                return;
            }

            var result = await _apiClient.Delete(_currentCar.Id);
            if (result.Success)
            {
                await LoadCars(); // Refresh the list
                _view.ClearFields();
                _view.ShowMessage("Car deleted successfully", "Success", MessageType.Information);
            }
            else
            {
                _view.ShowMessage($"Error deleting car: {result.Error}", "Error", MessageType.Error);
            }
        }
    }
}