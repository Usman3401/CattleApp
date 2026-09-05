using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        public int AnimalStatusID { get; set; }
        public int AnimalID { get; set; }
        public string AnimalName { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public string UserName { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private string _Message;
        public string Message
        {
            get => _Message;
            set { _Message = value; OnPropertyChanged(); }
        }
        private DataTable _animalOptions;
        public DataTable AnimalOptions
        {
            get => _animalOptions;
            set { _animalOptions = value; OnPropertyChanged(); }
        }

        private DataTable _statusOptions;
        public DataTable StatusOptions
        {
            get => _statusOptions;
            set { _statusOptions = value; OnPropertyChanged(); }
        }
        private readonly MainViewModel _mainVM;
        private readonly StatusAnimalRecordsViewModel _parentVM;
        private readonly StatusAnimal _editingStatus;

        public AddEditStatusAnimalViewModel(MainViewModel mainVM, StatusAnimalRecordsViewModel parentVM,string Username, StatusAnimal editingStatus = null)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingStatus = editingStatus;
            UserName = Username;

            AnimalOptions = MSP.GetAnimalOptions();
            StatusOptions = MSP.GetStatusOptions();

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

        private async void Save()
        {
            var status = _editingStatus ?? new StatusAnimal();
            status.AnimalID = AnimalID;
            status.StatusID = StatusID;
            status.StartDate = StartDate;
            status.EndDate = EndDate;
            status.Notes = Notes;
            
            MSP.AddEditStatusAnimal(status, UserName, out string message);
            Message = message;
            await Task.Delay(3000);
            _mainVM.NavigateToStatusAnimalRecords(); 
        }

        private void Cancel()
        {
            _mainVM.NavigateToStatusAnimalRecords(); 
        }
    }
}
