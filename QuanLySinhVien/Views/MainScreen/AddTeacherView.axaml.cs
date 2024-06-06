using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class AddTeacherView : AppWindow
    {
        public AddTeacherView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
