using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;

namespace QuanLySinhVien.Views.Login
{
    public partial class LoginScreenView : AppWindow
    {
        public LoginScreenView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
        }
    }
}
