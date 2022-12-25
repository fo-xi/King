using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class GameVM : ViewModelBase
    {
        private string _gameSessionID;

        private GameStateVM _gameStateVM;

        public string GameSessionID
        {
            get
            {
                return _gameSessionID;
            }
            set
            {
                _gameSessionID = value;
                RaisePropertyChanged();
            }
        }

        public GameStateVM GameStateVM
        {
            get
            {
                return _gameStateVM;
            }
            set
            {
                _gameStateVM = value;
                RaisePropertyChanged();
            }
        }

        public GameVM()
        {
            GameStateVM = new GameStateVM();
        }
    }
}
