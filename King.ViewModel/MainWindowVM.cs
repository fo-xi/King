using Client.WebSocketClient;
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

		private WaitingConnectionControlVM _waitingConnectionControlVM;

		private PauseControlVM _pauseControlVM;

		private PausePlayerControlVM _pausePlayerControlVM;

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

		public WaitingConnectionControlVM WaitingConnectionControlVM
		{
			get
			{
				return _waitingConnectionControlVM;

			}
			set
			{
				_waitingConnectionControlVM = value;
				RaisePropertyChanged();
			}
		}

		public PauseControlVM PauseControlVM
		{
			get
			{
				return _pauseControlVM;

			}
			set
			{
				_pauseControlVM = value;
				RaisePropertyChanged();
			}
		}

		public PausePlayerControlVM PausePlayerControlVM
		{
			get
			{
				return _pausePlayerControlVM;

			}
			set
			{
				_pausePlayerControlVM = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand CloseWindowCommand { get; set; }

		public MainWindowVM(WebSocketClient webSocketClient)
		{
			MainTabControlVM = new MainTabControlVM(webSocketClient);
			RulesControlVM = new RulesControlVM();
			StartControlVM = new StartControlVM();
			WaitingConnectionControlVM = new WaitingConnectionControlVM();
			PauseControlVM = new PauseControlVM();
			PausePlayerControlVM = new PausePlayerControlVM();

			_webSocketClient = webSocketClient;
			_webSocketClient.DataChanged += OnDataChanged;

			MainTabControlVM.OpenRulesCommand = new RelayCommand(OpenRules);
			MainTabControlVM.PauseCommand = new RelayCommand(Pause);
			RulesControlVM.BackCommand = new RelayCommand(Back);
			StartControlVM.StartGameCommand = new RelayCommand(StartGame);
			PausePlayerControlVM.ResumeGameCommand = new RelayCommand(ResumeGame);

			CloseWindowCommand = new RelayCommand(CloseWindow);

			StartControlVM.IsVisableState = true;
			MainTabControlVM.IsVisableState = false;
			RulesControlVM.IsVisableState = false;
			WaitingConnectionControlVM.IsVisableState = false;
			PauseControlVM.IsVisableState = false;
			PausePlayerControlVM.IsVisableState = false;
		}

        private void OpenRules()
		{
			RulesControlVM.IsVisableState = true;
			MainTabControlVM.IsVisableState = false;
		}
		private void Pause()
		{
			_webSocketClient.SendDataPauseGame(_webSocketClient.Game.GameSessionID);
			PausePlayerControlVM.IsVisableState = true;
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
			WaitingConnectionControlVM.IsVisableState = true;
		}

		private void ResumeGame()
		{
			_webSocketClient.SendDataResumeGame(_webSocketClient.Game.GameSessionID);
			PausePlayerControlVM.IsVisableState = false;
			MainTabControlVM.IsVisableState = true;
		}

		private void CloseWindow()
        {
			_webSocketClient.CloseClient();
		}

		private void OnDataChanged(object sender, EventArgs e)
        {
			WaitingConnectionControlVM.IsVisableState = false;

			if (_webSocketClient.Game.GameState.State == "started")
            {
				_webSocketClient.Game.GameState.PausedBy = null;
				MainTabControlVM.IsVisableState = true;
				PauseControlVM.IsVisableState = false;
			}

			if (_webSocketClient.Game.GameState.State == "paused" && 
				_webSocketClient.PlayerID != _webSocketClient.Game.GameState.PausedBy)
            {
				PauseControlVM.PlayerName = 
					_webSocketClient.Game.GameState.Players.Find(player => player.ID == _webSocketClient.Game.GameState.PausedBy).Name;
				PauseControlVM.IsVisableState = true;
				MainTabControlVM.IsVisableState = false;
			}
		}
	}
}
