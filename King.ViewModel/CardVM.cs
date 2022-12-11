using King.ViewModel.Enumerations;
using System;

namespace King.ViewModel
{
	public class CardVM
	{

		#region Fields

		private CardRank _rank;

		private CardSuit _suit;

		private DeckVM _deck;

		private bool _visible = true;

		private bool _enabled = true;

		private bool _isDragable = true;

		public static bool IsAceBiggest = true;

		#endregion

		#region Properties

		public CardRank Rank
		{
			get
			{
				return _rank;
			}
		}

		public CardSuit Suit
		{
			get
			{
				return _suit;
			}
			set
			{
				_suit = value;
			}
		}

		public DeckVM Deck
		{
			get
			{
				return _deck;
			}
			set
			{
				if (_deck != value)
				{
					_deck.CARDS.Remove(this);
					_deck = value;
					_deck.CARDS.Add(this);
                    DeckChanged?.Invoke(this, null);
                }
			}
		}

		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (_visible != value)
				{
					_visible = value;
                    VisibleChanged?.Invoke(this, null);
                }
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

		public bool IsDragable
        {
			get 
			{ 
				return _isDragable; 
			}
			set 
			{
				_isDragable = value; 
			}
		}

		public CardColor Color
		{
			get
			{
				if ((Suit == CardSuit.Spades) || (Suit == CardSuit.Clubs))
				{
					return CardColor.Black;
				}
				else
				{
					return CardColor.Red;
				}
			}
		}

		public int Number
		{
			get
			{
				return (int) _rank;
			}
		}

		public string NumberString
		{
			get
			{
				switch (_rank)
				{
					case CardRank.Ace:
					{
						return "A";
					}
					case CardRank.Jack:
					{
						return "J";
					}
					case CardRank.Queen:
					{
						return "Q";
					}
					case CardRank.King:
					{
						return "K";
					}
					default:
					{
						return Number.ToString();
					}
				}
			}
		}

		public string SuitString
		{
			get
			{
				switch (_suit)
				{
					case CardSuit.Spades:
					{
						return "♠";
					}
					case CardSuit.Hearts:
					{
						return "♥";
					}
					case CardSuit.Clubs:
					{
						return "♣";
					}
					case CardSuit.Diamonds:
					{
						return "♦";
					}
					default:
					{
						return Suit.ToString();
					}
				}
			}
		}

		#endregion

		#region Events

		public event EventHandler VisibleChanged;

		public event EventHandler DeckChanged;

        #endregion

        #region Constructors

        /*public CardVM(int number, int suit, DeckVM deck)
        {
			_rank = (CardRank)number;
			_suit = (CardSuit)suit;
			_deck = deck;
			_deck.Game.CardsOLD.Add(this);
		}*/


        public CardVM(int number, int suit)
		{
			_rank = (CardRank)number;
			_suit = (CardSuit)suit;
		}

		#endregion

		#region Methods

		public void Shuffle()
		{
			Deck.CARDS.Remove(this);
			Deck.CARDS.Insert(Deck.Game.Random.Next(0, Deck.CARDS.Count), this);
		}

        #endregion
    }
}
