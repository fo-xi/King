using King.ViewModel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace King.Controls
{
	/// <summary>
	/// Логика взаимодействия для GameShape.xaml
	/// </summary>
	public partial class GameShape : UserControl
	{
		#region Fields

		private List<CardShape> _cardShapes = new List<CardShape>();

		private List<DeckShape> _deckShapes = new List<DeckShape>();

		private GameVM _game = new GameVM();

		#endregion

		#region Properties

		private static List<GameShape> GameShapes = new List<GameShape>();
		public List<CardShape> CardShapes
		{
			get
			{
				return _cardShapes;
			}
		}

		public List<DeckShape> DeckShapes
		{
			get
			{
				return _deckShapes;
			}
		}

		public GameVM Game { get; set; }

		#endregion

		#region Events

		public event MouseEventHandler CardMouseEnter;

		public event MouseEventHandler CardMouseLeave;

		public event MouseEventHandler CardMouseMove;

		public event MouseButtonEventHandler CardMouseLeftButtonDown;

		public event MouseButtonEventHandler CardMouseLeftButtonUp;

		public event CardDragEventHandler CardDrag;

		#endregion

		#region Constructor

		public GameShape()
		{
			InitializeComponent();

			GameShapes.Add(this);
		}

		#endregion

		#region Cards Event Handlers

		protected void cardShape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (CardMouseLeftButtonDown != null)
			{
				CardMouseLeftButtonDown(sender, e);
			}
		}

		protected void cardShape_MouseMove(object sender, MouseEventArgs e)
		{
			if (CardMouseMove != null)
			{
				CardMouseMove(sender, e);
			}
		}

		protected void cardShape_MouseLeave(object sender, MouseEventArgs e)
		{
			if (CardMouseLeave != null)
			{
				CardMouseLeave(sender, e);
			}
		}

		protected void cardShape_MouseEnter(object sender, MouseEventArgs e)
		{
			if (CardMouseEnter != null)
			{
				CardMouseEnter(sender, e);
			}
		}

		protected void cardShape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (CardMouseLeftButtonUp != null)
			{
				CardMouseLeftButtonUp(sender, e);
			}
		}

		protected void cardShape_CardDrag(CardShape cardShape, DeckShape oldDeckShape, DeckShape newDeckShape)
		{
			if (CardDrag != null)
			{
				CardDrag(cardShape, oldDeckShape, newDeckShape);
			}
		}

		#endregion

		#region Static Methods

		public static GameShape GetGameShape(GameVM game)
		{
			for (int i = 0; i < GameShapes.Count; i++)
			{
				if (GameShapes[i].Game == game)
				{
					return GameShapes[i];
				}
			}

			return null;
		}

		#endregion

		#region Methods

		public CardShape GetCardShape(CardVM card)
		{
			for (int i = 0; i < CardShapes.Count; i++)
			{
				if (CardShapes[i].Card == card)
				{
					return CardShapes[i];
				}
			}

			CardShape cardShape = new CardShape();
			cardShape.Card = card;
			CardShapes.Add(cardShape);

			cardShape.CardMouseLeftButtonDown += new MouseButtonEventHandler(cardShape_MouseLeftButtonDown);
			cardShape.CardMouseLeftButtonUp += new MouseButtonEventHandler(cardShape_MouseLeftButtonUp);
			cardShape.CardMouseEnter += new MouseEventHandler(cardShape_MouseEnter);
			cardShape.CardMouseLeave += new MouseEventHandler(cardShape_MouseLeave);
			cardShape.CardMouseMove += new MouseEventHandler(cardShape_MouseMove);
			cardShape.CardDrag += new CardDragEventHandler(cardShape_CardDrag);

			return cardShape;
		}

		public DeckShape GetDeckShape(DeckVM deck)
		{
			for (int i = 0; i < DeckShapes.Count; i++)
			{
				if (DeckShapes[i].Deck == deck)
				{
					return DeckShapes[i];
				}
			}

			return null;
		}

		#endregion
	}
}
