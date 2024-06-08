using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ClassView : UserControl
    {
        public ClassView()
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
                    ((ClassViewModel)this.DataContext).SearchClass(text);
                };
            }

            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                DataGrid.SelectionChanged += (sender, args) =>
                {
                    var dataGrid = (DataGrid)sender;
                    var selectedClassIndex = dataGrid.SelectedIndex;
                    ((ClassViewModel)this.DataContext).SelectedClassIndex = selectedClassIndex;
                };
            }
        }
    }
}
