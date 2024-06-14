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

        public static bool IsLogout = false;

        public MainScreenView()
        {
            InitializeComponent();
        }

        public MainScreenView(Window loginWindow)
        {
            IsLogout = false;
            _loginWindow = loginWindow;
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.Closed += (sender, e) =>
            {
                if (!IsLogout)
                    _loginWindow.Close();
                else
                    _loginWindow.Show();
            };
            var nv = this.FindControl<NavigationView>("NavigationView");
            var vm = new MainScreenViewModel();
            this.DataContext = vm;
            nv.SelectionChanged += vm.OnNavigateViewSelectionChanged;
            nv.SelectedItem = nv.MenuItems[0];

        }
    }
}
