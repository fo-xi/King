using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class GameState : ViewModelBase
    {
        private string _state;

        private int _startedBy;

        private int _gameNum;

        private int _circleNum;

        private int _playerTurn;

        private List<Player> _players = new List<Player>();

        private List<Card> _cards = new List<Card>();

        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        public int StartedBy
        {
            get
            {
                return _startedBy;
            }
            set
            {
                _startedBy = value;
                RaisePropertyChanged();
            }
        }

        public int GameNum
        {
            get
            {
                return _gameNum;
            }
            set
            {
                _gameNum = value;
                RaisePropertyChanged();
            }
        }

        public int CircleNum
        {
            get
            {
                return _circleNum;
            }
            set
            {
                _circleNum = value;
                RaisePropertyChanged();
            }
        }

        public int PlayerTurn
        {
            get
            {
                return _playerTurn;
            }
            set
            {
                _playerTurn = value;
                RaisePropertyChanged();
            }
        }

        public List<Player> Players
        {
            get
            {
                return _players;
            }
            set
            {
                _players = value;
                RaisePropertyChanged();
            }
        }

        public List<Card> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;
                RaisePropertyChanged();
            }
        }

        public GameState()
        {

        }
    }
}
