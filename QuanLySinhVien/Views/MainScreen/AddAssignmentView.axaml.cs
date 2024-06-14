using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using FluentAvalonia.UI.Windowing;


namespace QuanLySinhVien.Views.MainScreen
{
    public partial class AddAssignmentView : AppWindow
    {
        public AddAssignmentView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
