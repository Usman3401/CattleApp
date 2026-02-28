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
    public class AddEditMedicineViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Medicine";
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime MedicineDate { get; set; }
        public string Name { get; set; }
        public decimal Dosage { get; set; }
        public string Note { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly MedicineRecordsViewModel _parentVM;
        private readonly Medicine _editingMedicine;


        public AddEditMedicineViewModel(MainViewModel mainVM, MedicineRecordsViewModel parentVM, Medicine editingMedicine)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingMedicine = editingMedicine;

            if (_editingMedicine != null)
            {
                Title = "Edit Medicine";
                AnimalID = _editingMedicine.AnimalID;
                StatusID = _editingMedicine.StatusID;
                MedicineDate = _editingMedicine.MedicineDate;
                Name = _editingMedicine.Name;
                Dosage = _editingMedicine.Dosage;
                Note = _editingMedicine.Note;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void Save()
        {
            var medicine = _editingMedicine ?? new Medicine();
            medicine.AnimalID = AnimalID;
            medicine.StatusID = StatusID;
            medicine.MedicineDate = MedicineDate;
            medicine.Name = Name;
            medicine.Dosage = Dosage;
            medicine.Note = Note;

            if (_editingMedicine == null)
            {
                _parentVM.Medicines.Add(medicine);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToMedicineRecords();
        }
        private void Cancel() 
        {
            _mainVM.NavigateToMedicineRecords();
        }
    }
}
