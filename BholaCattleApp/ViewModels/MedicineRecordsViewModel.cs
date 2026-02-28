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
    public class MedicineRecordsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Medicine _selectedMedicine;
        public ObservableCollection<Medicine> Medicines { get; set; }
        public Medicine SelectedMedicine { get => _selectedMedicine; set { _selectedMedicine = value; OnPropertyChanged(); } }

        public ICommand AddMedicineCommand { get; }
        public ICommand EditMedicineCommand { get; }
        public ICommand DeleteMedicineCommand { get; }

        public MedicineRecordsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            Medicines = new ObservableCollection<Medicine>
            {
                new Medicine { MedicineHistoryID = 1, AnimalID = 1, StatusID = 1, MedicineDate = DateTime.Now ,Name = "Panadol", Dosage = 1, Note = "Sar Dard" },
            };

            AddMedicineCommand = new RelayCommand(AddMedicine);
            EditMedicineCommand = new RelayCommand(EditMedicine, CanEditOrDelete);
            DeleteMedicineCommand = new RelayCommand(DeleteMedicine, CanEditOrDelete);
        }

        public void AddMedicine()
        {
            _mainVM.NavigateToAddEditMedicine(this, null);
        }
        public void EditMedicine()
        {
            if (SelectedMedicine != null)
            {
                _mainVM.NavigateToAddEditMedicine(this, SelectedMedicine);
            }
        }

        public void DeleteMedicine()
        {
            if (SelectedMedicine != null && MessageBox.Show("Delete this Medicine Record?", "Confrim", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Medicines.Remove(SelectedMedicine);
            }
        }

        private bool CanEditOrDelete() => SelectedMedicine != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(SelectedMedicine));
        }

    }
}
