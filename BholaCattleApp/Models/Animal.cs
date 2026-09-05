using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Animal
    {
        [Key]
        public int AnimalID { get; set; }
        public int TagNumber { get; set; }
        public string Name { get; set; }
        public int SpeciesID { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int GenderID { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
