using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class AddEditAnimalViewModel : BaseViewModel 
    {
        public string Title { get; set; } = "Add Animal"; 
        public int AnimalID { get; set; }   
        public int TagNumber { get; set; }
        public string Name { get; set; }
        public int SpeciesID { get; set; }
        public string Breed { get; set; }
        public int GenderID { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string UserName { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private string _Message;
        public string Message
        {
            get => _Message;
            set { _Message = value; OnPropertyChanged(); }
        }

        private DataTable _genderOptions;
        public DataTable GenderOptions
        {
            get => _genderOptions;
            set { _genderOptions = value; OnPropertyChanged(); }
        }

        private DataTable _speciesOptions;
        public DataTable SpeciesOptions
        {
            get => _speciesOptions;
            set { _speciesOptions = value; OnPropertyChanged(); }
        }

        private readonly AnimalRecordsViewModel _parentVM;
        private readonly Animal _editingAnimal; 
        private readonly MainViewModel _mainVM; 
        

        public AddEditAnimalViewModel(MainViewModel mainVM, AnimalRecordsViewModel parentVM, string Username, Animal editingAnimal = null)
        {
            _mainVM = mainVM;
            _parentVM = parentVM;
            _editingAnimal = editingAnimal;
            UserName = Username;

            GenderOptions = MSP.GetGenderOptions();
            SpeciesOptions = MSP.GetSpeciesOptions();

            if (_editingAnimal != null)
            {
                Title = "Edit Animal";
                AnimalID = _editingAnimal.AnimalID;
                TagNumber = _editingAnimal.TagNumber;
                Name = _editingAnimal.Name;
                SpeciesID = _editingAnimal.SpeciesID;
                Breed = _editingAnimal.Breed;
                GenderID = _editingAnimal.GenderID;
                DateOfBirth = _editingAnimal.DateOfBirth;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        private async void Save()
        {
            if (SpeciesID <= 0)
            {
                Message = "Please select a species.";
                return;
            }

            if (GenderID <= 0)
            {
                Message = "Please select a gender.";
                return;
            }
            var animal = _editingAnimal ?? new Animal();
            animal.AnimalID = AnimalID;
            animal.TagNumber = TagNumber;
            animal.Name = Name;
            animal.SpeciesID = SpeciesID;
            animal.Breed = Breed;
            animal.GenderID = GenderID;
            animal.DateOfBirth = DateOfBirth;

            MSP.AddEditAnimal(animal, UserName , out string message);
            Message = message;  
            await Task.Delay(3000);
            _mainVM.NavigateToAnimalRecords(); 
        }

        private void Cancel()
        {
            _mainVM.NavigateToAnimalRecords(); 
        }
    }
}
