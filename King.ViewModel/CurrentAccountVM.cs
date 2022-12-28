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
        private string _firstPlayerName;

        private int _firstPlayerScore;

        private string _secondPlayerName;

        private int _secondPlayerScore;

        private string _thirdPlayerName;

        private int _thirdPlayerScore;

        private string _fourthPlayerName;

        private int _fourthPlayerScore;

        public string FirstPlayerName
        {
            get
            {
                return _firstPlayerName;
            }
            set
            {
                _firstPlayerName = value;
                RaisePropertyChanged();
            }
        }

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

        public string SecondPlayerName
        {
            get
            {
                return _secondPlayerName;
            }
            set
            {
                _secondPlayerName = value;
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

        public string ThirdPlayerName
        {
            get
            {
                return _thirdPlayerName;
            }
            set
            {
                _thirdPlayerName = value;
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

        public string FourthPlayerName
        {
            get
            {
                return _fourthPlayerName;
            }
            set
            {
                _fourthPlayerName = value;
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
            
        }
    }
}
