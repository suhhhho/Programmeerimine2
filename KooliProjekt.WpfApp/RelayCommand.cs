using System;
using System.Windows.Input;

namespace KooliProjekt.WpfApp
{
    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null ||
                       (parameter == null && _canExecute(default(T)) ||
                        parameter is T && _canExecute((T)parameter));
            }
            catch
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            try
            {
                // Если параметр null, передаем default(T), иначе пытаемся привести к T
                if (parameter == null)
                    _execute(default(T));
                else if (parameter is T)
                    _execute((T)parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в Execute RelayCommand: {ex.Message}");
            }
        }
    }
}