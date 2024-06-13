using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Avalonia.Controls;
using ReactiveUI;
using QuanLySinhVien.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Reactive;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class SubjectViewModel : ViewModelBase
    {
        public bool EverLoaded { get; set; }
        public Window MonHocWD { get; set; }
        public bool IsLoadAll { get; set; } = false;

        private ObservableCollection<MonHoc> _danhSachMonHoc;
        public ObservableCollection<MonHoc> DanhSachMonHoc
        {
            get => _danhSachMonHoc;
            set => this.RaiseAndSetIfChanged(ref _danhSachMonHoc, value);
        }

        private MonHoc _monHocEditting;
        public MonHoc MonHocEditting
        {
            get => _monHocEditting;
            set => this.RaiseAndSetIfChanged(ref _monHocEditting, value);
        }


        private int selectedMonHocIndex;

        public int SelectedMonHocIndex
        {
            get => selectedMonHocIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedMonHocIndex, value);
            }
        }

        private bool _dataGridVisibility;
        public bool DataGridVisibility
        {
            get => _dataGridVisibility;
            set => this.RaiseAndSetIfChanged(ref _dataGridVisibility, value);
        }

        private bool _progressBarVisibility;
        public bool ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set => this.RaiseAndSetIfChanged(ref _progressBarVisibility, value);
        }

        public ReactiveCommand<Unit, Unit> LoadData { get; }
        public ReactiveCommand<object, Unit> DeleteSubject { get; }
        public ReactiveCommand<object, Unit> EditSubject { get; }
        public ReactiveCommand<TextBox, Unit> SubjectSearch { get; }
        public ReactiveCommand<TextBox, Unit> AddSubject { get; }
        public ReactiveCommand<TextBox, Unit> AddConfirm { get; }
        public ReactiveCommand<TextBox, Unit> SubjectSearchAll { get; }
        public ReactiveCommand<Unit, Unit> EditEnable { get; }
        public ReactiveCommand<Unit, Unit> LostFocusTxt { get; }

        public SubjectViewModel()
        {
            EverLoaded = false;
            MonHocEditting = new MonHoc();

            LoadData = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!EverLoaded)
                {
                    DataGridVisibility = false;
                    ProgressBarVisibility = true;
                    await LoadThongTinMonHoc();
                    DataGridVisibility = true;
                    ProgressBarVisibility = false;
                    EverLoaded = true;
                }
            });

            DeleteSubject = ReactiveCommand.CreateFromTask<object>(async (parameter) =>
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn xoá môn học không?",
                ButtonEnum.YesNo, Icon.Question);
                var result = await box.ShowWindowDialogAsync(MonHocWD);

                if (result == ButtonResult.Yes)
                {
                    var mh = parameter as MonHoc;
                    if (mh != null)
                    {
                        var context = DataProvider.Ins.DB;

                        // Tìm đối tượng MonHoc trong cơ sở dữ liệu và xoá nó
                        var entity = await context.MonHocs.FirstOrDefaultAsync(m => m.MaMon == mh.MaMon);
                        if (entity != null)
                        {
                            context.MonHocs.Remove(entity);
                            await context.SaveChangesAsync();

                            // Cập nhật danh sách
                            DanhSachMonHoc.Remove(mh);

                            await MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa môn học thành công!", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(MonHocWD);
                        }
                        else
                        {
                            await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không tìm thấy môn học để xóa", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(MonHocWD);
                        }
                    }
                }
            });

            EditSubject = ReactiveCommand.CreateFromTask<object>(async (parameter) =>
            {
                // Kiểm tra nếu không có môn học nào được chọn
                if (SelectedMonHocIndex == -1)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng chọn một môn học để chỉnh sửa", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(MonHocWD);
                    return;
                }

                // Lấy môn học được chọn
                var selectedMonHoc = DanhSachMonHoc[SelectedMonHocIndex];

                try
                {
                    var context = DataProvider.Ins.DB;

                    // Tìm đối tượng MonHoc trong cơ sở dữ liệu
                    var monHocToUpdate = await context.MonHocs.FirstOrDefaultAsync(mh => mh.MaMon == selectedMonHoc.MaMon);

                    if (monHocToUpdate != null)
                    {
                        // Cập nhật thông tin của đối tượng MonHoc
                        monHocToUpdate.TenMon = "Tên môn học mới"; // Ví dụ giá trị mới cho tên môn học

                        // Lưu các thay đổi vào cơ sở dữ liệu
                        await context.SaveChangesAsync();

                        // Cập nhật danh sách hiển thị
                        selectedMonHoc.TenMon = monHocToUpdate.TenMon;

                        await MessageBoxManager.GetMessageBoxStandard("Thông báo", "Chỉnh sửa môn học thành công!", ButtonEnum.Ok, Icon.Success)
                            .ShowWindowDialogAsync(MonHocWD);
                    }
                    else
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không tìm thấy môn học để cập nhật", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(MonHocWD);
                    }
                }
                catch (Exception ex)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Có lỗi xảy ra trong quá trình chỉnh sửa: {ex.Message}", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(MonHocWD);
                }
            });

            SubjectSearch = ReactiveCommand.CreateFromTask<TextBox>(async (parameter) =>
            {

                if (parameter == null || string.IsNullOrWhiteSpace(parameter.Text))
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng nhập tên môn học để tìm kiếm.", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(MonHocWD);
                    return;
                }

                DanhSachMonHoc.Clear();
                ProgressBarVisibility = true;

                try
                {
                    var context = DataProvider.Ins.DB;
                    var searchResults = await context.MonHocs
                        .Where(m => m.TenMon.Contains(parameter.Text))
                        .ToListAsync();

                    foreach (var monHoc in searchResults)
                    {
                        DanhSachMonHoc.Add(monHoc);
                    }
                }
                catch (Exception ex)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không tìm thấy môn học cần tìm.", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(MonHocWD);
                }
                finally
                {
                    ProgressBarVisibility = false;
                }

            });

            AddConfirm = ReactiveCommand.CreateFromTask<TextBox>(async (parameter) =>
            {
                var monhoc = parameter.Text;
                if (string.IsNullOrEmpty(monhoc))
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng nhập tên môn học", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(MonHocWD);
                    return;
                }

                var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn thêm môn học không?",
                ButtonEnum.YesNo, Icon.Question);
                var result = await box.ShowWindowDialogAsync(MonHocWD);

                if (result == ButtonResult.Yes)
                {
                    using (var con = new SqlConnection(ConnectionString.connectionString))
                    {
                        try
                        {
                            await con.OpenAsync();
                            var cmdCheck = new SqlCommand("select count(*) from MonHoc where TenMon = @TenMon", con);
                            cmdCheck.Parameters.AddWithValue("@TenMon", monhoc);
                            int count = (int)await cmdCheck.ExecuteScalarAsync();
                            if (count > 0)
                            {
                                var messageBoxExists = new MessageBoxOK { Content = "Tên môn học đã tồn tại, vui lòng chọn tên khác." };
                                await messageBoxExists.ShowDialog(MonHocWD);
                                MonHocWD.txtTenMH.Text = string.Empty;
                                return;
                            }

                            var cmd = new SqlCommand("insert into MonHoc (TenMon, MaTruong, ApDung) values (@TenMon, 1, 1)", con);
                            cmd.Parameters.AddWithValue("@TenMon", monhoc);
                            await cmd.ExecuteNonQueryAsync();
                            var successMessageBox = new MessageBoxSuccessful();
                            await successMessageBox.ShowDialog(MonHocWD);
                            MonHocWD.txtTenMH.Text = string.Empty;
                            DataGridVisibility = false;
                            ProgressBarVisibility = true;
                            await LoadThongTinMonHoc();
                            DataGridVisibility = true;
                            ProgressBarVisibility = false;
                        }
                        catch (Exception ex)
                        {
                            await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Có lỗi xảy ra trong quá trình chỉnh sửa: {ex.Message}", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(MonHocWD);
                        }
                    }
                }
            });




            // Các lệnh khác sẽ được khởi tạo tương tự như vậy...
        }

        public async Task LoadThongTinMonHoc()
        {
            var result = DataProvider.Ins.DB.MonHocs.AsNoTracking().ToList();
            if (DanhSachMonHoc == null)
            {
                DanhSachMonHoc = new ObservableCollection<MonHoc>(result);
            }
            else
            {
                DanhSachMonHoc.Clear();
                foreach (var mh in result)
                {
                    DanhSachMonHoc.Add(mh);
                }
            }
        }
    }
}
