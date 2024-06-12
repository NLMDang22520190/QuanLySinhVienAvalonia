using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using QuanLySinhVien.Models;
using QuanLySinhVien.Services;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly NguoiDung _initialNguoiDung;

        private string _matKhauCu = string.Empty;

        public string MatKhauCu
        {
            get => _matKhauCu;
            set => this.RaiseAndSetIfChanged(ref _matKhauCu, value);
        }

        private string _matKhauMoi = string.Empty;

        public string MatKhauMoi
        {
            get => _matKhauMoi;
            set => this.RaiseAndSetIfChanged(ref _matKhauMoi, value);
        }

        private string _xacNhanMatKhauMoi = string.Empty;

        public string XacNhanMatKhauMoi
        {
            get => _xacNhanMatKhauMoi;
            set => this.RaiseAndSetIfChanged(ref _xacNhanMatKhauMoi, value);
        }

        public ReactiveCommand<Window, NguoiDung> ChangePasswordCommand { get; }
        public ChangePasswordViewModel(NguoiDung nguoidung)
        {
            _initialNguoiDung = nguoidung;

            var isValidObservable = this.WhenAnyValue(
                x => x.MatKhauCu,
                x => x.MatKhauMoi,
                x => x.XacNhanMatKhauMoi,
                (matKhauCu, matKhauMoi, xacNhanMatKhauMoi) =>
                {
                    return !string.IsNullOrWhiteSpace(matKhauCu) &&
                           !string.IsNullOrWhiteSpace(matKhauMoi) &&
                           !string.IsNullOrWhiteSpace(xacNhanMatKhauMoi) &&
                           matKhauMoi == xacNhanMatKhauMoi;
                });

            ChangePasswordCommand = ReactiveCommand.CreateFromTask<Window, NguoiDung>(ChangePassword, isValidObservable);
        }

        private async Task<NguoiDung> ChangePassword(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn cập nhật mật khẩu không ?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                if(MatKhauMoi == MatKhauCu)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Mật khẩu mới không được trùng với mật khẩu cũ", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
                    return null;
                }
                var newUser = new NguoiDung
                {
                    MaNguoiDung = _initialNguoiDung.MaNguoiDung,
                    MaGiaoVien = _initialNguoiDung.MaGiaoVien,
                    TenDangNhap = _initialNguoiDung.TenDangNhap,
                    MatKhau = PasswordHasher.HashPassword(MatKhauMoi),
                    ChucNang = _initialNguoiDung.ChucNang,
                };
                return newUser;
            }

            return null;
        }

    }
}
