using BholaCattleApp.Helpers;
using BholaCattleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    /*public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            // Validate credentials, navigate to Home if success
            // e.g., NavigationService.NavigateTo<HomeView>();
        }
    }*/
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; }

        private readonly Window _window;
        private readonly PasswordBox _passwordBox;

        public LoginViewModel(Window window, PasswordBox passwordBox)
        {
            _window = window;
            _passwordBox = passwordBox;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            string password = _passwordBox.Password;
            if (AuthenticationService.ValidateUser(Username, password))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                _window.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials"); // Replaced MessageBox
            }
        }
    }
}
