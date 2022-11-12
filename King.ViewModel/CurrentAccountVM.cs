using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class CurrentAccountVM : ViewModelBase
    {
        private int _firstPlayerScore;

        private int _secondPlayerScore;

        private int _thirdPlayerScore;

        private int _fourthPlayerScore;

        public int FirstPlayerScore
        {
            get
            {
                return _firstPlayerScore;
            }
            set
            {
                _firstPlayerScore = value;
                RaisePropertyChanged();
            }
        }

        public int SecondPlayerScore
        {
            get
            {
                return _secondPlayerScore;
            }
            set
            {
                _secondPlayerScore = value;
                RaisePropertyChanged();
            }
        }

        public int ThirdPlayerScore
        {
            get
            {
                return _thirdPlayerScore;
            }
            set
            {
                _thirdPlayerScore = value;
                RaisePropertyChanged();
            }
        }

        public int FourthPlayerScore
        {
            get
            {
                return _fourthPlayerScore;
            }
            set
            {
                _fourthPlayerScore = value;
                RaisePropertyChanged();
            }
        }

        public CurrentAccountVM()
        {
            FirstPlayerScore = 100;
            SecondPlayerScore = 100;
            ThirdPlayerScore = 100;
            FourthPlayerScore = 100;
        }
    }
}
