using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ConfigueStudentScoreView : UserControl
    {
        public ConfigueStudentScoreView()
        {
            InitializeComponent();
        }

        private void SetUp()
        {
            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedDiemIndex = dataGrid.SelectedIndex;
                    ((ConfigueStudentScoreViewModel)this.DataContext).SelectedDiemIndex = selectedDiemIndex;
                };
            }
        }
    }
}
