using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core
{
    public class Game : ViewModelBase
    {
        private string _sessionID;

        private GameState _gameState;

        [JsonProperty("game_session_id")]
        public string SessionID
        {
            get
            {
                return _sessionID;
            }
            set
            {
                _sessionID = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("game_state")]
        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                _gameState = value;
                RaisePropertyChanged();
            }
        }

        public Game()
        {

        }
    }
}
