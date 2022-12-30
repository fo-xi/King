using Client.WebSocketClient;
using Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
	public class MainTabControlVM : ViewModelBase
	{
		#region Fields

		private GameVM _gameVM;

		private CurrentAccountVM _currentAccountVM;

		private bool _isVisableState;

		private string _currentNameGame;

		private WebSocketClient _webSocketClient;

		private string _playerMove;

		private ObservableCollection<string> _playerNames;

		private PlayerVM _firstPlayer;

		private PlayerVM _secondPlayer;

		private PlayerVM _thirdPlayer;

		private int _currentPlayerTurn;

        private List<Card> _oldBribe;

        #endregion

        #region Properties

        public GameVM GameVM
		{
			get
			{
				return _gameVM;
			}
			set
			{
				_gameVM = value;
				RaisePropertyChanged();
			}
		}

		public CurrentAccountVM CurrentAccountVM
		{
			get
			{
				return _currentAccountVM;
			}
			set
			{
				_currentAccountVM = value;
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

		public string CurrentNameGame
		{
			get
			{
				return _currentNameGame;
			}
			set
			{
				_currentNameGame = GetNameGame(value);
				RaisePropertyChanged();
			}
		}

		public string PlayerMove
		{
			get
			{
				return _playerMove;
			}
			set
			{
				_playerMove = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> PlayerNames
		{
			get
			{
				return _playerNames;
			}
			set
			{
				_playerNames = value;
				RaisePropertyChanged();
			}
		}

		public PlayerVM FirstPlayer
		{
			get
			{
				return _firstPlayer;
			}
			set
			{
				_firstPlayer = value;
				RaisePropertyChanged();
			}
		}

		public PlayerVM SecondPlayer
		{
			get
			{
				return _secondPlayer;
			}
			set
			{
				_secondPlayer = value;
				RaisePropertyChanged();
			}
		}

		public PlayerVM ThirdPlayer
		{
			get
			{
				return _thirdPlayer;
			}
			set
			{
				_thirdPlayer = value;
				RaisePropertyChanged();
			}
		}

		public int CurrentPlayerTurn
		{
			get
			{
				return _currentPlayerTurn;
			}
			set
			{
				_currentPlayerTurn = value;
				RaisePropertyChanged();
			}
		}

        public List<Card> OldBribe
        {
            get
            {
                return _oldBribe;
            }
            set
            {
                _oldBribe = value;
                RaisePropertyChanged();
            }
        }

		#endregion

		#region Events

		public event EventHandler<(Card bribeCard, int currentPlayerTurn)> FoundNewBribeCard;

		#endregion

		#region Commands

		public RelayCommand OpenRulesCommand { get; set; }

		public RelayCommand PauseCommand { get; set; }

		#endregion

		public MainTabControlVM(WebSocketClient webSocketClient)
		{
			_webSocketClient = webSocketClient;
			_webSocketClient.DataChanged += OnDataChanged;

			GameVM = new GameVM(_webSocketClient);
			CurrentAccountVM = new CurrentAccountVM();

			_playerNames = new ObservableCollection<string>
			{
				"Player 1",
				"Player 2",
				"Player 3",
				"Player 4"
			};
		}

		#region Methods

		private void NameInitialization()
        {
			var players = _webSocketClient.Game.GameState.Players;
			var index = players.IndexOf(players.First(player => player.ID == _webSocketClient.PlayerID));

			if (index != 3)
            {
				var temp = players[index];
				players[index] = players[3];
				players[3] = temp;
            }

			for (int i = 0; i < players.Count; i++)
			{
				PlayerNames[i] = players[i].Name;
			}

			FirstPlayer = new PlayerVM(players[0].ID, players[0].Name, players[0].Points);
			SecondPlayer = new PlayerVM(players[1].ID, players[1].Name, players[1].Points);
			ThirdPlayer = new PlayerVM(players[2].ID, players[2].Name, players[2].Points);
		}

		private string GetNameGame(string gameNumber)
		{
			switch (gameNumber)
			{
				case "1":
				{
				 return "Не брать взяток";
				}
				case "2":
				{
					return "Не брать мальчиков";
				}
				case "3":
				{
					return "Не брать девочек";
				}
				case "4":
				{
					return "Не брать червей";
				}
				case "5":
				{
					return "Не брать Кинга";
				}
				case "6":
				{
					return "Не брать 2 последние взятки";
				}
				case "7":
				{
					return "Брать взятки";
				}
				case "8":
				{
					return "Брать мальчиков";
				}
				case "9":
				{
					return "Брать девочек";
				}
				case "10":
				{
					return "Брать червей";
				}
				case "11":
				{
					return "Брать Кинга";
				}
				case "12":
				{
					return "Брать 2 последние взятки";
				}
				default:
				{
					return string.Empty;
				}
			}
		}

		private void OnDataChanged(object sender, EventArgs e)
		{
			NameInitialization();

			if (_webSocketClient.Game.GameState.PlayerTurn != 0)
			{
				PlayerMove = _webSocketClient.Game.GameState.Players.Find(x => x.ID == _webSocketClient.Game.GameState.PlayerTurn).Name;
			}
			CurrentNameGame = _webSocketClient.Game.GameState.GameNum.ToString();

			//Находим карту которую добавили во взятку
            if (_webSocketClient.Game.GameState.Players.Any(player => player.ID == CurrentPlayerTurn) && OldBribe != null)
            {
				Card addedCard = null;
				foreach (var bribe in _webSocketClient.Game.GameState.Bribe)
                {
					if (OldBribe.Any(oldBribe => oldBribe.Magnitude != bribe.Magnitude && oldBribe.Suit != bribe.Suit))
                    {
						addedCard = bribe;
						break;
					}
                }

				if (addedCard != null)
                {
					FoundNewBribeCard?.Invoke(this, (addedCard, CurrentPlayerTurn));
					//Сообщить вью что мы нашли карту и отправить ее и CurrentPlayerTurn
					//и через CurrentPlayerTurn определить трики
				}

			}
            CurrentPlayerTurn = _webSocketClient.Game.GameState.PlayerTurn;
            OldBribe = _webSocketClient.Game.GameState.Bribe;

            CurrentAccountVM.FirstPlayerScore = FirstPlayer.Points;
			CurrentAccountVM.SecondPlayerScore = SecondPlayer.Points;
			CurrentAccountVM.ThirdPlayerScore = ThirdPlayer.Points;
			CurrentAccountVM.FourthPlayerScore 
				= _webSocketClient.Game.GameState.Players.Find(player => player.ID == _webSocketClient.PlayerID).Points;

			CurrentAccountVM.FirstPlayerName = FirstPlayer.Name;
			CurrentAccountVM.SecondPlayerName = SecondPlayer.Name;
			CurrentAccountVM.ThirdPlayerName = ThirdPlayer.Name;
			CurrentAccountVM.FourthPlayerName
				= _webSocketClient.Game.GameState.Players.Find(player => player.ID == _webSocketClient.PlayerID).Name;
		}

		#endregion
	}
}
