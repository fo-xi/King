using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace King.ViewModel
{
    public class DeckVM
    {
        #region Fields

        private List<CardVM> _cards = new List<CardVM>();

        private bool _enabled = true;

        private GameVM _game;

        #endregion

        #region Properties

        public List<CardVM> Cards
        {
            get
            {
                return _cards;
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public GameVM Game
        {
            get
            {
                return _game;
            }
        }

        public CardVM TopCard
        {
            get
            {
                return Cards.Count > 0 ? Cards[Cards.Count - 1] : null;
            }
        }

        public CardVM BottomCard
        {
            get
            {
                return Cards.Count > 0 ? Cards[0] : null;
            }
        }

        public bool HasCards
        {
            get
            {
                return Cards.Count > 0;
            }
        }

        #endregion

        #region Events

        public event EventHandler SortChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public DeckVM(GameVM game)
        {
            this._game = game;
            this._game.Decks.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="numberOfDecks">The number of decks.</param>
        /// <param name="uptoNumber">The upto number.</param>
        /// <param name="game">The game.</param>
        public DeckVM(int numberOfDecks, int uptoNumber, GameVM game)
            : this(game)
        {
            for (int deck = 0; deck < numberOfDecks; deck++)
            {
                for (int suit = 1; suit <= 4; suit++)
                {
                    for (int number = 1; number <= uptoNumber; number++)
                    {
                        Cards.Add(new CardVM(number, (CardSuit)suit, this));
                    }
                }
            }

            Shuffle();
        }

        #endregion

        #region Get Methods

        public bool Has(int number, CardSuit suit)
        {
            return Has((CardRank)number, suit);
        }

        public bool Has(CardRank rank, CardSuit suit)
        {
            if (GetCard(rank, suit) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CardVM GetCard(int number, CardSuit suit)
        {
            return GetCard((CardRank)number, suit);
        }

        public CardVM GetCard(CardRank rank, CardSuit suit)
        {
            return Cards.FirstOrDefault(card => (card.Rank == rank) && (card.Suit == suit));
        }

        #endregion

        #region Sort Methods

        public void Shuffle()
        {
            Shuffle(1);
        }

        public void Shuffle(int times)
        {
            for (int time = 0; time < times; time++)
            {
                for (int i = 0; i < Cards.Count; i++)
                {
                    Cards[i].Shuffle();
                }
            }

            if (SortChanged != null)
            {
                SortChanged(this, null);
            }
        }

        public void Sort()
        {
            Cards.Sort(Game.CardSuitComparer);

            if (SortChanged != null)
            {
                SortChanged(this, null);
            }
        }

        #endregion

        #region Draw Cards Methods

        public void Draw(DeckVM toDeck, int count)
        {
            for (var i = 0; i < count; i++)
            {
                TopCard.Deck = toDeck;
            }
        }

        #endregion

        #region Methods

        public void FlipAllCards()
        {
            foreach (var t in Cards)
            {
                t.Visible = !t.Visible;
            }
        }

        public void EnableAllCards(bool enable)
        {
            foreach (var t in Cards)
            {
                t.Enabled = enable;
            }
        }

        public void MakeAllCardsDragable(bool isDragable)
        {
            foreach (var t in Cards)
            {
                t.IsDragable = isDragable;
            }
        }

        public override string ToString()
        {
            var output = new StringBuilder();

            output.Append("[" + Environment.NewLine);

            foreach (var t in Cards)
            {
                output.Append(t.ToString() + Environment.NewLine);
            }

            output.Append("]" + Environment.NewLine);
            return output.ToString();
        }

        #endregion
    }
}
