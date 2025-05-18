using KooliProjekt.WpfApp.Api;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel(new ApiClient());

            // Set up error handling
            _viewModel.OnError = ShowError;

            // Set up deletion confirmation
            _viewModel.ConfirmDelete = car => MessageBox.Show(
                $"Are you sure you want to delete {car.Title}?",
                "Confirm deletion",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes;

            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.Load();
        }
    }
}