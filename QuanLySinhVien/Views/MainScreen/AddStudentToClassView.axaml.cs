using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class AddStudentToClassView : Window
    {
        public AddStudentToClassView()
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
                    ((AddStudentToClassViewModel)this.DataContext).SearchStudent(text);
                };
            }

            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedHocSinhIndex = dataGrid.SelectedIndex;
                    ((AddStudentToClassViewModel)this.DataContext).SelectedHocSinhIndex = selectedHocSinhIndex;
                };
            }
        }
    }
}
