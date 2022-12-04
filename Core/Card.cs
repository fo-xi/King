using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Card : ViewModelBase
    {
        private string _suit;

        private int _magnitude;

        public string Suit
        {
            get
            {
                return _suit;
            }
            set
            {
                _suit = value;
                RaisePropertyChanged();
            }
        }

        public int Magnitude
        {
            get
            {
                return _magnitude;
            }
            set
            {
                _magnitude = value;
                RaisePropertyChanged();
            }
        }

        public Card()
        {

        }
    }
}
