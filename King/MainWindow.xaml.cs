using Client.WebSocketClient;
using King.Controls;
using King.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace King
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		private WebSocketClient _webSocketClient;

		private MainWindowVM _mainWindowVM;

		#endregion

		#region Constructor

		public MainWindow()
		{
			InitializeComponent();

			_webSocketClient = new WebSocketClient();
			_mainWindowVM = new MainWindowVM(_webSocketClient);
			DataContext = _mainWindowVM;

			grid.Children.Add(new MainTabControl(_webSocketClient, _mainWindowVM.MainTabControlVM));
		}

		#endregion
	}
}
