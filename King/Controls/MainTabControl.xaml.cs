﻿using Client.WebSocketClient;
using Core;
using King.ViewModel;
using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

			DataChangedControl += OnDataChangedControl;

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

			_dealer.MakeAllCardsDragable(false);
			_dealer.Enabled = true;
			_dealer.FlipAllCards();

			Dealer.Deck = _dealer;
			GameShape.DeckShapes.Add(Dealer);
			Dealer.DeckMouseLeftButtonDown +=
				new MouseButtonEventHandler(Dealer_DeckMouseLeftButtonDown);
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

			GameShape.DeckShapes.Add(Player1Trick);
			GameShape.DeckShapes.Add(Player2Trick);
			GameShape.DeckShapes.Add(Player3Trick);
			GameShape.DeckShapes.Add(Player4Trick);
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

			GameShape.DeckShapes.Add(Player1Hand);
			GameShape.DeckShapes.Add(Player2Hand);
			GameShape.DeckShapes.Add(Player3Hand);
			GameShape.DeckShapes.Add(Player4Hand);
		}

		#endregion

		#region Event Handlers

		private void OnDataChanged(object sender, EventArgs e)
		{
			if (_isNewGame)
            {
				Application.Current.Dispatcher.Invoke(() =>
				{
					NewGame();
					DealNextHand();
				});
				_isNewGame = false;
			}

            //var card = (CardShape)sender;
            //var gameShape = GameShape.GetGameShape(card.Card.Deck.GameStateVM);
            //var oldDeckShape = gameShape.GetDeckShape(card.Card.Deck);

            //if (oldDeckShape.Name == "Player1Hand")
            //{
            //	card.Card.Deck = Player1Trick.Deck;
            //}

            //if (oldDeckShape.Name == "Player2Hand")
            //{
            //	card.Card.Deck = Player2Trick.Deck;
            //}

            //if (oldDeckShape.Name == "Player3Hand")
            //{
            //	card.Card.Deck = Player3Trick.Deck;
            //}

            //gameShape.GetDeckShape(card.Card.Deck).UpdateCardShapes();
            //Canvas.SetZIndex(oldDeckShape, 0);

            //DataChangedControl.Invoke(this, EventArgs.Empty);
        }

		private void OnDataChangedControl(object sender, EventArgs e)
        {

			//var card = (CardShape)sender;
			//var gameShape = GameShape.GetGameShape(card.Card.Deck.GameStateVM);
			//var oldDeckShape = gameShape.GetDeckShape(card.Card.Deck);

			////Перемещение карт других игроков

			//if (oldDeckShape.Name == "Player1Hand" && 
			//	_webSocketClient.Game.GameState.StartedBy == _mainTabControlVM.FirstPlayer.ID)
			//{
			//	card.Card.Deck = Player1Trick.Deck;
			//}

			//if (oldDeckShape.Name == "Player2Hand" &&
			//	_webSocketClient.Game.GameState.StartedBy == _mainTabControlVM.SecondPlayer.ID)
			//{
			//	card.Card.Deck = Player2Trick.Deck;
			//}

			//if (oldDeckShape.Name == "Player3Hand" &&
			//	_webSocketClient.Game.GameState.StartedBy == _mainTabControlVM.ThirdPlayer.ID)
			//{
			//	card.Card.Deck = Player3Trick.Deck;
			//}

			//gameShape.GetDeckShape(card.Card.Deck).UpdateCardShapes();
			//Canvas.SetZIndex(oldDeckShape, 0);
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

			if (_webSocketClient.Game.GameState.Bribe != null)
			{
				_mainTabControlVM.GameVM.GameStateVM.Bribe.Add(new CardVM(e.bribeCard.Magnitude,
					Convert.ToInt32(getCardSuit[e.bribeCard.Suit]), _playerBribeDecks[e.currentPlayerTurn]));
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
