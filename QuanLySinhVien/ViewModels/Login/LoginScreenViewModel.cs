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
        public LoginScreenViewModel()
        {
            // Add code here
        }

        public void OpenMainScreen()
        {
            var mainScreen = new MainScreenView();
            mainScreen.Show();
        }
    }
}
