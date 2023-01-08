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

		private bool _isEnabled = false;

		public string PlayerName
		{
			get
			{
				return _playerName;

			}
			set
			{
				IsEnabled = !string.IsNullOrEmpty(value);
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

		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				_isEnabled = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand StartGameCommand { get; set; }

		public StartControlVM()
        {

        }
	}
}
