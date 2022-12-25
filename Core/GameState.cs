using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private List<Card> _bribe = new List<Card>();

        [JsonProperty("state")]
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

        [JsonProperty("started_by")]
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

        [JsonProperty("game_num")]
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

        [JsonProperty("circle_num")]
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

        [JsonProperty("player_turn")]
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

        [JsonProperty("players")]
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

        [JsonProperty("cards")]
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

        [JsonProperty("bribe")]
        public List<Card> Bribe
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

        public GameState()
        {

        }
    }
}
