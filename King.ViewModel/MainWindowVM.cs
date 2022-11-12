using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
	public class MainWindowVM : ViewModelBase
	{
		private MainTabControlVM _mainTabControlVM = new MainTabControlVM();

		private RulesControlVM _rulesControlVM = new RulesControlVM();

		private ViewModelBase _selectedControl;

		/// <summary>
		/// Выбранный контрол
		/// </summary>
		public ViewModelBase SelectedControl
		{
			get
			{
				return _selectedControl;
			}
			set
			{
				_selectedControl = value;
				RaisePropertyChanged();
			}
		}

		public MainWindowVM()
		{
			_mainTabControlVM.OpenRulesCommand = new RelayCommand(OpenRules);
			_rulesControlVM.BackCommand = new RelayCommand(Back);
			SelectedControl = _mainTabControlVM;
		}

		private void OpenRules()
		{
			SelectedControl = _rulesControlVM;
		}

		private void Back()
		{
			SelectedControl = _mainTabControlVM;
		}
	}
}
