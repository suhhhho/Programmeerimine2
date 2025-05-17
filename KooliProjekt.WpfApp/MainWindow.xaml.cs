using KooliProjekt.WpfApp.Api;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowViewModel(new ApiClient());
            viewModel.ConfirmDelete = car =>
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите удалить выбранный автомобиль?",
                    "Удаление автомобиля",
                    MessageBoxButton.YesNo
                    );

                return result == MessageBoxResult.Yes;
            };

            DataContext = viewModel;

            await viewModel.Load();
        }
    }
}