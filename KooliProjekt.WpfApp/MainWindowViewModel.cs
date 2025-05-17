using KooliProjekt.WpfApp.Api;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : IDisposable
    {
        private readonly IApiClient _apiClient;

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
                await _apiClient.Save(SelectedCar);
                await Load(); // Обновляем список после сохранения
            }, _ =>
            {
                // CanExecute method
                return SelectedCar != null;
            });

            DeleteCommand = new RelayCommand<Car>(async _ =>
            {
                // Execute method
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
            }, _ => SelectedCar != null);

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

        public Car SelectedCar { get; set; }

        public void Dispose()
        {
            if (_apiClient is IDisposable)
            {
                (_apiClient as IDisposable).Dispose();
            }
        }
    }
}