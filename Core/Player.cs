using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Player : ViewModelBase
    {
        private int _id;

        private string _name;

        private int _points;

        [JsonProperty("player_id")]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("player_name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("points")]
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

        public Player()
        {

        }
    }
}
