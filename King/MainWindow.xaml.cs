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

        private MainWindowVM _mainWindowVM = new MainWindowVM();

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainWindowVM;
        }

        #endregion
    }
}
