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
    public class PregnantAnimalRecordViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Pregnant _selectedPregnant;
        public ObservableCollection<Pregnant> Pregnants { get; set; }  
        public Pregnant SelectedPregnant { get => _selectedPregnant; set { _selectedPregnant = value; OnPropertyChanged(); } }

        public ICommand AddPregnantCommand { get; }
        public ICommand EditPregnantCommand { get; }
        public ICommand DeletePregnantCommand { get; }

        public PregnantAnimalRecordViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            Pregnants = new ObservableCollection<Pregnant> 
            {
                new Pregnant { PregnantHistoryID = 1, AnimalID = 1, StatusID = 1, PregnantDate = DateTime.Now.AddMonths(-6), DeliverDate = DateTime.Now.AddMonths(-6), Result = "Healthy", Gender = "Female" }
            };

            AddPregnantCommand = new RelayCommand(AddPregnantAnimal);
            EditPregnantCommand = new RelayCommand(EditPregnantAnimal, CanEditOrDelete);
            DeletePregnantCommand = new RelayCommand(DeletePregnantAnimal, CanEditOrDelete);
        }
        private void AddPregnantAnimal()
        {
            _mainVM.NavigateToAddEditPregnant(this, null);
        }
        private void EditPregnantAnimal()
        {
            if (SelectedPregnant != null)
            {
                _mainVM.NavigateToAddEditPregnant(this, SelectedPregnant);
            }
        }
        private void DeletePregnantAnimal()
        {
            if (SelectedPregnant != null && MessageBox.Show("Delete this pregnant animal?","Confirm",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Pregnants.Remove(SelectedPregnant);
            }
        }

        private bool CanEditOrDelete() => SelectedPregnant != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(SelectedPregnant));
        }
    }
}
