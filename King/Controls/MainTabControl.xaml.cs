using Client.WebSocketClient;
using Core;
using King.ViewModel;
using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace King.Controls
{
	/// <summary>
	/// Логика взаимодействия для MainTabControl.xaml
	/// </summary>
	public partial class MainTabControl : UserControl
	{
		#region Constants

		private const int CardsPerPlayer = 8;

		private const int NumberAllCards = 32;

		private const int NumberDecks = 4;

		#endregion

		#region Fields

		private WebSocketClient _webSocketClient;

		private MainTabControlVM _mainTabControlVM;

		private DeckVM _dealer;

		private bool _isNewGame = true;

		private Dictionary<int, DeckVM> _playerBribeDecks;

		private int _oldGameNum = 1;

		private int _oldCircleNum = 0;

		private bool _isNewOldGameNum = false;

		private DeckVM _bin;

		private int _firstPlayerMove;

		private DeckVM _trump;

		#endregion

		#region Events

		public event EventHandler DataChangedControl;

		#endregion

		#region Constructor

		public MainTabControl(WebSocketClient webSocketClient, MainTabControlVM dataContext)
		{
			InitializeComponent();

			_webSocketClient = webSocketClient;
			_webSocketClient.DataChanged += OnDataChanged;

			_mainTabControlVM = dataContext;
			DataContext = dataContext;
			dataContext.FoundNewBribeCard += OnFoundNewBribeCard;

			_playerBribeDecks = new Dictionary<int, DeckVM>();
		}

        #endregion

        #region Methods

        private void DrawCards(DeckVM deckVM)
        {
			for (var cardCount = 0; cardCount < CardsPerPlayer; cardCount++)
			{
				_dealer.Draw(deckVM, 1);
			}
		}

		private void NewGame()
		{
			GameShape.GameStateVM = _mainTabControlVM.GameVM.GameStateVM;
			SetupDealerDeck();
			SetupPlayerHandDecks();
			SetupTrickDecks();
		}

		private void DealNextHand()
		{
			if (_dealer.Cards.Count < NumberAllCards)
			{
				CollectCards();
			}

			DrawCards(Player1Hand.Deck);
			DrawCards(Player2Hand.Deck);
			DrawCards(Player3Hand.Deck);
			DrawCards(Player4Hand.Deck);

			Player4Hand.Deck.MakeAllCardsDragable(true);
			Player4Hand.Deck.FlipAllCards();
		}

		private void CollectCards()
		{
			Player1Hand.Deck.Draw(_dealer, Player1Hand.Deck.Cards.Count);
			Player2Hand.Deck.Draw(_dealer, Player2Hand.Deck.Cards.Count);
			Player3Hand.Deck.Draw(_dealer, Player3Hand.Deck.Cards.Count);
			Player4Hand.Deck.FlipAllCards();
			Player4Hand.Deck.Draw(_dealer, Player1Hand.Deck.Cards.Count);
		}

		private void SetupDealerDeck()
		{
			_dealer = new DeckVM(NumberDecks, 
				new ObservableCollection<Card>(_webSocketClient.Game.GameState.Cards),
				GameShape.GameStateVM, _webSocketClient.PlayerID);

			_bin = new DeckVM(GameShape.GameStateVM);

			_dealer.MakeAllCardsDragable(false);
			_dealer.Enabled = true;
			_dealer.FlipAllCards();

			_bin.MakeAllCardsDragable(false);
			_bin.Enabled = true;
			_bin.FlipAllCards();

			Dealer.Deck = _dealer;
			Bin.Deck = _bin;

			if (!_isNewOldGameNum)
			{
				GameShape.DeckShapes.Add(Dealer);
				GameShape.DeckShapes.Add(Bin);

				_trump = new DeckVM(GameShape.GameStateVM);
				_trump.MakeAllCardsDragable(false);
				_trump.Enabled = true;
				_trump.FlipAllCards();
				Trump.Deck = _trump;
				GameShape.DeckShapes.Add(Trump);
			}
		}

		private void SetupTrickDecks()
		{
			Player1Trick.Deck = new DeckVM(GameShape.GameStateVM, _mainTabControlVM.FirstPlayer.ID)
			{
				Enabled = true
			};

			Player2Trick.Deck = new DeckVM(GameShape.GameStateVM, _mainTabControlVM.SecondPlayer.ID)
			{
				Enabled = true
			};

			Player3Trick.Deck = new DeckVM(GameShape.GameStateVM, _mainTabControlVM.ThirdPlayer.ID)
			{
				Enabled = true
			};

			Player4Trick.Deck = new DeckVM(GameShape.GameStateVM, _webSocketClient.PlayerID)
			{
				Enabled = true
			};

			//Сопоставление id и триков
			_playerBribeDecks.Add(_mainTabControlVM.FirstPlayer.ID, Player1Trick.Deck);
			_playerBribeDecks.Add(_mainTabControlVM.SecondPlayer.ID, Player2Trick.Deck);
			_playerBribeDecks.Add(_mainTabControlVM.ThirdPlayer.ID, Player3Trick.Deck);
			_playerBribeDecks.Add(_webSocketClient.PlayerID, Player4Trick.Deck);

			Player1Trick.Deck.MakeAllCardsDragable(false);
			Player2Trick.Deck.MakeAllCardsDragable(false);
			Player3Trick.Deck.MakeAllCardsDragable(false);
			Player4Trick.Deck.MakeAllCardsDragable(false);

			if (!_isNewOldGameNum)
			{
				GameShape.DeckShapes.Add(Player1Trick);
				GameShape.DeckShapes.Add(Player2Trick);
				GameShape.DeckShapes.Add(Player3Trick);
				GameShape.DeckShapes.Add(Player4Trick);
			}
		}

		private void SetupPlayerHandDecks()
		{
			Player1Hand.Deck = new DeckVM(GameShape.GameStateVM, _mainTabControlVM.FirstPlayer.ID)
			{
				Enabled = true
			};
			Player2Hand.Deck = new DeckVM(GameShape.GameStateVM, _mainTabControlVM.SecondPlayer.ID)
			{
				Enabled = true
			};
			Player3Hand.Deck = new DeckVM(GameShape.GameStateVM, _mainTabControlVM.ThirdPlayer.ID)
			{
				Enabled = true
			};
			Player4Hand.Deck = new DeckVM(GameShape.GameStateVM, _webSocketClient.PlayerID)
			{
				Enabled = true
			};

			Player1Hand.Deck.MakeAllCardsDragable(true);
			Player2Hand.Deck.MakeAllCardsDragable(true);
			Player3Hand.Deck.MakeAllCardsDragable(true);
			Player4Hand.Deck.MakeAllCardsDragable(true);

			if (!_isNewOldGameNum)
			{
				GameShape.DeckShapes.Add(Player1Hand);
				GameShape.DeckShapes.Add(Player2Hand);
				GameShape.DeckShapes.Add(Player3Hand);
				GameShape.DeckShapes.Add(Player4Hand);
			}
		}

		private void ClearData()
        {
			_playerBribeDecks.Clear();
			foreach(var deckShape in GameShape.DeckShapes)
            {
				deckShape.Deck.Cards.Clear();
				deckShape.UpdateCardShapes();
            }
		}

		private void SendBin()
        {
			Thread.Sleep(1000);
			Application.Current.Dispatcher.Invoke(() =>
			{
				while (Player1Trick.Deck.Cards.Count != 0)
				{
					var card = Player1Trick.Deck.Cards[0];
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _bin;
					_bin.Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				}

				while (Player2Trick.Deck.Cards.Count != 0)
				{
					var card = Player2Trick.Deck.Cards[0];
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _bin;
					_bin.Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				}

				while (Player3Trick.Deck.Cards.Count != 0)
				{
					var card = Player3Trick.Deck.Cards[0];
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _bin;
					_bin.Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				}

				while (Player4Trick.Deck.Cards.Count != 0)
				{
					var card = Player4Trick.Deck.Cards[0];
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _bin;
					_bin.Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				}

				while (Trump.Deck.Cards.Count != 0)
				{
					var card = Trump.Deck.Cards[0];
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _bin;
					_bin.Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				}
			});
		}

		#endregion

		#region Event Handlers

		private void OnDataChanged(object sender, EventArgs e)
		{
			if (_oldCircleNum != _webSocketClient.Game.GameState.CircleNum)
			{
				if (_webSocketClient.Game.GameState.CircleNum != 1)
				{
					SendBin();
				}

				_oldCircleNum = _webSocketClient.Game.GameState.CircleNum;
				_firstPlayerMove = _webSocketClient.Game.GameState.PlayerTurn;
			}
			
			if (_oldGameNum != _webSocketClient.Game.GameState.GameNum)
			{
				_isNewGame = true;
				_isNewOldGameNum = true;

				SendBin();

				_oldGameNum = _webSocketClient.Game.GameState.GameNum;
			}

			if (_isNewGame)
            {
				Application.Current.Dispatcher.Invoke(() =>
				{
					ClearData();
					NewGame();
					DealNextHand();
				});
				_isNewGame = false;
			}
		}

		private void OnFoundNewBribeCard(object sender, (Card bribeCard, int currentPlayerTurn) e)
		{
			var getCardSuit = new Dictionary<string, CardSuit>
			{
				{ "hearts", CardSuit.Hearts },
				{ "clubs", CardSuit.Clubs },
				{ "diamonds", CardSuit.Diamonds },
				{ "spades", CardSuit.Spades }
			}; 
			
			if (_firstPlayerMove == e.currentPlayerTurn)
			{
				var card = new CardVM(e.bribeCard.Magnitude,
					Convert.ToInt32(getCardSuit[e.bribeCard.Suit]), _trump);

				Application.Current.Dispatcher.Invoke(() =>
				{
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _trump;
					_trump.Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				});
			}

			if (e.currentPlayerTurn == _webSocketClient.PlayerID)
            {
				return;
            }

			if (_webSocketClient.Game.GameState.Bribe != null)
			{
				var card = new CardVM(e.bribeCard.Magnitude,
					Convert.ToInt32(getCardSuit[e.bribeCard.Suit]), _playerBribeDecks[e.currentPlayerTurn]);
				_mainTabControlVM.GameVM.GameStateVM.Bribe.Add(card);
				Application.Current.Dispatcher.Invoke(() =>
				{
					var gameShape = GameShape.GetGameShape(card.Deck.GameStateVM);
					var cardShape = gameShape.GetCardShape(card);
					cardShape.Card.Deck = _playerBribeDecks[e.currentPlayerTurn];
					_playerBribeDecks[e.currentPlayerTurn].Cards.Add(cardShape.Card);
					gameShape.GetDeckShape(cardShape.Card.Deck).UpdateCardShapes();
				});
			}

		}

		private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
		{
			GameShape.CardMouseLeftButtonDown +=
				new MouseButtonEventHandler(GameShape_CardMouseLeftButtonDown);
			GameShape.CardDrag += new CardDragEventHandler(GameShape_CardDrag);
		}

		private void Dealer_DeckMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			CollectCards();
		}

		private void GameShape_CardDrag(CardShape cardShape, DeckShape oldDeckShape, DeckShape newDeckShape)
		{
			if (((newDeckShape.Deck.TopCard == null) && (cardShape.Card.Number == 1)) ||
				((newDeckShape.Deck.TopCard != null) &&
				(cardShape.Card.Suit == newDeckShape.Deck.TopCard.Suit) &&
				(cardShape.Card.Number - 1 == newDeckShape.Deck.TopCard.Number)))
			{
				cardShape.Card.Deck = newDeckShape.Deck;

				if (oldDeckShape.Deck.TopCard != null)
				{
					oldDeckShape.Deck.TopCard.Visible = true;
					oldDeckShape.Deck.TopCard.Enabled = true;
					oldDeckShape.Deck.TopCard.IsDragable = true;
				}
			}
		}

		private void GameShape_CardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_webSocketClient.PlayerID != _webSocketClient.Game.GameState.PlayerTurn)
            {
				return;
            }

			var card = (CardShape)sender;

			if ((_webSocketClient.Game.GameState.GameNum == 4 || 
				_webSocketClient.Game.GameState.GameNum == 10) && 
				card.Card.Suit == CardSuit.Hearts && 
				Player4Hand.Deck.Cards.Any(cardDeck => cardDeck.Suit != CardSuit.Hearts))
			{
				return;
			}

			var gameShape = GameShape.GetGameShape(card.Card.Deck.GameStateVM);
			var oldDeckShape = gameShape.GetDeckShape(card.Card.Deck);

			if (oldDeckShape.Name == "Player4Hand")
			{
				card.Card.Deck = Player4Trick.Deck;
			}

			gameShape.GetDeckShape(card.Card.Deck).UpdateCardShapes();
			Canvas.SetZIndex(oldDeckShape, 0);

			var getCardSuit = new Dictionary<CardSuit, string>
			{
				{ CardSuit.Hearts, "hearts" },
				{ CardSuit.Clubs, "clubs" },
				{ CardSuit.Diamonds, "diamonds" },
				{ CardSuit.Spades, "spades" }
			};

			_webSocketClient.SendData(_mainTabControlVM.GameVM.GameSessionID,
				_mainTabControlVM.GameVM.GameStateVM.GameNum, _mainTabControlVM.GameVM.GameStateVM.CircleNum,
				_webSocketClient.PlayerID, getCardSuit[card.Card.Suit], card.Card.Number);
		}

        #endregion
    }
}
