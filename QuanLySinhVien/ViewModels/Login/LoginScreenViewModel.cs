using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;
using QuanLySinhVien.Views.MainScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Login;
using ReactiveUI;
using QuanLySinhVien.Services;

namespace QuanLySinhVien.ViewModels.Login
{
    public class LoginScreenViewModel : ViewModelBase
    {
        #region Properties

        private string _username;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        private Window _loginWindow;

        private bool _revealPassword = false;

        public bool RevealPassword
        {
            get => _revealPassword;
            set => this.RaiseAndSetIfChanged(ref _revealPassword, value);
        }

        private bool _loginState = false;


        #endregion

        public MainScreenView MainScreen { get; set; }


        public LoginScreenViewModel()
        {
            // Add code here
        }

        public LoginScreenViewModel(Window loginWindow)
        {

            _loginWindow = loginWindow;
        }

        public void OpenMainScreen(NguoiDung nguoiDung)
        {

            MainScreen = new MainScreenView(_loginWindow);
            _loginWindow.Hide();
            MainScreen.Show();
        }

        public void Login()
        {
            // Sử dụng VerifyPassword để xác thực mật khẩu
            try
            {
                var user = DataProvider.Ins.DB.NguoiDungs.FirstOrDefault(x => x.TenDangNhap == Username);

                if (user != null && PasswordHasher.VerifyPassword(Password, user.MatKhau))
                {
                    OpenMainScreen(user);
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("Lỗi", "Tên đăng nhập hoặc mật khẩu không đúng !", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_loginWindow);
                }
            }
            catch (Exception e)
            {
                MessageBoxManager.GetMessageBoxStandard("Lỗi", "Hệ thống đang khởi động vui lòng thử lại sau vài giây", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_loginWindow);
            }
            
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

