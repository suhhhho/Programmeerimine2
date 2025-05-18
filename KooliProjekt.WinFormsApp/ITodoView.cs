using KooliProjekt.WinFormsApp.Api;
using System.Collections.Generic;

namespace KooliProjekt.WinFormsApp.MVP
{
    public interface ITodoView
    {
        // Properties to get/set form data
        int CurrentId { get; set; }
        string CurrentTitle { get; set; }
        decimal CurrentRatePerMinute { get; set; }
        decimal CurrentRatePerKm { get; set; }
        bool CurrentIsAvailable { get; set; }

        // Methods to update the view
        void DisplayCars(List<Car> cars);
        void ClearFields();
        void ShowMessage(string message, string title, MessageType messageType);
        void DisplayCarDetails(Car car);
    }

    public enum MessageType
    {
        Information,
        Warning,
        Error
    }
}