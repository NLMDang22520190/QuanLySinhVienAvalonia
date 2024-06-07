using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using FluentAvalonia.UI.Windowing;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class EditStudentView : AppWindow
    {
        public EditStudentView()
        {
            InitializeComponent();
            TitleBar.Height = -1;
            TitleBar.ExtendsContentIntoTitleBar = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var gioiTinhComboBox = this.FindControl<ComboBox>("GioiTinh");
            if (gioiTinhComboBox != null)
            {
                gioiTinhComboBox.SelectionChanged += (sender, args) =>
                {
                    var comboBox = (ComboBox)sender;
                    var selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                    var content = selectedItem.Content.ToString();
                    if (content == "Nam")
                    {
                        ((EditStudentViewModel)this.DataContext).GioiTinh = true;
                    }
                    else
                    {
                        ((EditStudentViewModel)this.DataContext).GioiTinh = false;
                    }
                };
            }

            var ngaySinhCalenderDatePicker = this.FindControl<CalendarDatePicker>("NgaySinh");
            if (ngaySinhCalenderDatePicker != null)
            {
                ngaySinhCalenderDatePicker.SelectedDateChanged += (sender, args) =>
                {
                    var datePicker = (CalendarDatePicker)sender;
                    var selectedDate = datePicker.SelectedDate;
                    if (selectedDate.HasValue)
                    {
                        ((EditStudentViewModel)this.DataContext).NgaySinh = selectedDate.Value;
                    }
                };
            }
        }
    }
}
