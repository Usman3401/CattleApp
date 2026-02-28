using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Views;
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
        public ICommand NavigateToPregnantAnimalRecordsCommand { get; }
        public ICommand NavigateToMilkingAnimalRecordsCommand { get; }
        public ICommand NavigateToFeedingRecordsCommad { get; }
        public ICommand NavigateToMedicineRecordsCommad { get; }
        public ICommand NavigateToVaccineRecordsCommad { get; }
        public ICommand NavigateToTransactionRecordsCommad { get; }


        public MainViewModel()
        {
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            NavigateToAnimalRecordsCommand = new RelayCommand(NavigateToAnimalRecords);
            NavigateToStatusAnimalRecordsCommand = new RelayCommand(NavigateToStatusAnimalRecords);
            NavigateToHeiferRecordsCommand = new RelayCommand(NavigateToHeiferRecords);
            NavigateToPregnantAnimalRecordsCommand = new RelayCommand(NavigateToPregnantRecords);
            NavigateToMilkingAnimalRecordsCommand = new RelayCommand(NavigateToMilkingAnimalRecords);
            NavigateToFeedingRecordsCommad = new RelayCommand(NavigateToFeedRecords);
            NavigateToMedicineRecordsCommad = new RelayCommand(NavigateToMedicineRecords);
            NavigateToVaccineRecordsCommad = new RelayCommand(NavigateToVaccineRecords);
            NavigateToTransactionRecordsCommad = new RelayCommand(NavigateToTransactionRecords);
            NavigateToHome();
        }

        private void NavigateToHome()
        {
            var homeView = new HomeView();
            homeView.DataContext = new HomeViewModel();
            CurrentView = homeView;
        }
        #region Animal 
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
        #endregion

        #region Status
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
        #endregion

        #region heifer
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
        #endregion

        #region Pregnant
        public void NavigateToAddEditPregnant(PregnantAnimalRecordViewModel parentVM, Pregnant editingPregnant = null)
        {
            var addEditView = new AddEditPregnantView();
            addEditView.DataContext = new AddEditPregnantViewModel(this, parentVM, editingPregnant);
            CurrentView = addEditView;
        }
        public void NavigateToPregnantRecords()
        {
            var pregnantView = new PregnantAnimalRecordView();
            pregnantView.DataContext = new PregnantAnimalRecordViewModel(this);
            CurrentView = pregnantView;
        }
        #endregion

        #region Milking 
        public void NavigateToAddEditMilkingAnimal(MilkingAnimalRecordViewModel parentVM, Milking editingMilking = null)
        {
            var addEditView = new AddEditMilkingView();
            addEditView.DataContext = new AddEditMilkingViewModel(this, parentVM, editingMilking);
            CurrentView = addEditView;
        }
        public void NavigateToMilkingAnimalRecords()
        {
            var milkingView = new MilkingAnimalRecordView();
            milkingView.DataContext = new MilkingAnimalRecordViewModel(this);
            CurrentView = milkingView;
        }
        #endregion

        #region Feeding 
        public void NavigateToAddEditFeed(FeedRecordsViewModel parentVM, Feeding editingFeeding = null)
        {
            var addEditView = new AddEditFeedView();
            addEditView.DataContext = new AddEditFeedingViewModel(this, parentVM, editingFeeding);
            CurrentView = addEditView;
        }
        public void NavigateToFeedRecords()
        {
            var feedView = new FeedRecordsView();
            feedView.DataContext = new FeedRecordsViewModel(this);
            CurrentView = feedView;
        }
        #endregion

        #region Medicine
        public void NavigateToAddEditMedicine(MedicineRecordsViewModel parentVM, Medicine editingMedicine = null)
        {
            var addEditView = new AddEditMedicineView();
            addEditView.DataContext = new AddEditMedicineViewModel(this,parentVM,editingMedicine);
            CurrentView = addEditView;
        }
        public void NavigateToMedicineRecords()
        {
            var medicineView = new MedicineRecordsView();
            medicineView.DataContext = new MedicineRecordsViewModel(this);
            CurrentView = medicineView;
        }
        #endregion

        #region Vaccine
        public void NavigateToAddEditVaccine(VaccineRecordViewModel parentVM, Vaccine editingVaccine = null)
        {
            var addEditView = new AddEditVaccineView();
            addEditView.DataContext = new AddEditVaccineViewModel(this,parentVM,editingVaccine);
            CurrentView = addEditView;
        }
        public void NavigateToVaccineRecords()
        {
            var vaccineView = new VaccineRecordsView();
            vaccineView.DataContext = new VaccineRecordViewModel(this);
            CurrentView = vaccineView;
        }
        #endregion

        #region Transaction
        public void NavigateToAddEditTransaction(TransactionRecordViewModel parentVM, Transaction editingTransaction = null)
        {
            var addEditView = new AddEditTransactionView();
            addEditView.DataContext = new AddEditTransactionViewModel(this,parentVM,editingTransaction);
            CurrentView = addEditView;
        }
        public void NavigateToTransactionRecords()
        {
            var transactionView = new TransactionRecordsView();
            transactionView.DataContext = new TransactionRecordViewModel(this);
            CurrentView = transactionView;
        }
        #endregion

    }
}