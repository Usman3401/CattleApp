using BholaCattleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.ViewModels
{
    public class PregnantAnimalRecordViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private Pregnant _selectedPregnant;
        public ObservableCollection<Pregnant> Pregnants { get; set; }  
        public Pregnant SelectedPregnant { get => _selectedPregnant; set { _selectedPregnant = value; OnPropertyChanged(); } }


        public PregnantAnimalRecordViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            Pregnants = new ObservableCollection<Pregnant> 
            {
                new Pregnant { PregnantHistoryID = 1, AnimalID = 1, StatusID = 1, PregnantDate = DateTime.Now.AddMonths(-6), 
                               DeliverDate = DateTime.Now.AddMonths(-6), Result = "Healthy", Gender = "Female" }
            };
        }
    }
}
