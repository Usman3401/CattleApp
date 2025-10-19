using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class StatusAnimalRecordsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private StatusAnimal _selectedstatusAnimal;
        public ObservableCollection<StatusAnimal> StatusAnimals { get; set; }
        public StatusAnimal SelectedStatusAnimal { get => _selectedstatusAnimal; set { _selectedstatusAnimal = value; OnPropertyChanged(); } }

        public ICommand AddStatusAnimalCommand { get; }
        public ICommand EditStatusAnimalCommand { get; }
        public ICommand DeleteStatusAnimalCommand { get; }

        public StatusAnimalRecordsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            StatusAnimals = new ObservableCollection<StatusAnimal>
            {
                new StatusAnimal { StatusHistoryID = 1, AnimalID = 1, StatusID = 2, StartDate = DateTime.Now.AddMonths(-3), Notes = "Milking started" },
                // Add more mock data
            };

            AddStatusAnimalCommand = new RelayCommand(AddStatusAnimal);
            EditStatusAnimalCommand = new RelayCommand(EditStatusAnimal, CanEditOrDelete);
            DeleteStatusAnimalCommand = new RelayCommand(DeleteStatusAnimal, CanEditOrDelete);
        }
        private void AddStatusAnimal()
        {
            _mainVM.NavigateToAddEditStatusAnimal(this,null); // New navigation method
        }

        // In EditAnimal
        private void EditStatusAnimal()
        {
            if (SelectedStatusAnimal != null)
            {
                _mainVM.NavigateToAddEditStatusAnimal(this, SelectedStatusAnimal);
            }
        }

        private void DeleteStatusAnimal()
        {
            if (SelectedStatusAnimal != null && MessageBox.Show("Delete this status animal?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                StatusAnimals.Remove(SelectedStatusAnimal);
            }
        }

        private bool CanEditOrDelete() => SelectedStatusAnimal != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(StatusAnimals));
        }

    }
}
