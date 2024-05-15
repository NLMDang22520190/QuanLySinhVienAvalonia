using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class AddStudentView : AppWindow
    {

        public AddStudentView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
