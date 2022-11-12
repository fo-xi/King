using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class MainTabControlVM : ViewModelBase
    {
        private CurrentAccountVM _currentAccountVM;

        private bool _isVisableState;

        private string _currentNameGame;

        public CurrentAccountVM CurrentAccountVM
        {
            get
            {
                return _currentAccountVM;
            }
            set
            {
                _currentAccountVM = value;
                RaisePropertyChanged();
            }
        }

        public bool IsVisableState
        {
            get
            {
                return _isVisableState;
            }
            set
            {
                _isVisableState = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentNameGame
        {
            get
            {
                return _currentNameGame;
            }
            set
            {
                _currentNameGame = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenRulesCommand { get; set; }

        public MainTabControlVM()
        {
            CurrentAccountVM = new CurrentAccountVM();
            CurrentNameGame = "Не брать 2х последних";
        }
    }
}
