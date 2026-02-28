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
    public class AddEditVaccineViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Vaccine";
        public int VaccineHistoryID { get; set; }
        public string Name { get; set; }
        public decimal Qty { get; set; }
        public int Price { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly VaccineRecordViewModel _parentVM;
        private readonly Vaccine _editingVaccine;

        public AddEditVaccineViewModel(MainViewModel mainVM, VaccineRecordViewModel parentVM, Vaccine editingVaccine)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingVaccine = editingVaccine;

            if(_editingVaccine != null)
            {
                Title = "Add Vaccine";
                Name = _editingVaccine.Name;
                Qty = _editingVaccine.Qty;
                Price = _editingVaccine.Price;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        public void Save()
        {
            var vaccine = _editingVaccine ?? new Vaccine();
            vaccine.Name = Name;
            vaccine.Qty = Qty;
            vaccine.Price = Price;

            if (_editingVaccine == null)
            {
                _parentVM.Vaccines.Add(vaccine);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToVaccineRecords();
        }
        private void Cancel()
        {
            _mainVM.NavigateToVaccineRecords();
        }



    }
}
