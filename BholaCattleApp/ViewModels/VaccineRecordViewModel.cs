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
    public class VaccineRecordViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Vaccine _selectedVaccine;
        public ObservableCollection<Vaccine> Vaccines { get; set; }
        public Vaccine SelectedVaccine { get => _selectedVaccine; set { _selectedVaccine = value; OnPropertyChanged(); } }

        public ICommand AddVaccineCommand { get; }
        public ICommand EditVaccineCommand { get; }
        public ICommand DeleteVaccineCommand { get; }

        public VaccineRecordViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            Vaccines = new ObservableCollection<Vaccine>
            {
                new Vaccine { VaccineHistoryID = 1, Name = "ABC Vaccine", Qty = 1, Price = 1 },
            };

            AddVaccineCommand = new RelayCommand(AddVaccine);
            EditVaccineCommand = new RelayCommand(EditVaccine, CanEditOrDelete);
            DeleteVaccineCommand = new RelayCommand(DeleteVaccine, CanEditOrDelete);
        }

        public void AddVaccine()
        {
            _mainVM.NavigateToAddEditVaccine(this, null);
        }
        public void EditVaccine()
        {
            if (SelectedVaccine != null)
            {
                _mainVM.NavigateToAddEditVaccine(this, SelectedVaccine);
            }
        }

        public void DeleteVaccine()
        {
            if (SelectedVaccine != null && MessageBox.Show("Delete this vaccine?", "Confrim", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Vaccines.Remove(SelectedVaccine);
            }
        }

        private bool CanEditOrDelete() => SelectedVaccine != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(SelectedVaccine));
        }
    }
}
