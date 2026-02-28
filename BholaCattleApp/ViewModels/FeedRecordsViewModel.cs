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
    public class FeedRecordsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Feeding _selectedFeeding;
        public ObservableCollection<Feeding> Feedings { get; set; }
        public Feeding SelectedFeeding { get => _selectedFeeding; set { _selectedFeeding = value; OnPropertyChanged(); } }

        public ICommand AddFeedingCommand { get; }
        public ICommand EditFeedingCommand { get; }
        public ICommand DeleteFeedingCommand { get; }
        public FeedRecordsViewModel(MainViewModel mainVM) 
        {
            _mainVM = mainVM;

            Feedings = new ObservableCollection<Feeding>
            {
                new Feeding { FeedingHistoryID=1, AnimalID= 2 ,StatusID = 4 , Type = "Wet Feed" , Quantity = 3, StartDate = DateTime.Now, EndDate = DateTime.Today.AddDays(30) , TotalCount = 33},
            };

            AddFeedingCommand = new RelayCommand(AddFeeding);
            EditFeedingCommand = new RelayCommand(EditFeeding,CanEditOrDelete);
            DeleteFeedingCommand = new RelayCommand(DeleteFeeding,CanEditOrDelete);
        }

        public void AddFeeding()
        {
            _mainVM.NavigateToAddEditFeed(this, null);
        }
        public void EditFeeding()
        {
            if (SelectedFeeding != null)
            {
                _mainVM.NavigateToAddEditFeed(this, SelectedFeeding);
            }
        }

        public void DeleteFeeding()
        {
            if (SelectedFeeding != null && MessageBox.Show("Delete this feed record?", "Confrim", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Feedings.Remove(SelectedFeeding);
            }
        }

        private bool CanEditOrDelete() => SelectedFeeding != null;

        public void RefreshList()
        {
            OnPropertyChanged(nameof(SelectedFeeding));
        }
    }
}
