using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class AddEditFeedingViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Feeding";
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCount { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public readonly MainViewModel _mainVM;
        public readonly FeedRecordsViewModel _parentVM;
        public readonly Feeding _editingFeeding;

        public AddEditFeedingViewModel(MainViewModel mainVM,FeedRecordsViewModel parentVM, Feeding editingFeeding)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;   
            _editingFeeding = editingFeeding;

            if (_editingFeeding != null)
            {
                Title = "Editing Feeding";
                AnimalID = _editingFeeding.AnimalID;
                StatusID = _editingFeeding.StatusID;    
                Type = _editingFeeding.Type;    
                Quantity = _editingFeeding.Quantity;
                StartDate = _editingFeeding.StartDate;
                EndDate = _editingFeeding.EndDate;
                TotalCount = _editingFeeding.TotalCount;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void Save()
        {
            var feeding = _editingFeeding ?? new Feeding();
            feeding.AnimalID = AnimalID;
            feeding.StatusID = StatusID;
            feeding.Type = Type;
            feeding.Quantity = Quantity;
            feeding.StartDate = StartDate;
            feeding.EndDate = EndDate;
            feeding.TotalCount = TotalCount;


            if (_editingFeeding == null)
            {
                _parentVM.Feedings.Add(feeding);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToFeedRecords();
        }

        private void Cancel()
        {
            _mainVM.NavigateToFeedRecords();    
        }
    }

    
}
