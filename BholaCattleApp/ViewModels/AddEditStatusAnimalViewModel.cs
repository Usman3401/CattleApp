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
    public class AddEditStatusAnimalViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Status";

        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly StatusAnimalRecordsViewModel _parentVM;
        private readonly StatusAnimal _editingStatus;

        public AddEditStatusAnimalViewModel(MainViewModel mainVM, StatusAnimalRecordsViewModel parentVM, StatusAnimal editingStatus = null)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingStatus = editingStatus;

            if (_editingStatus != null)
            {
                Title = "Edit Status";
                AnimalID = _editingStatus.AnimalID;
                StatusID = _editingStatus.StatusID;
                StartDate = _editingStatus.StartDate;
                EndDate = _editingStatus.EndDate;
                Notes = _editingStatus.Notes;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            var status = _editingStatus ?? new StatusAnimal();
            status.AnimalID = AnimalID;
            status.StatusID = StatusID;
            status.StartDate = StartDate;
            status.EndDate = EndDate;
            status.Notes = Notes;

            if (_editingStatus == null)
            {
                _parentVM.StatusAnimals.Add(status);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToStatusAnimalRecords(); // Back to list
        }

        private void Cancel()
        {
            _mainVM.NavigateToStatusAnimalRecords(); // Back without save
        }
    }
}
