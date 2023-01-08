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

		private NotStartedControlVM _notStartedControlVM;

		private GameOverControlVM _gameOverControlVM;

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

		public NotStartedControlVM NotStartedControlVM
		{
			get
			{
				return _notStartedControlVM;

			}
			set
			{
				_notStartedControlVM = value;
				RaisePropertyChanged();
			}
		}

		public GameOverControlVM GameOverControlVM
		{
			get
			{
				return _gameOverControlVM;

			}
			set
			{
				_gameOverControlVM = value;
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
			NotStartedControlVM = new NotStartedControlVM();
			GameOverControlVM = new GameOverControlVM();

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
			NotStartedControlVM.IsVisableState = false;
			GameOverControlVM.IsVisableState = false;
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
					_webSocketClient.Game.GameState.Players.Find(player => player.ID ==
					_webSocketClient.Game.GameState.PausedBy).Name;
				MainTabControlVM.IsVisableState = false;
				PauseControlVM.IsVisableState = true;
				RulesControlVM.IsVisableState = false;
				NotStartedControlVM.IsVisableState = false;
				GameOverControlVM.IsVisableState = false;
			}

			if (_webSocketClient.Game.GameState.State == "cancelled")
			{
				MainTabControlVM.IsVisableState = false;
				RulesControlVM.IsVisableState = false;
				PauseControlVM.IsVisableState = false;
				PausePlayerControlVM.IsVisableState = false;
				NotStartedControlVM.IsVisableState = true;
			}

			if (_webSocketClient.Game.GameState.State == "finished")
            {
				MainTabControlVM.IsVisableState = false;
				RulesControlVM.IsVisableState = false;

				var players = _webSocketClient.Game.GameState.Players;
				var index = players.IndexOf(players.First(player => player.ID == _webSocketClient.Game.GameState.Winner));
				int numberOtherPlayers = 3;
				int winnerIndex = 4;

				if (index != numberOtherPlayers)
				{
					var temp = players[index];
					players[index] = players[3];
					players[3] = temp;
				}

				for (int i = 0; i < numberOtherPlayers; i++)
				{
					GameOverControlVM.PlayersName[i] = players[i].Name;
					GameOverControlVM.PlayersScore[i] = players[i].Points;
				}

				GameOverControlVM.WinnerName = players[winnerIndex].Name;
				GameOverControlVM.WinnerScore = players[winnerIndex].Points;
			}
		}
	}
}
