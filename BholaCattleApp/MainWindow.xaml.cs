using BholaCattleApp.ViewModels;
using BholaCattleApp.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BholaCattleApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(User user = null)
        {
            InitializeComponent();
            DataContext = new MainViewModel(user);
        }
    }
}