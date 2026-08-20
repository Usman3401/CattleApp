using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }

        private readonly Window _window;
        private readonly PasswordBox _passwordBox;
        private readonly TextBox _usernameBox;
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        
        private string _usernameError;
        public string UsernameError
        {
            get => _usernameError;
            set { _usernameError = value; OnPropertyChanged(); }
        }

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set { _passwordError = value; OnPropertyChanged(); }
        }

        private string _loginMessage; 
        public string LoginMessage
        {
            get => _loginMessage;
            set { _loginMessage = value; OnPropertyChanged(); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
        public LoginViewModel(Window window, PasswordBox passwordBox, TextBox usernameBox)
        {
            _window = window;
            _passwordBox = passwordBox;
            _usernameBox = usernameBox;
            IsLoading = false;
            Connection.OpenDatabaseConnection();
            LoginCommand = new RelayCommand(Login);
        }
        private async void Login()
        {
            string password = _passwordBox.Password;

            if (ValidateUsername() || ValidatePassword())
            {
                if (ValidatePassword())
                {
                    return;
                }
                return;
            }

            IsLoading = true;

            if (Connection._connection == null || Connection._connection.State != ConnectionState.Open)
            {
                MessageBox.Show("No active database connection. Cannot proceed with login.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = MSP.LoginVerification(Username, password, out string message);
            

            if (user != null)
            {
                await Task.Delay(2000);
                LoginMessage = message;
                await Task.Delay(1000);
                IsLoading = false;
                var mainWindow = new MainWindow(user);
                mainWindow.Show();
                _window.Close();
            }
            else
            {
                LoginMessage = message;
                IsLoading = false;
                _usernameBox?.Focus();  
                _usernameBox?.SelectAll();  
                await Task.Delay(2000);
                LoginMessage = string.Empty;
            }
        }
        private bool ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Username is required";
                return true;
            }
            else
            {
                UsernameError = string.Empty;
                return false;
            }
        }
        private bool ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(_passwordBox.Password))
            {
                PasswordError = "Password is required";
                return true;
            }
            else
            {
                PasswordError = string.Empty;
                return false;
            }
        }

    }
}
