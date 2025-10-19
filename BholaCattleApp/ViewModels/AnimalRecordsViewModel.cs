using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Views;
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
    public class AnimalRecordsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Animal _selectedAnimal;
        public ObservableCollection<Animal> Animals { get; set; }
        public Animal SelectedAnimal { get => _selectedAnimal; set { _selectedAnimal = value; OnPropertyChanged(); } }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public AnimalRecordsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            Animals = new ObservableCollection<Animal>
            {
                new Animal { AnimalID = 1, TagNumber = "T001", Name = "Bessie", Species = "Cow", Breed = "Holstein", Gender = "Female", DateOfBirth = DateTime.Now.AddYears(-2) },
                // Add more mock data
            };

            AddCommand = new RelayCommand(AddAnimal);
            EditCommand = new RelayCommand(EditAnimal, CanEditOrDelete);
            DeleteCommand = new RelayCommand(DeleteAnimal, CanEditOrDelete);
        }

        private void AddAnimal()
        {
            _mainVM.NavigateToAddEditAnimal(this, null); // New navigation method
        }

        // In EditAnimal
        private void EditAnimal()
        {
            if (SelectedAnimal != null)
            {
                _mainVM.NavigateToAddEditAnimal( this, SelectedAnimal);
            }
        }

        private void DeleteAnimal()
        {
            if (SelectedAnimal != null && MessageBox.Show("Delete this animal?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Animals.Remove(SelectedAnimal);
            }
        }

        private bool CanEditOrDelete() => SelectedAnimal != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(Animals));
        }
    }
}
