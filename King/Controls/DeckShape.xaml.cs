using King.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace King.Controls
{
	/// <summary>
	/// Логика взаимодействия для DeckShape.xaml
	/// </summary>
	public partial class DeckShape : UserControl
	{
		#region Fields

		private DeckVM _deck = null;

		private double _cardSpacerX = 0;

		private double _cardSpacerY = 0;

		private int _maxCardsSpace = 0;

		#endregion

		#region Properties

		private static List<DeckShape> PlayingDecks = new List<DeckShape>();

		public double CardSpacerX
		{
			get 
			{ 
				return _cardSpacerX; 
			}
			set 
			{ 
				_cardSpacerX = value; 
			}
		}

		public double CardSpacerY
		{
			get 
			{ 
				return _cardSpacerY; 
			}
			set 
			{ 
				_cardSpacerY = value; 
			}
		}

		public int MaxCardsSpace
		{
			get
			{
				return _maxCardsSpace;
			}
			set
			{
				_maxCardsSpace = value;
			}
		}

		public double NextCardX { get; set; }

		public double NextCardY { get; set; }

		public DeckVM Deck
		{
			get
			{
				return _deck;
			}
			set
			{
				if (_deck != null)
				{
					_deck.SortChanged -= DeckSortChanged;
				}

				_deck = value;

				_deck.SortChanged += DeckSortChanged;
				UpdateCardShapes();
			}
		}

		#endregion

		#region Events

		public event MouseEventHandler DeckMouseEnter;

		public event MouseEventHandler DeckMouseLeave;

		public event MouseEventHandler DeckMouseMove;

		public event MouseButtonEventHandler DeckMouseLeftButtonDown;

		public event MouseButtonEventHandler DeckMouseLeftButtonUp;

		#endregion

		#region Constructor

		public DeckShape()
		{
			InitializeComponent();

			PlayingDecks.Add(this);
			rectBorder.Visibility = Visibility.Collapsed;
		}

		#endregion

		#region Methods

		public void UpdateCardShapes()
		{
			GameShape game = GameShape.GetGameShape(Deck.Game);
			NextCardX = 0;
			NextCardY = 0;

			double localCardSpacerX = CardSpacerX;
			double localCardSpacerY = CardSpacerY;

			if ((MaxCardsSpace > 0) && (Deck.CARDS.Count > MaxCardsSpace))
			{
				localCardSpacerX = (CardSpacerX * MaxCardsSpace) / Deck.CARDS.Count;
				localCardSpacerY = (CardSpacerY * MaxCardsSpace) / Deck.CARDS.Count;
			}

			Duration duration = new Duration(TimeSpan.FromSeconds(0.2));

			Storyboard sb = new Storyboard();
			sb.Duration = duration;

			for (int i = 0; i < Deck.CARDS.Count; i++)
			{
				CardShape cardShape = game.GetCardShape(Deck.CARDS[i]);
				if (cardShape.Parent != LayoutRoot)
				{
					LayoutRoot.Children.Add(cardShape);
				}

				if (double.IsNaN(Canvas.GetLeft(cardShape)))
				{
					cardShape.SetValue(Canvas.LeftProperty, Convert.ToDouble(0));
				}
				if (double.IsNaN(Canvas.GetTop(cardShape)))
				{
					cardShape.SetValue(Canvas.TopProperty, Convert.ToDouble(0));
				}

				DoubleAnimation xAnim = new DoubleAnimation();
				xAnim.Duration = duration;
				sb.Children.Add(xAnim);
				Storyboard.SetTarget(xAnim, cardShape);
				Storyboard.SetTargetProperty(xAnim, new PropertyPath("(Canvas.Left)"));
				xAnim.To = NextCardX;

				DoubleAnimation yAnim = new DoubleAnimation();
				yAnim.Duration = duration;
				sb.Children.Add(yAnim);
				Storyboard.SetTarget(yAnim, cardShape);
				Storyboard.SetTargetProperty(yAnim, new PropertyPath("(Canvas.Top)"));
				yAnim.To = NextCardY;

				Canvas.SetZIndex(cardShape, i);

				NextCardX += localCardSpacerX;
				NextCardY += localCardSpacerY;
			}

			if (LayoutRoot.Resources.Contains("sb"))
			{
				LayoutRoot.Resources.Remove("sb");
			}

			LayoutRoot.Resources.Add("sb", sb);
			sb.Begin();
		}

		#endregion

		#region DeckShape Event Handlers

		private void DeckSortChanged(object sender, EventArgs e)
		{
			UpdateCardShapes();
		}


		private void RectBorderBackMouseEnter(object sender, MouseEventArgs e)
		{
			if (Deck != null && Deck.Enabled)
			{
				rectBorder.Visibility = Visibility.Visible;
			}

			if (DeckMouseEnter != null)
			{
				DeckMouseEnter(this, e);
			}
		}

		private void RectBorderBackMouseLeave(object sender, MouseEventArgs e)
		{
			if (Deck != null && Deck.Enabled)
			{
				rectBorder.Visibility = Visibility.Collapsed;
			}

			if (DeckMouseLeave != null)
			{
				DeckMouseLeave(this, e);
			}
		}

		private void RectBorderBackMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (DeckMouseLeftButtonDown != null)
			{
				DeckMouseLeftButtonDown(this, e);
			}
		}

		private void RectBorderBackMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (DeckMouseLeftButtonUp != null)
			{
				DeckMouseLeftButtonUp(this, e);
			}
		}

		private void RectBorderBackMouseMove(object sender, MouseEventArgs e)
		{
			if (DeckMouseMove != null)
			{
				DeckMouseMove(this, e);
			}
		}

		#endregion
	}
}
