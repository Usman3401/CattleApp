using BholaCattleApp.Helpers;
using BholaCattleApp.Models;
using BholaCattleApp.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class StatusAnimalRecordsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private StatusAnimal _selectedstatusAnimal;
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
        private ObservableCollection<StatusAnimal> _statusAnimals;
        public ObservableCollection<StatusAnimal> StatusAnimals 
        { 
            get => _statusAnimals;
            set { _statusAnimals = value; OnPropertyChanged(); } 
        }
        public StatusAnimal SelectedStatusAnimal { get => _selectedstatusAnimal; set { _selectedstatusAnimal = value; OnPropertyChanged(); } }

        public ICommand AddStatusAnimalCommand { get; }
        public ICommand EditStatusAnimalCommand { get; }
        public ICommand DeleteStatusAnimalCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public StatusAnimalRecordsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            AddStatusAnimalCommand = new RelayCommand(AddStatusAnimal);
            EditStatusAnimalCommand = new RelayCommand(EditStatusAnimal, CanEditOrDelete);
            DeleteStatusAnimalCommand = new RelayCommand(DeleteStatusAnimal, CanEditOrDelete);
            NextPageCommand = new RelayCommand(NextPage, () => CurrentPage < TotalPages);
            PreviousPageCommand = new RelayCommand(PreviousPage, () => CurrentPage > 1);

            LoadPage(1);
        }
        private void AddStatusAnimal()
        {
            _mainVM.NavigateToAddEditStatusAnimal(this,null);
        }
        private List<StatusAnimal> GetCheckedOrSelectedStatusAnimals()
        {
            var checkedRows = StatusAnimals?.Where(a => a.IsSelected).ToList() ?? new List<StatusAnimal>();
            if (checkedRows.Count > 0)
                return checkedRows;

            return SelectedStatusAnimal != null ? new List<StatusAnimal> { SelectedStatusAnimal } : new List<StatusAnimal>();
        }
        private void EditStatusAnimal()
        {
            var target = GetCheckedOrSelectedStatusAnimals();

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
            
            _mainVM.NavigateToAddEditStatusAnimal(this, target[0]);
        }
        private void DeleteStatusAnimal()
        {
            var toDelete = GetCheckedOrSelectedStatusAnimals();

            if (toDelete.Count == 0)
            {
                MessageBox.Show("Select at least one animal status to delete.", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string confirmText = toDelete.Count == 1
                ? $"Delete {toDelete[0].AnimalName} (Tag {toDelete[0].TagNumber})?"
                : $"Delete {toDelete.Count} selected animals status?";

            if (MessageBox.Show(confirmText, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            bool success = MSP.DeleteAnimalStatus(toDelete.Select(a => a.AnimalStatusID), out string message);

            if (!success)
            {
                MessageBox.Show(message, "Delete failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RefreshList();
        }

        private bool CanEditOrDelete() => SelectedStatusAnimal != null || (StatusAnimals?.Any(a => a.IsSelected) ?? false);
        private void NextPage() => LoadPage(CurrentPage + 1);
        private void PreviousPage() => LoadPage(CurrentPage - 1);
        private void LoadPage(int page)
        {
            int offset = (page - 1) * PageSize;

            DataTable dt = MSP.GetAnimalStatusRecords(offset, PageSize);

            var list = new ObservableCollection<StatusAnimal>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new StatusAnimal
                {
                    AnimalStatusID = Convert.ToInt32(row["ANIMALSTATUSID"]),
                    AnimalID = Convert.ToInt32(row["ANIMALID"]),
                    TagNumber = row["TAGNUMBER"] != DBNull.Value ? Convert.ToInt32(row["TAGNUMBER"]) : 0,
                    AnimalName = row["ANIMALNAME"]?.ToString(),
                    StatusID = Convert.ToInt32(row["STATUSID"]),
                    StatusName = row["STATUSNAME"]?.ToString(),
                    StartDate = row["STARTDATE"] != DBNull.Value ? Convert.ToDateTime(row["STARTDATE"]) : DateTime.MinValue,
                    EndDate = row["ENDDATE"] != DBNull.Value ? Convert.ToDateTime(row["ENDDATE"]) : (DateTime?)null,
                    Notes = row["NOTES"]?.ToString()
                });
            }

            StatusAnimals = list;
            CurrentPage = page;

            TotalRows = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TOTAL_ROWS"]) : 0;
        }

        public void RefreshList()
        {
            LoadPage(CurrentPage);
        }

    }
}
