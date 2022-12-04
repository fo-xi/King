using Client.WebSocket;
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
		private MainTabControlVM _mainTabControlVM;

		private RulesControlVM _rulesControlVM;

		private StartControlVM _startControlVM;

		private WebSocketClient _webSocketClient;

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

		public StartControlVM StartControlVM
		{
			get
			{
				return _startControlVM;

			}
			set
			{
				_startControlVM = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand CloseWindowCommand { get; set; }

		public MainWindowVM()
		{
			_webSocketClient = new WebSocketClient();

			MainTabControlVM = new MainTabControlVM(_webSocketClient);
			RulesControlVM = new RulesControlVM();
			StartControlVM = new StartControlVM();

			MainTabControlVM.OpenRulesCommand = new RelayCommand(OpenRules);
			RulesControlVM.BackCommand = new RelayCommand(Back);
			StartControlVM.StartGameCommand = new RelayCommand(StartGame);
			CloseWindowCommand = new RelayCommand(CloseWindow);

			StartControlVM.IsVisableState = true;
			MainTabControlVM.IsVisableState = false;
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

		private void StartGame()
        {
			_webSocketClient.StartGame(StartControlVM.PlayerName);
			StartControlVM.IsVisableState = false;
			MainTabControlVM.IsVisableState = true;
		}

		private void CloseWindow()
        {
			_webSocketClient.CloseClient();

		}
	}
}
