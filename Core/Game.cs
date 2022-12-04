using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Game : ViewModelBase
    {
        private string _sessioID;

        private GameState _state;

        public string SessioID
        {
            get
            {
                return _sessioID;
            }
            set
            {
                _sessioID = value;
                RaisePropertyChanged();
            }
        }

        public GameState State
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

        public Game()
        {

        }
    }
}
