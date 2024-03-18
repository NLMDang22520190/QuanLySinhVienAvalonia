using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;
using QuanLySinhVien.Views.MainScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.Login
{
    public class LoginScreenViewModel: ViewModelBase
    {
        private Window _loginWindow;

        public MainScreenView MainScreen { get; set; }

        public LoginScreenViewModel()
        {
            // Add code here
        }

        public LoginScreenViewModel(Window loginWindow)
        {
            
            _loginWindow = loginWindow;
        }

        public void OpenMainScreen()
        {
            MainScreen = new MainScreenView(_loginWindow)
            {
                DataContext = new MainScreenViewModel()
            };
            _loginWindow.Hide();
            MainScreen.Show();
        }
    }
}
