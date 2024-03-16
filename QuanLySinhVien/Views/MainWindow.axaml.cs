using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using FluentAvalonia.UI.Windowing;

namespace QuanLySinhVien.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        TitleBar.Height = 0;
        TitleBar.ExtendsContentIntoTitleBar = true;
    }
}
