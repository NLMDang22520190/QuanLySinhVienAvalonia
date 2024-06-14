using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLySinhVien.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class EditTeacherViewModel : ViewModelBase
    {
        #region Properties
        private string _maGiaoVien = string.Empty;
        public string MaGiaoVien
        {
            get => _maGiaoVien;
            set => this.RaiseAndSetIfChanged(ref _maGiaoVien, value);
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

        public ReactiveCommand<Window, GiaoVien> EditCommand { get; }

        private readonly GiaoVien _initialGiaoVien;

        private int _selectedGioiTinhIndex;
        public int SelectedGioiTinhIndex
        {
            get => _selectedGioiTinhIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedGioiTinhIndex, value);
        }

        public EditTeacherViewModel(GiaoVien giaoVien)
        {
            LoadDoTuoiQuyDinh();
            _initialGiaoVien = giaoVien;

            MaGiaoVien = giaoVien.MaGiaoVien;
            TenGiaoVien = giaoVien.TenGiaoVien;
            NgaySinh = (DateTime)giaoVien.NgaySinh;
            GioiTinh = giaoVien.GioiTinh;
            DiaChi = giaoVien.DiaChi;
            Email = giaoVien.Email;

            if(GioiTinh == true)
            {
                SelectedGioiTinhIndex = 0;
            }
            else
            {
                SelectedGioiTinhIndex = 1;
            }


            var isValidObservable = this.WhenAnyValue(
                x => x.TenGiaoVien,
                x => x.DiaChi,
                x => x.Email,
                x => x.NgaySinh,
                x => x.GioiTinh,
                (tenGiaoVien, diaChi, email, ngaySinh, gioiTinh) =>
                {
                    bool isChanged = tenGiaoVien != _initialGiaoVien.TenGiaoVien
                                     || diaChi != _initialGiaoVien.DiaChi
                                     || email != _initialGiaoVien.Email
                                     || ngaySinh != _initialGiaoVien.NgaySinh
                                     || gioiTinh != _initialGiaoVien.GioiTinh;

                    bool isValid = !string.IsNullOrWhiteSpace(tenGiaoVien)
                                   && !string.IsNullOrWhiteSpace(diaChi)
                                   && !string.IsNullOrWhiteSpace(email)
                                   && ngaySinh != DateTime.MinValue
                                   && gioiTinh.HasValue;

                    return isChanged && isValid;
                });
            EditCommand = ReactiveCommand.CreateFromTask<Window, GiaoVien>(OnEdit, isValidObservable);

        }

        private async Task<GiaoVien> OnEdit(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn sửa thông tin giáo viên này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                Debug.WriteLine(TenGiaoVien);
                var newGiaoVien = new GiaoVien
                {
                    MaGiaoVien = MaGiaoVien,
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
