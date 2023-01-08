using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class GameOverControlVM : ViewModelBase
    {
        private ObservableCollection<string> _playersName;

        private ObservableCollection<int> _playersScore;

        private string _winnerName;

        private int _winnerScore;

        private bool _isVisableState;

        public ObservableCollection<string> PlayersName
        {
            get
            {
                return _playersName;
            }
            set
            {
                _playersName = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> PlayersScore
        {
            get
            {
                return _playersScore;
            }
            set
            {
                _playersScore = value;
                RaisePropertyChanged();
            }
        }

        public string WinnerName
        {
            get
            {
                return _winnerName;
            }
            set
            {
                _winnerName = value;
                RaisePropertyChanged();
            }
        }

        public int WinnerScore
        {
            get
            {
                return _winnerScore;
            }
            set
            {
                _winnerScore = value;
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

        public GameOverControlVM()
        {

        }
    }
}
