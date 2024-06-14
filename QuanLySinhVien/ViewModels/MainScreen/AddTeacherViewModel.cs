using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using QuanLySinhVien.Models;
using ReactiveUI;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddTeacherViewModel : ViewModelBase
    {
        #region Properties

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
        private DateTime _ngaySinhMinDate;

        public DateTime NgaySinhMinDate
        {
            get => _ngaySinhMinDate;
            set => this.RaiseAndSetIfChanged(ref _ngaySinhMinDate, value);
        }

        private DateTime _ngaySinhMaxDate;

        public DateTime NgaySinhMaxDate
        {
            get => _ngaySinhMaxDate;
            set => this.RaiseAndSetIfChanged(ref _ngaySinhMaxDate, value);
        }


        #endregion

        public ReactiveCommand<Window, GiaoVien> AddCommand { get; }

        public AddTeacherViewModel()
        {
            LoadDoTuoiQuyDinh();
            var isValidObservable = this.WhenAnyValue(
                x => x.TenGiaoVien,
                x => x.DiaChi,
                x => x.Email,
                x => x.NgaySinh,
                x => x.GioiTinh,
                (tenGiaoVien, diaChi, email, ngaySinh, gioiTinh) =>
                {
                    return !string.IsNullOrWhiteSpace(tenGiaoVien)
                           && !string.IsNullOrWhiteSpace(diaChi)
                           && !string.IsNullOrWhiteSpace(email)
                           && ngaySinh != DateTime.MinValue
                           && gioiTinh.HasValue;
                });
           
            AddCommand = ReactiveCommand.CreateFromTask<Window,GiaoVien>(OnAdd, isValidObservable);

        }
       
        private async Task<GiaoVien> OnAdd(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn thêm giáo viên này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                var newGiaoVien = new GiaoVien
                {
                    MaGiaoVien = "GV" + (DataProvider.Ins.DB.GiaoViens.Count() + 1).ToString(),
                    TenGiaoVien = TenGiaoVien,
                    NgaySinh = NgaySinh,
                    GioiTinh = GioiTinh,
                    DiaChi = DiaChi,
                    Email = Email
                };
                return newGiaoVien;
            }
            return null;
        }
            

        public void OnCancel(Window window)
        {
            window.Close();
        }

        private void LoadDoTuoiQuyDinh()
        {
            var context = DataProvider.Ins.DB;
            var tuoiToiThieu = (int)context.QuiDinhs
                .Where(qd => qd.MaQuiDinh == "QD4")
                .Select(qd => qd.GiaTri)
                .FirstOrDefault();

            var tuoiToiDa = (int)context.QuiDinhs
                .Where(qd => qd.MaQuiDinh == "QD4")
                .Select(qd => qd.GiaTri)
                .FirstOrDefault();

            // Kiểm tra xem giá trị có tồn tại không
            if (tuoiToiThieu != 0 && tuoiToiDa != 0)
            {
                // Tính toán ngày sinh tối thiểu và tối đa
                DateTime today = DateTime.Today;
                NgaySinhMinDate = today.AddYears(-tuoiToiDa);  // Ngày sinh tối thiểu dựa trên tuổi tối đa
                NgaySinhMaxDate = today.AddYears(-tuoiToiThieu);  // Ngày sinh tối đa dựa trên tuổi tối thiểu
            }
        }
    }
}
