using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace QuanLySinhVien.ViewModels.Login
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        public ForgotPasswordViewModel()
        {
            // Add code here
        }

        public void SavePassword(Window window)
        {
            window.Close();
        }
    }
}
