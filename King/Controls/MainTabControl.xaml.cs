using Client.WebSocketClient;
using King.ViewModel;
using System;
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

        #endregion

        #region Fields

        private WebSocketClient _webSocketClient;

        private DeckVM _dealer;

        #endregion

        #region Constructor

        public MainTabControl(WebSocketClient webSocketClient)
        {
            InitializeComponent();

            _webSocketClient = webSocketClient;
        }

        #endregion

        #region Methods

        private void NewGame()
        {
            GameShape.Game = new GameVM();
            SetupDealerDeck();
            SetupPlayerHandDecks();
            SetupTrickDecks();
        }

        private void DealNextHand()
        {
            if (_dealer.CARDS.Count < 32)
            {
                CollectCards();
            }

            //_dealer.Shuffle(5);

            for (var cardCount = 0; cardCount < CardsPerPlayer; cardCount++)
            {
                _dealer.Draw(Player1Hand.Deck, 1);
                _dealer.Draw(Player2Hand.Deck, 1);
                _dealer.Draw(Player3Hand.Deck, 1);
                _dealer.Draw(Player4Hand.Deck, 1);
            }

            //Player4Hand.Deck.Sort();
            Player4Hand.Deck.MakeAllCardsDragable(true);
            Player4Hand.Deck.FlipAllCards();

        }

        private void CollectCards()
        {
            Player1Hand.Deck.Draw(_dealer, Player1Hand.Deck.CARDS.Count);
            Player2Hand.Deck.Draw(_dealer, Player2Hand.Deck.CARDS.Count);
            Player3Hand.Deck.Draw(_dealer, Player3Hand.Deck.CARDS.Count);
            Player4Hand.Deck.FlipAllCards();
            Player4Hand.Deck.Draw(_dealer, Player4Hand.Deck.GameStateVM.Cards.Count);
        }

        private void SetupDealerDeck()
        {
            _dealer = new DeckVM(1, 13, GameShape.Game);

            //? _dealer.Shuffle(5);
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
            Player1Trick.Deck = new DeckVM(GameShape.Game)
            {
                Enabled = true
            };

            Player2Trick.Deck = new DeckVM(GameShape.Game)
            {
                Enabled = true
            };

            Player3Trick.Deck = new DeckVM(GameShape.Game)
            {
                Enabled = true
            };

            Player4Trick.Deck = new DeckVM(_webSocketClient.Game.State.Cards)
            {
                Enabled = true
            };

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
            Player1Hand.Deck = new DeckVM(GameShape.Game)
            {
                Enabled = true
            };
            Player2Hand.Deck = new DeckVM(GameShape.Game)
            {
                Enabled = true
            };
            Player3Hand.Deck = new DeckVM(GameShape.Game)
            {
                Enabled = true
            };
            Player4Hand.Deck = new DeckVM(GameShape.Game)
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

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //NewGame();

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
            var card = (CardShape)sender;
            var gameShape = GameShape.GetGameShape(card.Card.Deck.Game);
            var oldDeckShape = gameShape.GetDeckShape(card.Card.Deck);

            if (oldDeckShape.Name == "Player4Hand")
            {
                card.Card.Deck = Player4Trick.Deck;
            }

            gameShape.GetDeckShape(card.Card.Deck).UpdateCardShapes();
            Canvas.SetZIndex(oldDeckShape, 0);
        }

        #endregion

        private void MainWindow_DealButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame();

            var result = MessageBox.Show("Deal a new hand?", "Confirm New Deal", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            DealNextHand();
        }
    }
}
