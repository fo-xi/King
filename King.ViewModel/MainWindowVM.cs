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
		public MainTabControlVM _mainTabControlVM;

		public RulesControlVM _rulesControlVM;

		public MainTabControlVM MainTabControlVM 
		{
			get
            {
				return _mainTabControlVM;

			}
            set
            {
				_mainTabControlVM = value;
				RaisePropertyChanged();
            }
		}

		public RulesControlVM RulesControlVM
		{
			get
			{
				return _rulesControlVM;

			}
			set
			{
				_rulesControlVM = value;
				RaisePropertyChanged();
			}
		}

		public MainWindowVM()
		{
			MainTabControlVM = new MainTabControlVM();
			RulesControlVM = new RulesControlVM();

			MainTabControlVM.OpenRulesCommand = new RelayCommand(OpenRules);
			RulesControlVM.BackCommand = new RelayCommand(Back);

			MainTabControlVM.IsVisableState = true;
			RulesControlVM.IsVisableState = false;
		}

		private void OpenRules()
		{
			RulesControlVM.IsVisableState = true;
			MainTabControlVM.IsVisableState = false;
		}

		private void Back()
		{
			MainTabControlVM.IsVisableState = true;
			RulesControlVM.IsVisableState = false;
		}
	}
}
