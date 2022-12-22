using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class PlayerVM : ViewModelBase
    {
        private int _playerID;

        private string _playerName;

        private int _points;

        public int PlayerID
        {
            get
            {
                return _playerID;
            }
            set
            {
                _playerID = value;
                RaisePropertyChanged();
            }
        }

        public string PlayerName
        {
            get
            {
                return _playerName;
            }
            set
            {
                _playerName = value;
                RaisePropertyChanged();
            }
        }
        public int Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                RaisePropertyChanged();
            }
        }


        public PlayerVM()
        {

        }
    }
}
