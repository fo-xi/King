using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class RulesControlVM : ViewModelBase
    {
        private bool _isVisableState;

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

        public RelayCommand BackCommand { get; set; }

        public RulesControlVM()
        {

        }
    }
}
