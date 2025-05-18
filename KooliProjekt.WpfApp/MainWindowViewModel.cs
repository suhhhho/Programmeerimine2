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

        // Add the OnError action
        public Action<string> OnError { get; set; }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

            Cars = new ObservableCollection<Car>();

            SaveCommand = new RelayCommand<Car>(async _ =>
            {
                // Execute method
                if (SelectedCar == null) return;

                var result = await _apiClient.Save(SelectedCar);
                if (result.Success)
                {
                    await Load(); // Refresh list after saving
                }
                else
                {
                    OnError?.Invoke($"Failed to save car: {result.Error}");
                }
            });

            DeleteCommand = new RelayCommand<Car>(async _ =>
            {
                // Execute method
                if (SelectedCar == null) return;

                if (ConfirmDelete != null)
                {
                    var confirmResult = ConfirmDelete(SelectedCar);
                    if (!confirmResult)
                    {
                        return;
                    }
                }

                var result = await _apiClient.Delete(SelectedCar.Id);
                if (result.Success)
                {
                    Cars.Remove(SelectedCar);
                }
                else
                {
                    OnError?.Invoke($"Failed to delete car: {result.Error}");
                }
            });

            AddCommand = new RelayCommand<Car>(_ =>
            {
                // Execute method
                var newCar = new Car
                {
                    Id = 0,
                    Title = "New car",
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
            var result = await _apiClient.List();
            if (result.Success)
            {
                Cars.Clear();
                foreach (var car in result.Value)
                {
                    Cars.Add(car);
                }
            }
            else
            {
                OnError?.Invoke($"Failed to load cars: {result.Error}");
            }
        }

        // Property implementation with notification
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