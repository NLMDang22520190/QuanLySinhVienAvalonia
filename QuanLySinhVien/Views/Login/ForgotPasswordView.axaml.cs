using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using FluentAvalonia.UI.Windowing;
using QuanLySinhVien.ViewModels.Login;

namespace QuanLySinhVien.Views.Login
{
    public partial class ForgotPasswordView : AppWindow
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.DataContext = new ForgotPasswordViewModel();
        }
    }
}
