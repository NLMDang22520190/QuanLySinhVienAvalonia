using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLySinhVien.Models;
using QuanLySinhVien.Services;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.Login
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        #region Properties

        private DateTime lastCodeSentTime;

        private bool revealPassword;

        public bool RevealPassword
        {
            get => revealPassword;
            set => this.RaiseAndSetIfChanged(ref revealPassword, value);
        }

        private bool revealConfirmPassword;

        public bool RevealConfirmPassword
        {
            get => revealConfirmPassword;
            set => this.RaiseAndSetIfChanged(ref revealConfirmPassword, value);
        }

        private string tenDangNhap;

        public string TenDangNhap
        {
            get => tenDangNhap;
            set => this.RaiseAndSetIfChanged(ref tenDangNhap, value);
        }

        private string maXacNhan;

        public string MaXacNhan
        {
            get => maXacNhan;
            set => this.RaiseAndSetIfChanged(ref maXacNhan, value);
        }

        private string maXacNhanDaGui;
        public string MaXacNhanDaGui
        {
            get => maXacNhanDaGui;
            set => this.RaiseAndSetIfChanged(ref maXacNhanDaGui, value);
        }

        private string matKhauMoi;

        public string MatKhauMoi
        {
            get => matKhauMoi;
            set => this.RaiseAndSetIfChanged(ref matKhauMoi, value);
        }

        private string xacNhanMatKhau;

        public string XacNhanMatKhau
        {
            get => xacNhanMatKhau;
            set => this.RaiseAndSetIfChanged(ref xacNhanMatKhau, value);
        }


        #endregion

        public ReactiveCommand<Window, Unit> SavePasswordCommand { get; }
        public ReactiveCommand<Window, Unit> SendCodeCommand { get; }

        public ForgotPasswordViewModel()
        {
            var isValidObservable = this.WhenAnyValue(
                x => x.TenDangNhap,
                x => x.MaXacNhan,
                x => x.MatKhauMoi,
                x => x.XacNhanMatKhau,
                (TenDangNhap, MaXacNhan, MatKhauMoi, XacNhanMatKhau) =>
                {
                    return !string.IsNullOrWhiteSpace(TenDangNhap)
                           && !string.IsNullOrWhiteSpace(MaXacNhan)
                           && !string.IsNullOrWhiteSpace(MatKhauMoi)
                           && !string.IsNullOrWhiteSpace(XacNhanMatKhau)
                           && string.Equals(MatKhauMoi, XacNhanMatKhau);
                });

            var canSendCodeObservable = this.WhenAnyValue(
                x => x.lastCodeSentTime,
                x => x.TenDangNhap,
                (lastCodeSentTime, TenDangNhap) =>
                {
                    return (DateTime.Now - lastCodeSentTime).TotalSeconds > 30
                           && !string.IsNullOrWhiteSpace(TenDangNhap);
                });

            SavePasswordCommand = ReactiveCommand.Create<Window>(SavePassword, isValidObservable);
            SendCodeCommand = ReactiveCommand.Create<Window>(SendCode, canSendCodeObservable);
        }

        public async void SendCode(Window window)
        {
            string username = TenDangNhap;
            string email = GetTeacherEmailByUsername(username);
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Thông tin SMTP client (sử dụng Gmail trong ví dụ này)
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;

                // Tài khoản Gmail của bạn (tài khoản gửi email)
                string emailFromAddress = "tandungnguyen918@gmail.com"; // Email của bạn
                string password = "oese kloq vtgv mxii"; // Mật khẩu ứng dụng Gmail của bạn

                // Email người nhận
                string emailToAddress = email; // Email người nhận
                string subject = "Xác nhận email của bạn";

                // Tạo mã xác nhận ngẫu nhiên
                string verificationCode = GenerateVerificationCode();
                maXacNhanDaGui = verificationCode;
                string body = $"Mã xác nhận của bạn là: {verificationCode}";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailToAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                lastCodeSentTime = DateTime.Now;

                MessageBoxManager
                    .GetMessageBoxStandard("Thông báo", "Đã gửi mã xác nhận", ButtonEnum.Ok, Icon.Info)
                    .ShowWindowDialogAsync(window);
            }
            else
            {
                MessageBoxManager
                    .GetMessageBoxStandard("Lỗi", "Tên đăng nhập không tồn tại", ButtonEnum.Ok, Icon.Error)
                    .ShowWindowDialogAsync(window);

            }
        }


        public async void SavePassword(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn đổi mật khẩu không ?",
                ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                if (MaXacNhan == MaXacNhanDaGui)
                {
                    var context = DataProvider.Ins.DB;


                    var user = context.NguoiDungs
                        .Where(nd => nd.TenDangNhap == TenDangNhap)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        user.MatKhau = PasswordHasher.HashPassword(MatKhauMoi); // Hash mật khẩu mới trước khi lưu
                        context.SaveChanges();

                        await MessageBoxManager
                            .GetMessageBoxStandard("Thông báo", "Đổi mật khẩu thành công", ButtonEnum.Ok, Icon.Info)
                            .ShowWindowDialogAsync(window);
                        window.Close();

                    }
                    else
                    {
                        await MessageBoxManager
                            .GetMessageBoxStandard("Lỗi", "Không tìm thấy người dùng", ButtonEnum.Ok, Icon.Error)
                            .ShowWindowDialogAsync(window);
                    }

                }
                else
                {
                    await MessageBoxManager
                        .GetMessageBoxStandard("Lỗi", "Mã xác nhận không đúng", ButtonEnum.Ok, Icon.Error)
                        .ShowWindowDialogAsync(window);
                }
            }
        }

        public string GetTeacherEmailByUsername(string username)
        {
            var email = DataProvider.Ins.DB.NguoiDungs
                .Include(nd => nd.MaGiaoVienNavigation)
                .Where(nd => nd.TenDangNhap == username)
                .Select(nd => nd.Email)
                .FirstOrDefault();

            return email ?? "";
        }

        private static string GenerateVerificationCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void ChangePasswordTextBoxState()
        {
            RevealPassword = !RevealPassword;
        }

        public void ChangeConfirmPasswordTextBoxState()
        {
            RevealConfirmPassword = !RevealConfirmPassword;
        }
    }
}
