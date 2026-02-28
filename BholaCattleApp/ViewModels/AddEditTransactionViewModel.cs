using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class AddEditTransactionViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Transaction";
        public DateTime TransDate { get; set; }
        public string Type { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly TransactionRecordViewModel _parentVM;
        private readonly Transaction _editingTransaction;

        public AddEditTransactionViewModel(MainViewModel mainVM, TransactionRecordViewModel parentVM, Transaction editingTransaction)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingTransaction = editingTransaction;

            if (_editingTransaction != null)
            {
                Title  = "Edit Transaction";
                TransDate = _editingTransaction.TransDate;
                Type = _editingTransaction.Type;
                ItemID = _editingTransaction.ItemID;
                Qty = _editingTransaction.Qty;
                Price = _editingTransaction.Price;
                TotalAmount = _editingTransaction.TotalAmount;
                Note = _editingTransaction.Note;
            }
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void Save()
        {
            var transaction = _editingTransaction ?? new Transaction();
            transaction.TransDate = TransDate;
            transaction.Type = Type;
            transaction.ItemID = ItemID;
            transaction.Qty = Qty;
            transaction.Price = Price;
            transaction.TotalAmount = TotalAmount;
            transaction.Note = Note;

            if (_editingTransaction == null)
            {
                _parentVM.Transactions.Add(transaction);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToTransactionRecords();
        }
        private void Cancel()
        {
            _mainVM.NavigateToTransactionRecords();
        }

    }
}
