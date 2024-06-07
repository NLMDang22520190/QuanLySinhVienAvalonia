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
                    ((TeacherInfoViewModel)this.DataContext).SearchTeacher(text);
                };
            }

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

