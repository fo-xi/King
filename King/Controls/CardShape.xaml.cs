using King.ViewModel;
using King.ViewModel.Enumerations;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace King.Controls
{
	public delegate void CardDragEventHandler(CardShape cardShape,
			DeckShape oldDeckShape, DeckShape newDeckShape);

	/// <summary>
	/// Логика взаимодействия для CardShape.xaml
	/// </summary>
	public partial class CardShape : UserControl
	{
		#region Constants

		public const double CardOrigX = 57;

		public const double CardOrigY = 45.75;

		public const double CardWidth = 54 * 2;

		public const double CardHeight = 72.75 * 2;

		public const double CardWidthRect = 54.75 * 2;

		public const double CardHeightRect = 73.5 * 2;

		#endregion

		#region Fields

		private Storyboard _aniFlipStart;

		private Storyboard _aniFlipEnd;

		private Storyboard _animRotate;

		private Point _oldMousePos;

		private bool _isDrag = false;

		private CardVM _cardVM = null;

		#endregion

		#region Properties

		public CardVM Card
		{
			get
			{
				return _cardVM;
			}
			set
			{
				if (_cardVM != null)
				{
					_cardVM.VisibleChanged -= new EventHandler(CardVisibleChanged);
					_cardVM.DeckChanged -= new EventHandler(CardDeckChanged);
				}

				_cardVM = value;

				_cardVM.VisibleChanged += new EventHandler(CardVisibleChanged);
				_cardVM.DeckChanged += new EventHandler(CardDeckChanged);

				double x = 0;
				double y = 0;

				if (Card.Visible)
				{
					var number = Card.Number;
					if (number == 14)
                    {
						number = 1;
                    }

					//Отображение карты
					if (number <= 10)
					{
						x = (number - 1) % 2;
						y = (number - 1) / 2;

						switch (Card.Suit)
						{
							case CardSuit.Spades:
								x += 6;
								break;
							case CardSuit.Hearts:
								x += 0;
								break;
							case CardSuit.Diamonds:
								x += 2;
								break;
							case CardSuit.Clubs:
								x += 4;
								break;
						}
					}
					else
					{
						int highNumber = number - 11;
						switch (Card.Suit)
						{
							case CardSuit.Spades:
								highNumber += 6;
								break;
							case CardSuit.Hearts:
								highNumber += 9;
								break;
							case CardSuit.Diamonds:
								highNumber += 3;
								break;
							case CardSuit.Clubs:
								highNumber += 0;
								break;
						}

						x = (highNumber % 2) + 8;
						y = highNumber / 2;
					}
				}
				else
				{
					//Отображение рубашки
					x = 8;
					y = 6;
				}

				//Отображение карт на руках (как у себя, так и других)
				((RectangleGeometry)imgCard.Clip).Rect = new Rect(x * CardWidthRect + CardOrigX, y * CardHeightRect + CardOrigY, CardWidth, CardHeight);
				foreach (Transform tran in ((TransformGroup)imgCard.RenderTransform).Children)
				{
					if (tran.GetType() == typeof(TranslateTransform))
					{
						tran.SetValue(TranslateTransform.XProperty, -x * CardWidthRect - CardOrigX);
						tran.SetValue(TranslateTransform.YProperty, -y * CardHeightRect - CardOrigY);
					}
				}
				imgCard.RenderTransformOrigin = new Point(0.05 + (x * 0.1), 0.08 + (y * 0.166666));
			}
		}

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

		public CardShape()
		{
			InitializeComponent();

			_aniFlipStart = (Storyboard)Resources["aniFlipStart"];
			_aniFlipEnd = (Storyboard)Resources["aniFlipEnd"];
			_animRotate = (Storyboard)Resources["animRotate"];

			rectBorder.Visibility = Visibility.Collapsed;
			_aniFlipStart.Completed += new EventHandler(AniFlipStartCompleted);
		}

		#endregion

		#region Card Event Handlers

		private void CardVisibleChanged(object sender, EventArgs e)
		{
			var gameShape = GameShape.GetGameShape(this.Card.Deck.GameStateVM);
			var cardShape = gameShape.GetCardShape((CardVM)sender);

			if (double.IsNaN(Canvas.GetLeft(cardShape)))
			{
				cardShape.SetValue(Canvas.LeftProperty, Convert.ToDouble(0));
			}

			if (double.IsNaN(Canvas.GetTop(cardShape)))
			{
				cardShape.SetValue(Canvas.TopProperty, Convert.ToDouble(0));
			}

			_aniFlipStart.Begin();
		}

		private void CardDeckChanged(object sender, EventArgs e)
		{
			GameShape gameShape = GameShape.GetGameShape(this.Card.Deck.GameStateVM);
			DeckShape oldDeck = (DeckShape)((Canvas)this.Parent).Parent;
			DeckShape newDeck = gameShape.GetDeckShape(this.Card.Deck);

			double startX = Canvas.GetLeft(oldDeck) + Canvas.GetLeft(this);
			double startY = Canvas.GetTop(oldDeck) + Canvas.GetTop(this);

			double endX = Canvas.GetLeft(newDeck);
			double endY = Canvas.GetTop(newDeck);

			((Canvas)this.Parent).Children.Remove(this);
			newDeck.LayoutRoot.Children.Add(this);

			Canvas.SetLeft(this, (double.IsNaN(startX) ? Convert.ToDouble(0) : startX) - endX);
			Canvas.SetTop(this, (double.IsNaN(startY) ? Convert.ToDouble(0) : startY) - endY);

			oldDeck.UpdateCardShapes();
			newDeck.UpdateCardShapes();
		}

		#endregion

		#region CardShape Event Handlers

		protected void AniFlipStartCompleted(object sender, EventArgs e)
		{
			this.Card = this.Card;
			_aniFlipEnd.Begin();
		}

		private void ImgCardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (Card != null && Card.IsDragable)
			{
				imgCard.CaptureMouse();
				_isDrag = true;
				_oldMousePos = e.GetPosition(LayoutRoot);
			}

			if (CardMouseLeftButtonDown != null)
			{
				CardMouseLeftButtonDown(this, e);
			}
		}

		private void ImgCardMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_isDrag)
			{
				imgCard.ReleaseMouseCapture();
				_isDrag = false;

				GameShape gameShape = GameShape.GetGameShape(this.Card.Deck.GameStateVM);
				DeckShape oldDeckShape = gameShape.GetDeckShape(this.Card.Deck);
				DeckShape nearestDeckShape = null;
				double nearestDistance = double.MaxValue;

				foreach (var deck in gameShape.DeckShapes)
				{
					if (deck.Deck.Enabled)
					{
						//double dx = Canvas.GetLeft(deck) - (Canvas.GetLeft(this) + Canvas.GetLeft((UIElement)((Canvas)this.Parent).Parent));
						//double dy = Canvas.GetTop(deck) - (Canvas.GetTop(this) + Canvas.GetTop((UIElement)((Canvas)this.Parent).Parent));
						var offset = VisualTreeHelper.GetOffset(deck);

						var dx = offset.X - e.GetPosition((UIElement)((Canvas)this.Parent).Parent).X;
						var dy = offset.Y - e.GetPosition((UIElement)((Canvas)this.Parent).Parent).Y;

						double distance = Math.Sqrt(dx * dx + dy * dy);

						if (distance < nearestDistance)
						{
							nearestDistance = distance;
							nearestDeckShape = deck;
						}
					}
				}

				if ((nearestDeckShape != null) && (CardDrag != null))
				{
					CardDrag(this, gameShape.GetDeckShape(this.Card.Deck), nearestDeckShape);
				}

				gameShape.GetDeckShape(this.Card.Deck)?.UpdateCardShapes();
				if (oldDeckShape != null)
				{
					Canvas.SetZIndex(oldDeckShape, 0);
				}
			}

			if (CardMouseLeftButtonUp != null)
			{
				CardMouseLeftButtonUp(this, e);
			}
		}

		private void ImgCardMouseMove(object sender, MouseEventArgs e)
		{
			if (_isDrag)
			{
				Point newMousePos = e.GetPosition(LayoutRoot);

				double dx = newMousePos.X - _oldMousePos.X;
				double dy = newMousePos.Y - _oldMousePos.Y;

				GameShape gameShape = GameShape.GetGameShape(this.Card.Deck.GameStateVM);

				var card = Card.Deck.Cards.FirstOrDefault(c => c.Suit == Card.Suit && c.Number == Card.Number);
				if(card == null)
                {
					return;
                }

				for (int i = this.Card.Deck.Cards.IndexOf(card); i < this.Card.Deck.Cards.Count; i++)
				{
					CardShape cardShape = gameShape.GetCardShape(this.Card.Deck.Cards[i]);
					Canvas.SetLeft(cardShape, Canvas.GetLeft(cardShape) + dx);
					Canvas.SetTop(cardShape, Canvas.GetTop(cardShape) + dy);
					Canvas.SetZIndex(gameShape.GetDeckShape(this.Card.Deck), 100);
				}
			}

			if (CardMouseMove != null)
			{
				CardMouseMove(this, e);
			}
		}

		private void ImgCardMouseEnter(object sender, MouseEventArgs e)
		{
			if (Card != null && Card.Enabled)
			{
				rectBorder.Visibility = Visibility.Visible;
			}

			if (CardMouseEnter != null)
			{
				CardMouseEnter(this, e);
			}
		}

		private void ImgCardMouseLeave(object sender, MouseEventArgs e)
		{
			if (Card != null && Card.Enabled)
			{
				rectBorder.Visibility = Visibility.Collapsed;
			}

			if (CardMouseLeave != null)
			{
				CardMouseLeave(this, e);
			}
		}

		#endregion
	}
}
