using Core;
using GalaSoft.MvvmLight;
using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class GameStateVM : ViewModelBase
    {
        private GameState _gameStateCore;

        private string _state;

        private int _startedBy;

        private int _gameNum;

        private int _circleNum;

        private int _playerTurn;

        private List<PlayerVM> _players = new List<PlayerVM>();

        private List<CardVM> _cards = new List<CardVM>();

        private List<CardVM> _bribe = new List<CardVM>();

        private List<DeckVM> _deck = new List<DeckVM>();

        public Dictionary<string, CardSuit> GetCardSuit { get; }

        public GameState GameStateCore
        {
            get
            {
                return _gameStateCore;
            }
            set
            {
                _gameStateCore = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(State));
                RaisePropertyChanged(nameof(StartedBy));
                RaisePropertyChanged(nameof(GameNum));
                RaisePropertyChanged(nameof(CircleNum));
                RaisePropertyChanged(nameof(PlayerTurn));
                RaisePropertyChanged(nameof(Players));
                RaisePropertyChanged(nameof(Cards));
                RaisePropertyChanged(nameof(Bribe));
            }
        }

        public string State
        {
            get
            {
                return GameStateCore.State;
            }
        }

        public int StartedBy
        {
            get
            {
                return GameStateCore.StartedBy;
            }
        }
        public int GameNum
        {
            get
            {
                return GameStateCore.GameNum;
            }
        }

        public int CircleNum
        {
            get
            {
                return GameStateCore.CircleNum;
            }
        }

        public int PlayerTurn
        {
            get
            {
                return GameStateCore.PlayerTurn;
            }
        }

        public List<Player> Players
        {
            get
            {
                return GameStateCore.Players;
            }
        }

        public List<CardVM> Cards
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

        public List<CardVM> Bribe
        {
            get
            {
                return _bribe;
            }
            set
            {
                _bribe = value;
                RaisePropertyChanged();
            }
        }

        public List<DeckVM> Deck
        {
            get
            {
                return _deck;
            }
            set
            {
                _deck = value;
                RaisePropertyChanged();
            }
        }

        public GameStateVM()
        {
            
        }
    }
}
