using KooliProjekt.WpfApp.Api;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase, IDisposable
    {
        private readonly IApiClient _apiClient;
        private Car _selectedCar;

        public ObservableCollection<Car> Cars { get; private set; }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public Predicate<Car> ConfirmDelete { get; set; }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

            Cars = new ObservableCollection<Car>();

            SaveCommand = new RelayCommand<Car>(async _ =>
            {
                // Execute method
                if (SelectedCar == null) return;

                await _apiClient.Save(SelectedCar);
                await Load(); // Обновляем список после сохранения
            });

            DeleteCommand = new RelayCommand<Car>(async _ =>
            {
                // Execute method
                if (SelectedCar == null) return;

                if (ConfirmDelete != null)
                {
                    var result = ConfirmDelete(SelectedCar);
                    if (!result)
                    {
                        return;
                    }
                }

                await _apiClient.Delete(SelectedCar.Id);
                Cars.Remove(SelectedCar);
            });

            AddCommand = new RelayCommand<Car>(_ =>
            {
                // Execute method
                var newCar = new Car
                {
                    Id = 0,
                    Title = "Новый автомобиль",
                    rental_rate_per_minute = 0.5m,
                    rental_rate_per_km = 1.0m,
                    is_available = true
                };

                Cars.Add(newCar);
                SelectedCar = newCar;
            });
        }

        public async Task Load()
        {
            Cars.Clear();
            var cars = await _apiClient.List();
            foreach (var car in cars)
            {
                Cars.Add(car);
            }
        }

        // Это важная часть - правильная реализация свойства SelectedCar с уведомлением об изменениях
        public Car SelectedCar
        {
            get => _selectedCar;
            set => SetProperty(ref _selectedCar, value);
        }

        public void Dispose()
        {
            if (_apiClient is IDisposable)
            {
                (_apiClient as IDisposable).Dispose();
            }
        }
    }
}