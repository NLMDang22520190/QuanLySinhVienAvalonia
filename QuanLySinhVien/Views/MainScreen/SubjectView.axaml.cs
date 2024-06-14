using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class SubjectView : UserControl
    {
        public SubjectView()
        {
            InitializeComponent();

            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedSubjectIndex = dataGrid.SelectedIndex;
                    ((SubjectViewModel)this.DataContext).SelectedMonHocIndex = selectedSubjectIndex;
                };
            }
        }
    }
}
