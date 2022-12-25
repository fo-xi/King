using Client.WebSocketClient;
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

		private CurrentAccountVM _currentAccountVM;

		private bool _isVisableState;

		private string _currentNameGame;

		private WebSocketClient _webSocketClient;

		private string _playerMove;

		private ObservableCollection<string> _playerNames;

		#endregion

		#region Player fields

		private string _firstPlayer;

		private string _secondPlayer;

		private string _thirdPlayer;

		private string _fourthPlayer;

		#endregion

		#region Properties

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

		#endregion

		#region Player зroperties

		public string FirstPlayer
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

		public string SecondPlayer
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

		public string ThirdPlayer
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

		public string FourthPlayer
		{
			get
			{
				return _fourthPlayer;
			}
			set
			{
				_fourthPlayer = value;
				RaisePropertyChanged();
			}
		}

		#endregion

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

		#region Commands

		public RelayCommand OpenRulesCommand { get; set; }

		#endregion

		public MainTabControlVM(WebSocketClient webSocketClient)
		{
			_webSocketClient = webSocketClient;
			_webSocketClient.DataChanged += OnDataChanged;

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
			PlayerMove = $"Ход игрока {_webSocketClient.Game.GameState.Players.Find(x => x.ID == _webSocketClient.Game.GameState.PlayerTurn).Name}";
			CurrentNameGame = _webSocketClient.Game.GameState.GameNum.ToString();
		}

		#endregion
	}
}
