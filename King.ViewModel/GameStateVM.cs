using Client.WebSocketClient;
using Core;
using GalaSoft.MvvmLight;
using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class GameStateVM : ViewModelBase
    {
        private WebSocketClient _webSocketClient;

        private string _state;

        private int? _pausedBy;

        private int _startedBy;

        private int _gameNum;

        private int _circleNum;

        private int _playerTurn;

        private List<PlayerVM> _players = new List<PlayerVM>();

        private List<CardVM> _cards = new List<CardVM>();

        private List<CardVM> _bribe = new List<CardVM>();

        private List<DeckVM> _decks = new List<DeckVM>();

        public Dictionary<string, CardSuit> GetCardSuit { get; }

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

        public int? PausedBy
        {
            get
            {
                return _pausedBy;
            }
            set
            {
                _pausedBy = value;
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

        public List<PlayerVM> Players
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

        public List<DeckVM> Decks
        {
            get
            {
                return _decks;
            }
            set
            {
                _decks = value;
                RaisePropertyChanged();
            }
        }

        public GameStateVM(WebSocketClient webSocketClient)
        {
            _webSocketClient = webSocketClient;
            _webSocketClient.DataChanged += OnDataChanged;
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            var gameState = _webSocketClient.Game.GameState;
            State = gameState.State;
            StartedBy = gameState.StartedBy;
            GameNum = gameState.GameNum;
            CircleNum = gameState.CircleNum;
            PlayerTurn = gameState.PlayerTurn;
            PausedBy = gameState.PausedBy;

            Players = new List<PlayerVM>();
            foreach (var player in gameState.Players)
            {
                Players.Add(new PlayerVM(player.ID, player.Name, player.Points));
            }

            if (Decks.Count != 0)
            {
                var deck = Decks.Find(x => x.IDPlayer == _webSocketClient.PlayerID);
                deck.UpdateCards(new ObservableCollection<Card>(gameState.Cards));
            }

            foreach (var bribe in _webSocketClient.Game.GameState.Bribe)
            {
                Bribe.Add(new CardVM(bribe.Magnitude, Convert.ToInt32(GetCardSuit[bribe.Suit]), this));
            }
        }
    }
}
