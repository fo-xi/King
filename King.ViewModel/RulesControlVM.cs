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
        public RelayCommand BackCommand { get; set; }

        public RulesControlVM()
        {

        }
    }
}
