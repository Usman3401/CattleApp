using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.ViewModels;
using BholaCattleApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BholaCattleApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        public string UserName { get; set; } = "User Name"; 

        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToAnimalRecordsCommand { get; }
        public ICommand NavigateToStatusAnimalRecordsCommand { get; }   
        public ICommand NavigateToHeiferRecordsCommand { get; }
        public ICommand NavigateTopPregnantAnimalRecordsCommand { get; }

        public MainViewModel()
        {
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            NavigateToAnimalRecordsCommand = new RelayCommand(NavigateToAnimalRecords);
            NavigateToStatusAnimalRecordsCommand = new RelayCommand(NavigateToStatusAnimalRecords);
            NavigateToHeiferRecordsCommand = new RelayCommand(NavigateToHeiferRecords);
            NavigateTopPregnantAnimalRecordsCommand = new RelayCommand(NavigateToPregnantRecords);
            NavigateToHome(); 
        }

        private void NavigateToHome()
        {
            var homeView = new HomeView();
            homeView.DataContext = new HomeViewModel();
            CurrentView = homeView;
        }
        public void NavigateToAddEditAnimal(AnimalRecordsViewModel parentVM, Animal editingAnimal)
        {
            var addEditView = new AddEditAnimalView();
            addEditView.DataContext = new AddEditAnimalViewModel(this, parentVM, editingAnimal);
            CurrentView = addEditView;
        }

        public void NavigateToAnimalRecords()
        {
            var animalView = new AnimalRecordsView(); 
            animalView.DataContext = new AnimalRecordsViewModel(this); 
            CurrentView = animalView;
        }
        public void NavigateToAddEditStatusAnimal(StatusAnimalRecordsViewModel parentVM, StatusAnimal editingAnimalStatus)
        {
            var addEditView = new AddEditStatusAnimalView();
            addEditView.DataContext = new AddEditStatusAnimalViewModel(this, parentVM, editingAnimalStatus);
            CurrentView = addEditView;
        }
        public void NavigateToStatusAnimalRecords()
        {
            var statusView = new StatusAnimalRecordsView();
            statusView.DataContext = new StatusAnimalRecordsViewModel(this);
            CurrentView = statusView;
        }
        public void NavigateToAddEditHeifer(HeiferRecordsViewModel parentVM, Heifer editingHeifer = null)
        {
            var addEditView = new AddEditHeiferView();
            addEditView.DataContext = new AddEditHeiferViewModel(this, parentVM, editingHeifer);
            CurrentView = addEditView;
        }
        public void NavigateToHeiferRecords()
        {
            var heiferView = new HeiferRecordsView();
            heiferView.DataContext = new HeiferRecordsViewModel(this);
            CurrentView = heiferView;
        }
        public void NavigateToPregnantRecords()
        {
            var pregnantView = new PregnantAnimalRecordView();
            pregnantView.DataContext = new PregnantAnimalRecordViewModel(this); 
            CurrentView = pregnantView;
        }
    }
}