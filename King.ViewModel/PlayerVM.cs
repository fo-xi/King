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
        private int _id;

        private string _name;

        private int _points;

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
