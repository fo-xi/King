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
            get 
            { 
                return _enabled; 
            }
            set 
            { 
                _enabled = value; 
            }
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

        #endregion

        #region Events

        public event EventHandler SortChanged;

        #endregion

        #region Constructors

        public DeckVM(GameVM game)
        {
            _game = game;
            _game.Decks.Add(this);
        }

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

            SortChanged?.Invoke(this, null);
        }

        public void Sort()
        {
            SortChanged?.Invoke(this, null);
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

        public void MakeAllCardsDragable(bool isDragable)
        {
            foreach (var t in Cards)
            {
                t.IsDragable = isDragable;
            }
        }

        #endregion
    }
}
