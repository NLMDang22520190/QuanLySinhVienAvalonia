using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using Microsoft.VisualBasic;
using QuanLySinhVien.ViewModels.Login;

namespace QuanLySinhVien.Views.Login
{
    public partial class LoginScreenView : AppWindow
    {
        public LoginScreenView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;

            var RevealPasswordBtn = this.FindControl<Button>("RevealPasswordBtn");
            var passwordIcon = this.FindControl<PathIcon>("PasswordIcon");
            var showPasswordIcon = this.FindResource("eye_show_regular") as StreamGeometry;
            var hidePasswordIcon = this.FindResource("eye_hide_regular") as StreamGeometry;

            if (RevealPasswordBtn != null)
            {
                RevealPasswordBtn.Click += (sender, args) =>
                {
                    if (!((LoginScreenViewModel)this.DataContext).RevealPassword)
                    {
                            passwordIcon.Data = showPasswordIcon;
                    }
                    else
                    {
                            passwordIcon.Data = hidePasswordIcon;
                    }
                };
            }
        }

       
    }
}
