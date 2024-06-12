using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Media;
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

            var RevealPasswordBtn = this.FindControl<Button>("RevealPasswordBtn");
            var RevealConfirmPasswordBtn = this.FindControl<Button>("RevealConfirmPasswordBtn");
            var passwordIcon = this.FindControl<PathIcon>("PasswordIcon");
            var confirmPasswordIcon = this.FindControl<PathIcon>("PasswordConfirmIcon");
            var showPasswordIcon = this.FindResource("eye_show_regular") as StreamGeometry;
            var hidePasswordIcon = this.FindResource("eye_hide_regular") as StreamGeometry;

            if (RevealPasswordBtn != null)
            {
                RevealPasswordBtn.Click += (sender, args) =>
                {
                    if (!((ForgotPasswordViewModel)this.DataContext).RevealPassword)
                    {
                        passwordIcon.Data = showPasswordIcon;
                    }
                    else
                    {
                        passwordIcon.Data = hidePasswordIcon;
                    }
                };
            }
            if (RevealConfirmPasswordBtn != null)
            {
                RevealConfirmPasswordBtn.Click += (sender, args) =>
                {
                    if (!((ForgotPasswordViewModel)this.DataContext).RevealConfirmPassword)
                    {
                        confirmPasswordIcon.Data = showPasswordIcon;
                    }
                    else
                    {
                        confirmPasswordIcon.Data = hidePasswordIcon;
                    }
                };
            }
        }
    }
}
