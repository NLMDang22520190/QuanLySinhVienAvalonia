using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLySinhVien.Models;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class UserInfoViewModel : ViewModelBase
    {
        #region Properties

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
                Debug.WriteLine(_ngaySinh);
            }
        }

        private bool? _gioiTinh;

        public bool? GioiTinh
        {
            get => _gioiTinh;
            set
            {
                this.RaiseAndSetIfChanged(ref _gioiTinh, value);
            }
        }

        private string _diaChi = string.Empty;

        public string DiaChi
        {
            get => _diaChi;
            set => this.RaiseAndSetIfChanged(ref _diaChi, value);
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

        #endregion

        public ReactiveCommand<Window, Unit> UpdateUserInfoCommand { get; }

        public UserInfoViewModel(NguoiDung nguoiDung)
        {
            _initialNguoiDung = nguoiDung;

            TenDangNhap = nguoiDung.TenDangNhap;
            TenGiaoVien = nguoiDung.MaGiaoVienNavigation.TenGiaoVien;
            NgaySinh = (DateTime)nguoiDung.MaGiaoVienNavigation.NgaySinh;
            GioiTinh = nguoiDung.MaGiaoVienNavigation.GioiTinh;
            DiaChi = nguoiDung.MaGiaoVienNavigation.DiaChi;
            Email = nguoiDung.Email;

            if (GioiTinh == true)
            {
                SelectedGioiTinhIndex = 0;
            }
            else
            {
                SelectedGioiTinhIndex = 1;
            }
            // Create the IsValidObservable
            var IsValidObservable = this.WhenAnyValue(
                 x => x.DiaChi,
                 x => x.NgaySinh,
                 x => x.GioiTinh,
                 (diaChi, ngaySinh, gioiTinh) =>
                 {
                     bool IsChanged = diaChi != _initialNguoiDung.MaGiaoVienNavigation.DiaChi
                                      || ngaySinh != _initialNguoiDung.MaGiaoVienNavigation.NgaySinh
                                      || gioiTinh != _initialNguoiDung.MaGiaoVienNavigation.GioiTinh;

                     bool IsValid = !string.IsNullOrWhiteSpace(diaChi)
                                    && ngaySinh != DateTime.MinValue
                                    && gioiTinh.HasValue;

                     return IsChanged && IsValid;
                 });


            UpdateUserInfoCommand = ReactiveCommand.Create<Window>(UpdateUserInfo, IsValidObservable);
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
        
    }
}
