using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using QuanLySinhVien.Models;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class MainScreenView : AppWindow
    {
        private Window _loginWindow;

        public MainScreenView()
        {
            InitializeComponent();
        }

        public MainScreenView(Window loginWindow)
        {
            _loginWindow = loginWindow;
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.Closed += (sender, e) =>
            {
                _loginWindow.Show();
            };
            var nv = this.FindControl<NavigationView>("NavigationView");
            var vm = new MainScreenViewModel();
            this.DataContext = vm;
            nv.SelectionChanged += vm.OnNavigateViewSelectionChanged;
            
        }
    }
}
