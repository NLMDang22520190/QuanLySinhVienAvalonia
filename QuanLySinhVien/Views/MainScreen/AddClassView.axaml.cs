using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using FluentAvalonia.UI.Windowing;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class AddClassView : AppWindow
    {
        public AddClassView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
