using Core;
using GalaSoft.MvvmLight;
using King.ViewModel.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private List<CardVM> _cards = new List<CardVM>();

		private GameStateVM _gameStateVM;

		private int _idPlayer;

		private bool _enabled = true;

		#endregion

		#region Properties

		public Dictionary<string, CardSuit> GetCardSuit { get; }

		public List<CardVM> Cards
		{
			get
			{
				return _cards;
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

		public int IDPlayer
		{
			get
			{
				return _idPlayer;
			}
			set
			{
				_idPlayer = value;
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

		public DeckVM()
		{

		}

		public DeckVM(GameStateVM gameStateVM, int idPlayer)
        {
			GameStateVM = gameStateVM;
			GameStateVM.Decks.Add(this);
			IDPlayer = idPlayer;
		}

		public DeckVM(int numberOfDecks, ObservableCollection<Card> cards, GameStateVM gameStateVM, int idPlayer) 
			: this(gameStateVM, idPlayer)
		{
			GetCardSuit = new Dictionary<string, CardSuit>
			{
				{ "hearts", CardSuit.Hearts },
				{ "clubs", CardSuit.Clubs },
				{ "diamonds", CardSuit.Diamonds },
				{ "spades", CardSuit.Spades }
			};

			for (int i = 0; i < numberOfDecks; i++)
			{
				foreach (var card in cards)
				{
					Cards.Add(new CardVM(card.Magnitude, Convert.ToInt32(GetCardSuit[card.Suit]), this));
				}
			}
		}

		#endregion

		#region Methods

		public void UpdateCards(ObservableCollection<Card> cards)
        {
			Cards.Clear();
			foreach (var card in cards)
			{
				Cards.Add(new CardVM(card.Magnitude, Convert.ToInt32(GetCardSuit[card.Suit]), this));
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
