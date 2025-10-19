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
    public class HeiferRecordsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Heifer _selectedHeifer;
        public ObservableCollection<Heifer> Heifers { get; set; }
        public Heifer SelectedHeifer { get => _selectedHeifer; set { _selectedHeifer = value; OnPropertyChanged(); } }

        public ICommand AddHeiferCommand { get; }
        public ICommand EditHeiferCommand { get; }
        public ICommand DeleteHeiferCommand { get; }

        public HeiferRecordsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            Heifers = new ObservableCollection<Heifer>
            {
                new Heifer { HeiferHistoryID = 1, AnimalID = 1, StatusID = 1, StartDate = DateTime.Now.AddMonths(-6), Weight = 450.50m, Notes = "Healthy heifer" }
                // Add more mock data
            };

            AddHeiferCommand = new RelayCommand(AddHeifer);
            EditHeiferCommand = new RelayCommand(EditHeifer, CanEditOrDelete);
            DeleteHeiferCommand = new RelayCommand(DeleteHeifer, CanEditOrDelete);
        }
        private void AddHeifer()
        {
            _mainVM.NavigateToAddEditHeifer(this, null); // New navigation method
        }
        private void EditHeifer()
        {
            if (SelectedHeifer != null)
            {
                _mainVM.NavigateToAddEditHeifer(this, SelectedHeifer);
            }
        }
        private void DeleteHeifer()
        {
            if (SelectedHeifer != null && MessageBox.Show("Delete this Heifer Record?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Heifers.Remove(SelectedHeifer);
            }
        }

        private bool CanEditOrDelete() => SelectedHeifer != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(Heifer));
        }
    }
}
