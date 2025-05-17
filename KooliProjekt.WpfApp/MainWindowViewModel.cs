using KooliProjekt.WpfApp.Api;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : IDisposable
    {
        private readonly IApiClient _apiClient;
        private readonly Dispatcher _dispatcher;

        public ObservableCollection<Car> Cars { get; private set; }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public Predicate<Car> ConfirmDelete { get; set; }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            _dispatcher = Dispatcher.CurrentDispatcher;

            Cars = new ObservableCollection<Car>();

            SaveCommand = new RelayCommand<Car>(async _ =>
            {
                try
                {
                    // Execute method
                    if (SelectedCar != null)
                    {
                        await _apiClient.Save(SelectedCar);
                        await Load(); // Обновляем список после сохранения
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка при сохранении: {ex.Message}");
                }
            });

            DeleteCommand = new RelayCommand<Car>(async _ =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка при удалении: {ex.Message}");
                }
            });

            AddCommand = new RelayCommand<Car>(_ =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка при добавлении: {ex.Message}");
                }
            });
        }

        private void ShowError(string message)
        {
            _dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        public async Task Load()
        {
            try
            {
                var cars = await _apiClient.List();

                _dispatcher.InvokeAsync(() =>
                {
                    Cars.Clear();
                    foreach (var car in cars)
                    {
                        Cars.Add(car);
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке данных: {ex.Message}");
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