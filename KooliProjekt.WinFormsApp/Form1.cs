using KooliProjekt.WinFormsApp.Api;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly IApiClient _apiClient;
        private List<Car> _cars;
        private Car _currentCar;

        public Form1()
        {
            InitializeComponent();
            _apiClient = new ApiClient("https://localhost:7136");

            // Wire up event handlers for the buttons
            NewButton.Click += NewButton_Click;
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;

            // Wire up the grid selection changed event
            TodoListsGrid.SelectionChanged += TodoListsGrid_SelectionChanged;

            LoadCars();
        }

        private async void LoadCars()
        {
            var result = await _apiClient.List();
            if (result.Success)
            {
                _cars = new List<Car>(result.Value);
                TodoListsGrid.DataSource = null;
                TodoListsGrid.DataSource = _cars;

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
            else
            {
                MessageBox.Show($"Error loading cars: {result.Error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TodoListsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (TodoListsGrid.CurrentRow != null)
            {
                _currentCar = TodoListsGrid.CurrentRow.DataBoundItem as Car;
                DisplayCarDetails();
            }
        }

        private void DisplayCarDetails()
        {
            if (_currentCar == null) return;

            IdField.Text = _currentCar.Id.ToString();
            TitleField.Text = _currentCar.Title;

            // Note: we don't have UI controls for these properties in the designer
            // If you want to display them, you'll need to add them to the form
            // numericUpDownRatePerMinute.Value = _currentCar.rental_rate_per_minute;
            // numericUpDownRatePerKm.Value = _currentCar.rental_rate_per_km;
            // checkBoxAvailable.Checked = _currentCar.is_available;
        }

        private void ClearFields()
        {
            IdField.Text = "0";
            TitleField.Text = "";
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            _currentCar = new Car
            {
                Id = 0,
                Title = "New car",
                rental_rate_per_minute = 0.5m,
                rental_rate_per_km = 1.0m,
                is_available = true
            };

            DisplayCarDetails();
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (_currentCar == null)
            {
                MessageBox.Show("No car selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update car with form data
            _currentCar.Title = TitleField.Text;
            // These fields aren't in the UI yet
            // _currentCar.rental_rate_per_minute = numericUpDownRatePerMinute.Value;
            // _currentCar.rental_rate_per_km = numericUpDownRatePerKm.Value;
            // _currentCar.is_available = checkBoxAvailable.Checked;

            var result = await _apiClient.Save(_currentCar);
            if (result.Success)
            {
                LoadCars(); // Refresh the list
                MessageBox.Show("Car saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error saving car: {result.Error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_currentCar == null || _currentCar.Id == 0)
            {
                MessageBox.Show("No car selected or car has not been saved yet", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Are you sure you want to delete '{_currentCar.Title}'?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var result = await _apiClient.Delete(_currentCar.Id);
                if (result.Success)
                {
                    LoadCars(); // Refresh the list
                    ClearFields();
                    MessageBox.Show("Car deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Error deleting car: {result.Error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}