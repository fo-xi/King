using System;
using System.Collections.Generic;

namespace King.ViewModel
{
	public class GameVM
	{
		#region Fields

		private List<CardVM> _cards = new List<CardVM>();

		private List<DeckVM> _decks = new List<DeckVM>();

		#endregion

		#region Properties

		public List<CardVM> Cards
		{
			get
			{
				return _cards;
			}
		}

		public List<DeckVM> Decks
		{
			get
			{
				return _decks;
			}
		}

		internal Random random = new Random();

		internal HighCardSuitComparer CardSuitComparer = new HighCardSuitComparer();

		#endregion
	}
}
