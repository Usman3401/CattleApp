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
    public class MilkingAnimalRecordViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Milking _selectedMilking;
        public ObservableCollection<Milking> Milkings { get; set; }
        public Milking SelectedMilking { get => _selectedMilking; set { _selectedMilking = value ; OnPropertyChanged(); } }

        public ICommand AddMilkingCommand { get; }
        public ICommand EditMilkingCommand { get; }
        public ICommand DeleteMilkingCommand { get; }

        public MilkingAnimalRecordViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            Milkings = new ObservableCollection<Milking>
            {
                new Milking { MilkingHistoryID = 1, AnimalID = 1, StatusID = 1, MorningQty = 1, EveningQty = 1, NightQty = 1, MilkingDate = DateTime.Now },
            };

            AddMilkingCommand = new RelayCommand(AddMilkingAnimal);
            EditMilkingCommand = new RelayCommand(EditMilkingAnimal, CanEditOrDelete);
            DeleteMilkingCommand = new RelayCommand(DeleteMilkingAnimal, CanEditOrDelete);
        }

        public void AddMilkingAnimal()
        {
            _mainVM.NavigateToAddEditMilkingAnimal(this, null);
        }
        public void EditMilkingAnimal() 
        {
            if (SelectedMilking != null)
            {
                _mainVM.NavigateToAddEditMilkingAnimal(this, SelectedMilking);
            }
        }

        public void DeleteMilkingAnimal()
        {
            if (SelectedMilking != null && MessageBox.Show("Delete this milking animal?","Confrim",MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
            {
                Milkings.Remove(SelectedMilking);
            }
        }

        private bool CanEditOrDelete() => SelectedMilking != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(SelectedMilking));
        }
    }
}
