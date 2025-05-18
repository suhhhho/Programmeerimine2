using KooliProjekt.WinFormsApp.Api;
using KooliProjekt.WinFormsApp.MVP;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form, ITodoView
    {
        private readonly IApiClient _apiClient;
        private TodoListPresenter _presenter;

        public int CurrentId
        {
            get => int.TryParse(IdField.Text, out int id) ? id : 0;
            set => IdField.Text = value.ToString();
        }

        public string CurrentTitle
        {
            get => TitleField.Text;
            set => TitleField.Text = value;
        }

        // Connect the properties to the new UI controls
        public decimal CurrentRatePerMinute
        {
            get => RatePerMinuteField.Value;
            set => RatePerMinuteField.Value = value;
        }

        public decimal CurrentRatePerKm
        {
            get => RatePerKmField.Value;
            set => RatePerKmField.Value = value;
        }

        public bool CurrentIsAvailable
        {
            get => IsAvailableCheckBox.Checked;
            set => IsAvailableCheckBox.Checked = value;
        }

        public Form1()
        {
            InitializeComponent();
            _apiClient = new ApiClient("https://localhost:7136");
            _presenter = new TodoListPresenter(this, _apiClient);

            // Wire up event handlers for the buttons
            NewButton.Click += NewButton_Click;
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;

            // Wire up the grid selection changed event
            TodoListsGrid.SelectionChanged += TodoListsGrid_SelectionChanged;

            // Load cars via the presenter
            LoadCars();
        }

        private async void LoadCars()
        {
            await _presenter.LoadCars();
        }

        private void TodoListsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (TodoListsGrid.CurrentRow != null)
            {
                var selectedCar = TodoListsGrid.CurrentRow.DataBoundItem as Car;
                _presenter.SetCurrentCar(selectedCar);
            }
        }

        public void DisplayCarDetails(Car car)
        {
            if (car == null) return;

            CurrentId = car.Id;
            CurrentTitle = car.Title;
            CurrentRatePerMinute = car.rental_rate_per_minute;
            CurrentRatePerKm = car.rental_rate_per_km;
            CurrentIsAvailable = car.is_available;
        }

        public void ClearFields()
        {
            CurrentId = 0;
            CurrentTitle = string.Empty;
            CurrentRatePerMinute = 0;
            CurrentRatePerKm = 0;
            CurrentIsAvailable = false;
        }

        public void DisplayCars(List<Car> cars)
        {
            TodoListsGrid.DataSource = null;
            TodoListsGrid.DataSource = cars;

            // Configure columns for better display
            if (TodoListsGrid.Columns.Count > 0)
            {
                TodoListsGrid.Columns["Id"].HeaderText = "ID";
                TodoListsGrid.Columns["Title"].HeaderText = "Title";
                TodoListsGrid.Columns["rental_rate_per_minute"].HeaderText = "Rate/Min";
                TodoListsGrid.Columns["rental_rate_per_km"].HeaderText = "Rate/KM";
                TodoListsGrid.Columns["is_available"].HeaderText = "Available";
            }
        }

        public void ShowMessage(string message, string title, MessageType messageType)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;

            switch (messageType)
            {
                case MessageType.Warning:
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageType.Error:
                    icon = MessageBoxIcon.Error;
                    break;
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            _presenter.CreateNewCar();
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            await _presenter.SaveCar();
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog
            var confirmResult = MessageBox.Show($"Are you sure you want to delete '{CurrentTitle}'?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                await _presenter.DeleteCar();
            }
        }
    }
}