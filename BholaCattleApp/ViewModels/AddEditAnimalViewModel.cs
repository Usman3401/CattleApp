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
    public class AddEditAnimalViewModel : BaseViewModel
    {
        public string Title { get; set; } = "Add Animal"; // Default; set to "Edit Animal" if editing
        public string TagNumber { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly AnimalRecordsViewModel _parentVM;
        private readonly Animal _editingAnimal; // Null for add
        private readonly MainViewModel _mainVM; // For navigation back

        public AddEditAnimalViewModel(MainViewModel mainVM, AnimalRecordsViewModel parentVM, Animal editingAnimal = null)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingAnimal = editingAnimal;

            if (_editingAnimal != null)
            {
                Title = "Edit Animal";
                TagNumber = _editingAnimal.TagNumber;
                Name = _editingAnimal.Name;
                Species = _editingAnimal.Species;
                Breed = _editingAnimal.Breed;
                Gender = _editingAnimal.Gender;
                DateOfBirth = _editingAnimal.DateOfBirth;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            var animal = _editingAnimal ?? new Animal();
            animal.TagNumber = TagNumber;
            animal.Name = Name;
            animal.Species = Species;
            animal.Breed = Breed;
            animal.Gender = Gender;
            animal.DateOfBirth = DateOfBirth;

            if (_editingAnimal == null)
            {
                _parentVM.Animals.Add(animal); // Add to collection
            }
            // Else, update existing (collection observes changes)

            _parentVM.RefreshList();
            _mainVM.NavigateToAnimalRecords(); // Navigate back to list
        }

        private void Cancel()
        {
            _mainVM.NavigateToAnimalRecords(); // Navigate back without saving
        }
    }
}
