using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GuessNumber
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _targetNumber;
        private string _userGuess = "";
        private string _message = "Угадайте число от 1 до 100";
        private int _attempts;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeGame();
            CheckGuessCommand = new RelayCommand(_ => CheckGuess(), _ => CanCheckGuess());
        }

        public ICommand CheckGuessCommand { get; }

        public string UserGuess
        {
            get => _userGuess;
            set
            {
                _userGuess = value;
                OnPropertyChanged(nameof(UserGuess));
                // Убрали вызов RaiseCanExecuteChanged
                CommandManager.InvalidateRequerySuggested(); // Добавили эту строку
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public int Attempts
        {
            get => _attempts;
            set
            {
                _attempts = value;
                OnPropertyChanged(nameof(Attempts));
            }
        }

        private void InitializeGame()
        {
            var rnd = new Random();
            _targetNumber = rnd.Next(1, 101);
            Attempts = 0;
            UserGuess = "";
        }

        private bool CanCheckGuess()
        {
            return int.TryParse(UserGuess, out int guess) && guess >= 1 && guess <= 100;
        }

        private void CheckGuess()
        {
            if (!int.TryParse(UserGuess, out int guess)) return;

            Attempts++;

            if (guess < _targetNumber)
                Message = "Слишком маленькое!";
            else if (guess > _targetNumber)
                Message = "Слишком большое!";
            else
            {
                Message = $"Поздравляем! Вы угадали за {Attempts} попыток!";
                InitializeGame();
            }

            UserGuess = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

            public void Execute(object parameter) => _execute(parameter);

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    }
}