using BholaCattleApp.Helpers;
using BholaCattleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public string TotalAnimals { get; set; } = "Total Animals = 25";
        public string MilkingCows { get; set; } = "Milking Cows: 10";
        public string Pregnant { get; set; } = "Pregnant: 5";
        public string PurchaseFeed { get; set; } = "Feed Skp (Sep 1)";
        public string PurchaseMedicine { get; set; } = "Medicine 10 dostage (Sep 1)";
        public string SellMilk { get; set; } = "Milk: 50L (Sep 1)";
        public string SellMeat { get; set; } = "Meat : 50kg (Sep 1)";

        public ICommand PrintReportCommand { get; } = new RelayCommand(PrintReport);

        private static void PrintReport()
        {
            MessageBox.Show("Printing report...");
        }
    }
}
