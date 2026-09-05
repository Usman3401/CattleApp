/*using BholaCattleApp.Helpers;
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

*/
using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Services;
using BholaCattleApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        private const int PageSize = 20; 

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); OnPropertyChanged(nameof(PageInfo)); }
        }

        private int _totalRows;
        public int TotalRows
        {
            get => _totalRows;
            set { _totalRows = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalPages)); OnPropertyChanged(nameof(PageInfo)); }
        }

        public int TotalPages => TotalRows == 0 ? 1 : (int)Math.Ceiling(TotalRows / (double)PageSize);

        public string PageInfo => $"Page {CurrentPage} of {TotalPages}";

        private ObservableCollection<Animal> _animals;
        public ObservableCollection<Animal> Animals
        {
            get => _animals;
            set { _animals = value; OnPropertyChanged(); }
        }

        public Animal SelectedAnimal { get => _selectedAnimal; set { _selectedAnimal = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public AnimalRecordsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            AddCommand = new RelayCommand(AddAnimal);
            EditCommand = new RelayCommand(EditAnimal, CanEditOrDelete);
            DeleteCommand = new RelayCommand(DeleteAnimal, CanEditOrDelete);
            NextPageCommand = new RelayCommand(NextPage, () => CurrentPage < TotalPages);
            PreviousPageCommand = new RelayCommand(PreviousPage, () => CurrentPage > 1);

            LoadPage(1);
        }

        private void AddAnimal()
        {
            _mainVM.NavigateToAddEditAnimal(this, null); 
        }

        private List<Animal> GetCheckedOrSelectedAnimals()
        {
            var checkedRows = Animals?.Where(a => a.IsSelected).ToList() ?? new List<Animal>();
            if (checkedRows.Count > 0)
                return checkedRows;

            return SelectedAnimal != null ? new List<Animal> { SelectedAnimal } : new List<Animal>();
        }

        private void EditAnimal()
        {
            var target = GetCheckedOrSelectedAnimals();

            if (target.Count == 0)
            {
                MessageBox.Show("Select a row to edit.", "Nothing selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (target.Count > 1)
            {
                MessageBox.Show("Select only one row to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _mainVM.NavigateToAddEditAnimal(this, target[0]);
        }

        private void DeleteAnimal()
        {
            var toDelete = GetCheckedOrSelectedAnimals();

            if (toDelete.Count == 0)
            {
                MessageBox.Show("Select at least one animal to delete.", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string confirmText = toDelete.Count == 1
                ? $"Delete {toDelete[0].Name} (Tag {toDelete[0].TagNumber})?"
                : $"Delete {toDelete.Count} selected animals?";

            if (MessageBox.Show(confirmText, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            bool success = MSP.DeleteAnimals(toDelete.Select(a => a.AnimalID), out string message);

            if (!success)
            {
                MessageBox.Show(message, "Delete failed",MessageBoxButton.OK,MessageBoxImage.Error);
            }

            RefreshList();
        }

        private bool CanEditOrDelete() => SelectedAnimal != null || (Animals?.Any(a => a.IsSelected) ?? false);

        private void NextPage() => LoadPage(CurrentPage + 1);

        private void PreviousPage() => LoadPage(CurrentPage - 1);

        private void LoadPage(int page)
        {
            int offset = (page - 1) * PageSize;

            DataTable dt = MSP.GetAnimalRecords(offset, PageSize);

            var list = new ObservableCollection<Animal>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Animal
                {
                    AnimalID = Convert.ToInt32(row["ANIMALID"]),
                    TagNumber = row["TAGNUMBER"] != DBNull.Value ? Convert.ToInt32(row["TAGNUMBER"]) : 0,
                    Name = row["NAME"]?.ToString(),
                    SpeciesID = Convert.ToInt32(row["FK_SPECIESID"]),
                    Species = row["SPECIESNAME"]?.ToString(),
                    Breed = row["BREED"]?.ToString(),
                    GenderID = Convert.ToInt32(row["FK_GENDERID"]),
                    Gender = row["GENDERNAME"]?.ToString(),
                    DateOfBirth = row["DATEOFBIRTH"] != DBNull.Value ? Convert.ToDateTime(row["DATEOFBIRTH"]) : DateTime.MinValue
                });
            }

            Animals = list;
            CurrentPage = page;

            TotalRows = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TOTAL_ROWS"]) : 0;
        }

        // Call this after Add/Edit completes and navigates back here,
        // so the grid reflects the change without losing the current page.
        public void RefreshList()
        {
            LoadPage(CurrentPage);
        }
    }
}