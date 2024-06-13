using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using FluentAvalonia.UI.Windowing;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ChangePasswordView : AppWindow
    {
        public ChangePasswordView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
