using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class StartControlVM : ViewModelBase
	{
		private string _playerName;

		private bool _isVisableState;

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

		public bool IsVisableState
		{
			get
			{
				return _isVisableState;
			}
			set
			{
				_isVisableState = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand StartGameCommand { get; set; }

		public StartControlVM()
        {

        }
	}
}
