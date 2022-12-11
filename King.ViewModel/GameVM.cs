using System;
using System.Collections.Generic;

namespace King.ViewModel
{
	/// <summary>
	/// Колода всех 32 карт по порядку с картинки
	/// </summary>
	public class GameVM 
	{
		#region Fields

		private List<CardVM> _cards = new List<CardVM>();

		#endregion

		#region Properties

		public List<CardVM> CardsOLD
		{
			get
			{
				return _cards;
			}
		}

		internal Random Random = new Random();

		public GameVM ()
		{

		}

        #endregion
    }
}
