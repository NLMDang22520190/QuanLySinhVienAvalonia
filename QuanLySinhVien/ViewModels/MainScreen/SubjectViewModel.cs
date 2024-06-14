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
using System.Diagnostics;

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

        private string addName;
        public string AddName
        {
            get => addName;
            set
            {
                this.RaiseAndSetIfChanged(ref addName, value);
                Debug.Write(value);
            }
        }

        private string searchName;
        public string SearchName
        {
            get => searchName;
            set
            {
                this.RaiseAndSetIfChanged(ref searchName, value);
                Debug.Write(value);
            }
        }

        private string _newSubjectName;
        public string NewSubjectName
        {
            get => _newSubjectName;
            set => this.RaiseAndSetIfChanged(ref _newSubjectName, value);
        }


        public ReactiveCommand<Unit, Unit> LoadData { get; }
        public ReactiveCommand<Window, Unit> DeleteSubject { get; }
        public ReactiveCommand<Window, Unit> EditSubject { get; }
        public ReactiveCommand<Window, Unit> SubjectSearch { get; }
        public ReactiveCommand<TextBox, Unit> AddSubject { get; }
        public ReactiveCommand<Window, Unit> AddConfirm { get; }
        public ReactiveCommand<Window, Unit> SubjectSearchAll { get; }
        public ReactiveCommand<Unit, Unit> EditEnable { get; }
        public ReactiveCommand<Unit, Unit> LostFocusTxt { get; }

        public SubjectViewModel()
        {
            //EverLoaded = false;
            MonHocEditting = new MonHoc();
            LoadThongTinMonHoc();
            //LoadData = ReactiveCommand.CreateFromTask(async () =>
            //{
            //    if (!EverLoaded)
            //    {
            //        DataGridVisibility = false;
            //        ProgressBarVisibility = true;
            //        await LoadThongTinMonHoc();
            //        DataGridVisibility = true;
            //        ProgressBarVisibility = false;
            //        EverLoaded = true;
            //    }
            //});

            DeleteSubject = ReactiveCommand.CreateFromTask<Window>(async (window) =>
            {
                // Kiểm tra nếu không có môn học nào được chọn
                if (SelectedMonHocIndex == -1)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng chọn một môn học để xóa", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(window);
                    return;
                }

                // Lấy môn học được chọn từ danh sách

                var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn xoá môn học không?",
                ButtonEnum.YesNo, Icon.Question);
                var result = await box.ShowWindowDialogAsync(window);

                if (result == ButtonResult.Yes)
                {
                    try
                    {
                        var context = DataProvider.Ins.DB;
                        var selectedMonHoc = DanhSachMonHoc[SelectedMonHocIndex];
                        // Ensure this entity is detached before attaching a new instance
                        var existingEntity =
                            DataProvider.Ins.DB.MonHocs.Local.SingleOrDefault(hs => hs.MaMon == selectedMonHoc.MaMon);
                        if (existingEntity != null)
                        {
                            DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
                        }


                        context.MonHocs.Remove(selectedMonHoc);
                        await context.SaveChangesAsync();

                        // Cập nhật danh sách
                        DanhSachMonHoc.Remove(selectedMonHoc);

                        await MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa môn học thành công!", ButtonEnum.Ok, Icon.Success)
                            .ShowWindowDialogAsync(window);
                        LoadThongTinMonHoc();

                    }
                    catch (Exception ex)
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Có lỗi xảy ra trong quá trình xoá: {ex.Message}", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(window);
                    }
                }
            });


            EditSubject = ReactiveCommand.CreateFromTask<Window>(async (parameter) =>
            {
                // Kiểm tra nếu không có môn học nào được chọn
                if (SelectedMonHocIndex == -1)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng chọn một môn học để chỉnh sửa", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(parameter);
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
                        // Hiển thị hộp thoại nhập liệu
                        var inputBox = new InputBox("Nhập tên môn học mới", "Vui lòng nhập tên môn học mới:");
                        var result = await inputBox.ShowWindowDialogAsync(parameter);

                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            // Cập nhật thông tin của đối tượng MonHoc
                            monHocToUpdate.TenMon = result;

                            // Lưu các thay đổi vào cơ sở dữ liệu
                            await context.SaveChangesAsync();

                            // Cập nhật danh sách hiển thị
                            selectedMonHoc.TenMon = monHocToUpdate.TenMon;

                            await MessageBoxManager.GetMessageBoxStandard("Thông báo", "Chỉnh sửa môn học thành công!", ButtonEnum.Ok, Icon.Success)
                                .ShowWindowDialogAsync(parameter);
                            LoadThongTinMonHoc();
                        }
                        else
                        {
                            await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Tên môn học mới không hợp lệ.", ButtonEnum.Ok, Icon.Error)
                                .ShowWindowDialogAsync(parameter);
                        }
                    }
                    else
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không tìm thấy môn học để cập nhật", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(parameter);
                    }
                }
                catch (Exception ex)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Có lỗi xảy ra trong quá trình chỉnh sửa: {ex.Message}", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(parameter);
                }
            });



            SubjectSearch = ReactiveCommand.CreateFromTask<Window>(async (parameter) =>
            {
                if (string.IsNullOrEmpty(SearchName))
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng nhập tên môn học để tìm kiếm.", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(parameter);
                    return;
                }

                DanhSachMonHoc.Clear();
                ProgressBarVisibility = true;

                try
                {
                    var context = DataProvider.Ins.DB;

                    // Kiểm tra nếu không có kết nối đến cơ sở dữ liệu
                    if (context == null)
                    {
                        throw new InvalidOperationException("Không thể kết nối đến cơ sở dữ liệu.");
                    }

                    // Thực hiện truy vấn tìm kiếm
                    var searchResults = await context.MonHocs
                        .AsNoTracking()
                        .Where(m => m.TenMon.Contains(SearchName))
                        .ToListAsync();

                    // Kiểm tra kết quả truy vấn
                    if (searchResults == null || searchResults.Count == 0)
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Thông báo", "Không tìm thấy môn học phù hợp.", ButtonEnum.Ok, Icon.Warning).ShowWindowDialogAsync(parameter);
                        return;
                    }

                    // Thêm kết quả tìm kiếm vào DanhSachMonHoc
                    foreach (var monHoc in searchResults)
                    {
                        DanhSachMonHoc.Add(monHoc);
                    }
                }
                catch (Exception ex)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Có lỗi xảy ra trong quá trình tìm kiếm: {ex.Message}", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(parameter);
                }
                finally
                {
                    ProgressBarVisibility = false;
                }
            });


            AddConfirm = ReactiveCommand.CreateFromTask<Window>(async (parameter) =>
            {
                if (string.IsNullOrEmpty(AddName))
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Vui lòng nhập tên môn học", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(parameter);
                    return;
                }

                var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn thêm môn học không?",
                ButtonEnum.YesNo, Icon.Question);
                var result = await box.ShowWindowDialogAsync(parameter);

                if (result == ButtonResult.Yes)
                {
                    try
                    {
                        var context = DataProvider.Ins.DB;

                        // Kiểm tra sự tồn tại của môn học
                        var existingMonHoc = await context.MonHocs.FirstOrDefaultAsync(m => m.TenMon == AddName);
                        if (existingMonHoc != null)
                        {
                            await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Tên môn học đã tồn tại, vui lòng chọn tên khác.", ButtonEnum.Ok, Icon.Error)
                                .ShowWindowDialogAsync(parameter);
                            AddName = string.Empty;
                            return;
                        }

                        // Thêm môn học mới
                        var Count = DataProvider.Ins.DB.MonHocs.Count();
                        var newMonHoc = new MonHoc
                        {
                            MaMon = Count > 9 ? "MH" + Count.ToString() : "MH0" + Count.ToString(),
                            TenMon = AddName
                            // Thiết lập các thuộc tính khác nếu cần
                        };
                        context.MonHocs.Add(newMonHoc);
                        await context.SaveChangesAsync();

                        await MessageBoxManager.GetMessageBoxStandard("Thành công", "Thêm môn học thành công!", ButtonEnum.Ok, Icon.Success)
                            .ShowWindowDialogAsync(parameter);

                        AddName = string.Empty;
                        DataGridVisibility = false;
                        ProgressBarVisibility = true;
                        await LoadThongTinMonHoc(); // Tải lại thông tin môn học
                        DataGridVisibility = true;
                        ProgressBarVisibility = false;
                    }
                    catch (Exception ex)
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Có lỗi xảy ra trong quá trình thêm môn học: {ex.Message}", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(parameter);
                    }
                }
            });


            SubjectSearchAll = ReactiveCommand.CreateFromTask<Window>(async (window) =>
            {
                LoadThongTinMonHoc();
            });




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
