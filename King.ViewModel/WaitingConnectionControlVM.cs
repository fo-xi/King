using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class WaitingConnectionControlVM : ViewModelBase
    {
		private bool _isVisableState;

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

		public WaitingConnectionControlVM()
		{

		}
	}
}
