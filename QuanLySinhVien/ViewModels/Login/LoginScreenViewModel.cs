using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;
using QuanLySinhVien.Views.MainScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLySinhVien.Views.Login;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.Login
{
    public class LoginScreenViewModel : ViewModelBase
    {
        private Window _loginWindow;

        private bool _revealPassword = false;

        public bool RevealPassword
        {
            get => _revealPassword;
            set => this.RaiseAndSetIfChanged(ref _revealPassword, value);
        }

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
            MainScreen = new MainScreenView(_loginWindow);
            _loginWindow.Hide();
            MainScreen.Show();
        }

        public void ChangePasswordTextBoxState()
        {
            RevealPassword = !RevealPassword;
        }

        public void OpenForgotPassword(Window window)
        {
            var forgotPassword = new ForgotPasswordView();
            window.Hide();
            forgotPassword.Closed += (sender, args) =>
            {
                window.Show();
            };
            forgotPassword.Show();
        }
    }
}

