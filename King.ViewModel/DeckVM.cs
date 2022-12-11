using Core;
using GalaSoft.MvvmLight;
using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace King.ViewModel
{
    /// <summary>
    /// Колода карт (на руке у каждого игрока своя)
    /// </summary>
    public class DeckVM : ViewModelBase
	{
		#region Fields

		private List<CardVM> _CARDS = new List<CardVM>();

		private GameStateVM _gameStateVM;

        private bool _enabled = true;

		private GameVM _game;

		#endregion

		#region Properties

		public List<CardVM> CARDS
		{
			get
			{
				return _CARDS;
			}
		}

		public GameStateVM GameStateVM
		{
			get
			{
				return _gameStateVM;
			}
            set
            {
				_gameStateVM = value;

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
				return CARDS.Count > 0 ? CARDS[CARDS.Count - 1] : null;
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
		}

        public DeckVM(List<Card> cards)
        {
			GameStateVM = new GameStateVM(cards);
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
						if (number >= 2 && number <= 6)
						{
							continue;
						}

						CARDS.Add(new CardVM(number, suit));
					}
				}
			}

			//Shuffle();
		}

		#endregion

		#region Sort Methods

		/*public void Shuffle()
		{
			Shuffle(1);
		}*/

		/*public void Shuffle(int times)
		{
			for (int time = 0; time < times; time++)
			{
				for (int i = 0; i < CARDS.Count; i++)
				{
					CARDS[i].Shuffle();
				}
			}

			SortChanged?.Invoke(this, null);
		}*/

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
			foreach (var t in CARDS)
			{
				t.Visible = !t.Visible;
			}
		}

		public void MakeAllCardsDragable(bool isDragable)
		{
			foreach (var t in CARDS)
			{
				t.IsDragable = isDragable;
			}
		}

		#endregion
	}
}
