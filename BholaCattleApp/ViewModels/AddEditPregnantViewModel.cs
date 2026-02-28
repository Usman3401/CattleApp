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
    public class AddEditPregnantViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Pregnant";
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime PregnantDate { get; set; }
        public DateTime DeliverDate { get; set; }
        public string Gender { get; set; }
        public string Result { get; set; }
        public string Notes { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly PregnantAnimalRecordViewModel _parentVM;
        private readonly Pregnant _editingPregnant;

        public AddEditPregnantViewModel(MainViewModel mainVM, PregnantAnimalRecordViewModel parentVM, Pregnant editingPregnant)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingPregnant = editingPregnant;

            if (_editingPregnant != null)
            {
                Title = "Edit Pregnant";
                AnimalID = _editingPregnant.AnimalID;
                StatusID = _editingPregnant.StatusID;
                PregnantDate = _editingPregnant.PregnantDate;
                DeliverDate = _editingPregnant.DeliverDate;
                Gender = _editingPregnant.Gender;
                Result = _editingPregnant.Result;
                Notes = _editingPregnant.Notes;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            var pregnant = _editingPregnant ?? new Pregnant();
            pregnant.AnimalID = AnimalID;
            pregnant.StatusID = StatusID;            
            pregnant.PregnantDate = PregnantDate;
            pregnant.DeliverDate = DeliverDate;
            pregnant.Gender = Gender;
            pregnant.Result = Result;
            pregnant.Notes = Notes;

            if (_editingPregnant == null)
            {
               _parentVM.Pregnants.Add(pregnant);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToPregnantRecords();
        }
        private void Cancel()
        {
            _mainVM.NavigateToPregnantRecords();
        }
        
    }
}
