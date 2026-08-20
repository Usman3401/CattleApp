using BholaCattleApp.Services;
using BholaCattleApp.ViewModels;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BholaCattleApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            fd_Username.Focus();
            DataContext = new LoginViewModel(this, PasswordBox, fd_Username);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var focusedElement = Keyboard.FocusedElement as UIElement;
                if (focusedElement != null)
                {
                    focusedElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                e.Handled = true;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            if (Connection._connection != null && Connection._connection.State != System.Data.ConnectionState.Closed)
            {
                try
                {
                    Connection._connection.Close();
                    Connection._connection.Dispose();
                }
                catch { }
            }

            base.OnClosed(e);
        }
    }
}
