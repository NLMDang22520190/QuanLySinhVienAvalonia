using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ReportSemesterView : UserControl
    {
        public ReportSemesterView()
        {
            InitializeComponent();
            SetUp();
        }

        private void SetUp() 
        {
            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedBaoCaoHocKyIndex = dataGrid.SelectedIndex;
                    ((ReportSemesterViewModel)this.DataContext).SelectedBaoCaoHocKyIndex = selectedBaoCaoHocKyIndex;
                };
            }

        }
    }
}
