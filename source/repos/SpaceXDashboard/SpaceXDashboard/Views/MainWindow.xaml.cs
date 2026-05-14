using System.Windows;
using SpaceXDashboard.ViewModels;

namespace SpaceXDashboard.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm;
            Loaded += async (s, e) => await _vm.LoadDataAsync();
        }
    }
}