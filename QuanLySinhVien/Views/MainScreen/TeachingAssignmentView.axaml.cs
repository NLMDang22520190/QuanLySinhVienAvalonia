using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class TeachingAssignmentView : UserControl
    {
        public TeachingAssignmentView()
        {
            InitializeComponent();
            SetUp();
        }
        private void SetUp()
        {

            var SearchTextBox = this.FindControl<TextBox>("SearchTextBox");
            if (SearchTextBox != null)
            {
                SearchTextBox.TextChanged += (sender, args) =>
                {
                    var textBox = (TextBox)sender;
                    var text = textBox.Text;
                    ((TeachingAssignmentViewModel)this.DataContext).SearchSubject(text);
                };
            }

            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedPhanCongIndex = dataGrid.SelectedIndex;
                    ((TeachingAssignmentViewModel)this.DataContext).SelectedPhanCongIndex = selectedPhanCongIndex;
                };
            }
        }
    }
}
