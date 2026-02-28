using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class TransactionRecordViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Transaction _selectedTransaction;
        public ObservableCollection<Transaction> Transactions { get; set; }
        public Transaction SelectedTransaction { get => _selectedTransaction; set { _selectedTransaction = value; OnPropertyChanged(); } }

        public ICommand AddTransactionCommand { get; }
        public ICommand EditTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }


        public TransactionRecordViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            Transactions = new ObservableCollection<Transaction>
            {
                new Transaction { TransactionHistoryID = 1, TransDate = DateTime.Now, Type = "Credit", ItemID = 1, Qty = 1, Price = 1, TotalAmount = 1900 , Note = "Kharcha Hogaya"},
            };

            AddTransactionCommand = new RelayCommand(AddTransaction);
            EditTransactionCommand = new RelayCommand(EditTransaction, CanEditOrDelete);
            DeleteTransactionCommand = new RelayCommand(DeleteTransaction, CanEditOrDelete);
        }
        public void AddTransaction()
        {
            _mainVM.NavigateToAddEditTransaction(this, null);
        }
        public void EditTransaction()
        {
            if (SelectedTransaction != null)
            {
                _mainVM.NavigateToAddEditTransaction(this, SelectedTransaction);
            }
        }

        public void DeleteTransaction()
        {
            if (SelectedTransaction != null && MessageBox.Show("Delete this transaction record?", "Confrim", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Transactions.Remove(SelectedTransaction);
            }
        }

        private bool CanEditOrDelete() => SelectedTransaction != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(SelectedTransaction));
        }
    }
}
