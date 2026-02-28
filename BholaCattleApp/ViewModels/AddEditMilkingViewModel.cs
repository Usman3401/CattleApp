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
    public class AddEditMilkingViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Milking";
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public decimal MorningQty { get; set; }
        public decimal EveningQty { get; set; }
        public decimal NightQty { get; set; }
        public DateTime MilkingDate { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly MainViewModel _mainVM;
        private readonly MilkingAnimalRecordViewModel _parentVM;
        private readonly Milking _editingMilking;

        public AddEditMilkingViewModel(MainViewModel mainVM, MilkingAnimalRecordViewModel parentVM, Milking editingMilking)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingMilking = editingMilking;

            if (_editingMilking != null)
            {
                Title = "Edit Milking";
                AnimalID = _editingMilking.AnimalID;
                StatusID = _editingMilking.StatusID;
                MorningQty = _editingMilking.MorningQty;
                EveningQty = _editingMilking.EveningQty;
                NightQty = _editingMilking.NightQty;
                MilkingDate = _editingMilking.MilkingDate;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        public void Save()
        {
            var milking = _editingMilking ?? new Milking();
            milking.AnimalID = AnimalID;
            milking.StatusID = StatusID;
            milking.MorningQty = MorningQty;
            milking.EveningQty = EveningQty;
            milking.NightQty = NightQty;
            milking.MilkingDate = MilkingDate;

            if (_editingMilking == null)
            {
                _parentVM.Milkings.Add(milking);
            }

            _parentVM.RefreshList();
            _mainVM.NavigateToMilkingAnimalRecords();   
        }
        private void Cancel()
        {
            _mainVM.NavigateToMilkingAnimalRecords();
        }

    }

}
