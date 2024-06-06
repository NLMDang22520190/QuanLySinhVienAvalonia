using System.Diagnostics;
using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class TeacherInfoView : UserControl
    {
        public TeacherInfoView()
        {
            InitializeComponent();

            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedTeacherIndex = dataGrid.SelectedIndex;
                    ((TeacherInfoViewModel)this.DataContext).SelectedGiaoVienIndex = selectedTeacherIndex;
                };
            }

        }
    }
}

