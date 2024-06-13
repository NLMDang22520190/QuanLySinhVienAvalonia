using Avalonia.Controls;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class UserInfoView : UserControl
    {
        public UserInfoView()
        {
            InitializeComponent();

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
                        ((UserInfoViewModel)this.DataContext).GioiTinh = true;
                    }
                    else
                    {
                        ((UserInfoViewModel)this.DataContext).GioiTinh = false;
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
                        ((UserInfoViewModel)this.DataContext).NgaySinh = selectedDate.Value;
                    }
                };
            }
        }
    }
}
