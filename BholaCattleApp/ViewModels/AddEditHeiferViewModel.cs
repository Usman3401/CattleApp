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
    public class AddEditHeiferViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Heifer";
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public decimal Weight { get; set; }
        public string Notes { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly HeiferRecordsViewModel _parentVM;
        private readonly Heifer _editingHeifer;

        public AddEditHeiferViewModel(MainViewModel mainVM, HeiferRecordsViewModel parentVM, Heifer editingHeifer = null)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingHeifer = editingHeifer;

            if (_editingHeifer != null)
            {
                Title = "Edit Heifer";
                AnimalID = _editingHeifer.AnimalID;
                StatusID = _editingHeifer.StatusID;
                StartDate = _editingHeifer.StartDate;
                Weight = _editingHeifer.Weight;
                Notes = _editingHeifer.Notes;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            var heifer = _editingHeifer ?? new Heifer();
            heifer.AnimalID = AnimalID;
            heifer.StatusID = StatusID;
            heifer.StartDate = StartDate;
            heifer.Weight = Weight;
            heifer.Notes = Notes;

            if (_editingHeifer == null)
            {
                _parentVM.Heifers.Add(heifer);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToHeiferRecords(); // Back to list
        }

        private void Cancel()
        {
            _mainVM.NavigateToHeiferRecords(); // Back without save
        }
    }
}
