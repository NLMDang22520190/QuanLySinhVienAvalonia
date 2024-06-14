using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class UserInfoViewModel : ViewModelBase
    {
        #region Properties

        public static string maNguoiDung { get; set; }

        private string _maNguoiDung;

        public string MaNguoiDung
        {
            get => _maNguoiDung;
            set => this.RaiseAndSetIfChanged(ref _maNguoiDung, value);
        }

        private string _maGiaoVien;

        public string MaGiaoVien
        {
            get => _maGiaoVien;
            set => this.RaiseAndSetIfChanged(ref _maGiaoVien, value);
        }

        private string _tenDangNhap;

        public string TenDangNhap
        {
            get => _tenDangNhap;
            set => this.RaiseAndSetIfChanged(ref _tenDangNhap, value);
        }


        private string _tenGiaoVien = string.Empty;

        public string TenGiaoVien
        {
            get => _tenGiaoVien;
            set => this.RaiseAndSetIfChanged(ref _tenGiaoVien, value);
        }

        private DateTime _ngaySinh;

        public DateTime NgaySinh
        {
            get => _ngaySinh;
            set
            {
                this.RaiseAndSetIfChanged(ref _ngaySinh, value);
                IsSaved = false; // Mark as unsaved

            }
        }

        private bool? _gioiTinh;

        public bool? GioiTinh
        {
            get => _gioiTinh;
            set
            {
                this.RaiseAndSetIfChanged(ref _gioiTinh, value);
                IsSaved = false; // Mark as unsaved

            }
        }

        private string _diaChi = string.Empty;

        public string DiaChi
        {
            get => _diaChi;
            set
            {
                this.RaiseAndSetIfChanged(ref _diaChi, value);
                IsSaved = false; // Mark as unsaved

            } 
        }

        private string _email = string.Empty;

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }



        private int _selectedGioiTinhIndex;

        public int SelectedGioiTinhIndex
        {
            get => _selectedGioiTinhIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedGioiTinhIndex, value);
        }

        private readonly NguoiDung _initialNguoiDung;

        private bool _isSaved; // Track if the data is saved successfully

        public bool IsSaved
        {
            get => _isSaved;
            set => this.RaiseAndSetIfChanged(ref _isSaved, value);
        }


        #endregion

        public ReactiveCommand<Window, Unit> UpdateUserInfoCommand { get; }
        public ReactiveCommand<Window, Unit> OpenChangePasswordWindowCommand { get; }

        public UserInfoViewModel()
        {
            _initialNguoiDung = DataProvider.Ins.DB.NguoiDungs
                .Include("MaGiaoVienNavigation")
                .FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);

            LoadAndSetUpData();
            // Create the IsValidObservable
            var IsValidObservable = this.WhenAnyValue(
                x => x.DiaChi,
                x => x.NgaySinh,
                x => x.GioiTinh,
                x => x.IsSaved, // Include _isSaved in the observable
                (diaChi, ngaySinh, gioiTinh, IsSaved) =>
                {
                    bool IsChanged = diaChi != _initialNguoiDung.MaGiaoVienNavigation.DiaChi
                                     || ngaySinh != _initialNguoiDung.MaGiaoVienNavigation.NgaySinh
                                     || gioiTinh != _initialNguoiDung.MaGiaoVienNavigation.GioiTinh;

                    bool IsValid = !string.IsNullOrWhiteSpace(diaChi)
                                   && ngaySinh != DateTime.MinValue
                                   && gioiTinh.HasValue;

                    return IsChanged && IsValid && !IsSaved; // Only enable if data is valid and not saved
                });


            UpdateUserInfoCommand = ReactiveCommand.Create<Window>(UpdateUserInfo, IsValidObservable);
            OpenChangePasswordWindowCommand = ReactiveCommand.Create<Window>(OpenChangePasswordWindow);
        }

        private async void UpdateUserInfo(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", 
                "Bạn có chắc chắn muốn cập nhật thông tin không?",
                ButtonEnum.YesNo,
                Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                var context = DataProvider.Ins.DB;

                // Lấy ra mã giáo viên từ người dùng hiện tại
                string maGiaoVien = _initialNguoiDung.MaGiaoVien;

                // Truy xuất thông tin của giáo viên từ cơ sở dữ liệu
                var giaoVien = context.GiaoViens.FirstOrDefault(gv => gv.MaGiaoVien == maGiaoVien);

                if (giaoVien != null)
                {
                    // Cập nhật thông tin của giáo viên dựa trên dữ liệu mới từ người dùng
                    giaoVien.TenGiaoVien = TenGiaoVien;
                    giaoVien.NgaySinh = NgaySinh;
                    giaoVien.GioiTinh = GioiTinh;
                    giaoVien.DiaChi = DiaChi;

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();
                    IsSaved = true; // Mark as saved
                    Debug.WriteLine(_isSaved);

                    // Hiển thị thông báo cập nhật thành công
                    MessageBoxManager.GetMessageBoxStandard("Thông báo", "Thông tin đã được cập nhật thành công!")
                        .ShowWindowDialogAsync(window);
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu không tìm thấy giáo viên
                    MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không tìm thấy thông tin của giáo viên!")
                        .ShowWindowDialogAsync(window);
                }


            }
        }

        private void OpenChangePasswordWindow(Window window)
        {

            var selectedUser = _initialNguoiDung;
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.NguoiDungs.Local.SingleOrDefault(gv => gv.MaNguoiDung == selectedUser.MaNguoiDung);
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            var changePasswordWindow = new ChangePasswordView();
            var changePasswordViewModel = new ChangePasswordViewModel(selectedUser);
            changePasswordWindow.DataContext = changePasswordViewModel;


            changePasswordWindow.Title = "Đổi mật khẩu";
            changePasswordWindow.ShowDialog(window);

            changePasswordViewModel.ChangePasswordCommand
                .Take(1)
                .Subscribe(gv =>
                {
                    if (gv != null)
                    {
                        DataProvider.Ins.DB.NguoiDungs.Update(gv);
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Đổi mật khẩu thành công !", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                        changePasswordWindow.Close();

                    }
                });
        }

        private void LoadAndSetUpData()
        {
            TenDangNhap = _initialNguoiDung.TenDangNhap;
            TenGiaoVien = _initialNguoiDung.MaGiaoVienNavigation.TenGiaoVien;
            NgaySinh = (DateTime)_initialNguoiDung.MaGiaoVienNavigation.NgaySinh;
            GioiTinh = _initialNguoiDung.MaGiaoVienNavigation.GioiTinh;
            DiaChi = _initialNguoiDung.MaGiaoVienNavigation.DiaChi;
            Email = _initialNguoiDung.Email;

            if (GioiTinh == true)
            {
                SelectedGioiTinhIndex = 0;
            }
            else
            {
                SelectedGioiTinhIndex = 1;
            }
            IsSaved = true; // Initially set to true since data is loaded from the database

        }

    }
}
