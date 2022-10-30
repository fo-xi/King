using King.ViewModel.Enumerations;
using System;

namespace King.ViewModel
{
	public class CardVM : IComparable<CardVM>
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

		public CardSuit Suit { get; set; }

		public DeckVM Deck
		{
			get
			{
				return _deck;
			}
			set
			{
				if (_deck.Game != value.Game)
				{
					throw new InvalidOperationException("The new deck must be in the same" +
						"game like the old deck of the card.");
				}

				if (_deck != value)
				{
					_deck.Cards.Remove(this);
					_deck = value;
					_deck.Cards.Add(this);

					if (DeckChanged != null)
					{
						DeckChanged(this, null);
					}
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

					if (VisibleChanged != null)
					{
						VisibleChanged(this, null);
					}
				}
			}
		}

		public bool Enabled { get; set; }

		public bool IsDragable { get; set; }

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

		public CardVM(CardRank rank, CardSuit suit, DeckVM deck)
		{
			this._rank = rank;
			this._suit = suit;
			this._deck = deck;
			this._deck.Game.Cards.Add(this);
		}

		public CardVM(int number, CardSuit suit, DeckVM deck)
		{
			this._rank = (CardRank)number;
			this._suit = suit;
			this._deck = deck;
			this._deck.Game.Cards.Add(this);
		}

		#endregion

		#region Methods

		public int CompareTo(CardVM other)
		{
			int value1 = this.Number;
			int value2 = other.Number;

			if (IsAceBiggest)
			{
				if (value1 == 1)
				{
					value1 = 14;
				}

				if (value2 == 1)
				{
					value2 = 14;
				}
			}

			if (value1 > value2)
			{
				return 1;
			}
			else if (value1 < value2)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		public void MoveToFirst()
		{
			MoveToIndex(0);
		}

		public void MoveToLast()
		{
			MoveToIndex(Deck.Cards.Count);
		}

		public void Shuffle()
		{
			MoveToIndex(Deck.Game.random.Next(0, Deck.Cards.Count));
		}

		public void MoveToIndex(int index)
		{
			Deck.Cards.Remove(this);
			Deck.Cards.Insert(index, this);
		}

		public override string ToString()
		{
			return NumberString + "" + SuitString;
		}

		#endregion
	}
}
