using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core
{
    public class Card : ViewModelBase
    {
        private string _suit;

        private int _magnitude;

        [JsonProperty("suit")]
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

        [JsonProperty("magnitude")]
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
