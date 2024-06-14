using Avalonia.Controls;
using QuanLySinhVien.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class EditStudentViewModel : ViewModelBase
    {
        #region Properties
        private string _maHocSinh = string.Empty;
        public string MaHocSinh
        {
            get => _maHocSinh;
            set => this.RaiseAndSetIfChanged(ref _maHocSinh, value);
        }

        private string _tenHocSinh = string.Empty;

        public string TenHocSinh
        {
            get => _tenHocSinh;
            set => this.RaiseAndSetIfChanged(ref _tenHocSinh, value);
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

        public ReactiveCommand<Window, HocSinh> EditCommand { get; }

        private readonly HocSinh _initialHocSinh;

        private int _selectedGioiTinhIndex;
        public int SelectedGioiTinhIndex
        {
            get => _selectedGioiTinhIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedGioiTinhIndex, value);
        }

        public EditStudentViewModel(HocSinh hocSinh)
        {
            LoadDoTuoiQuyDinh();
            _initialHocSinh = hocSinh;

            MaHocSinh = hocSinh.MaHocSinh;
            TenHocSinh = hocSinh.TenHocSinh;
            NgaySinh = (DateTime)hocSinh.NgaySinh;
            GioiTinh = hocSinh.GioiTinh;
            DiaChi = hocSinh.DiaChi;
            Email = hocSinh.Email;

            if (GioiTinh == true)
            {
                SelectedGioiTinhIndex = 0;
            }
            else
            {
                SelectedGioiTinhIndex = 1;
            }


            var isValidObservable = this.WhenAnyValue(
                x => x.TenHocSinh,
                x => x.DiaChi,
                x => x.Email,
                x => x.NgaySinh,
                x => x.GioiTinh,
                (tenHocSinh, diaChi, email, ngaySinh, gioiTinh) =>
                {
                    bool isChanged = tenHocSinh != _initialHocSinh.TenHocSinh
                                     || diaChi != _initialHocSinh.DiaChi
                                     || email != _initialHocSinh.Email
                                     || ngaySinh != _initialHocSinh.NgaySinh
                                     || gioiTinh != _initialHocSinh.GioiTinh;

                    bool isValid = !string.IsNullOrWhiteSpace(tenHocSinh)
                                   && !string.IsNullOrWhiteSpace(diaChi)
                                   && !string.IsNullOrWhiteSpace(email)
                                   && ngaySinh != DateTime.MinValue
                                   && gioiTinh.HasValue;

                    return isChanged && isValid;
                });
            EditCommand = ReactiveCommand.CreateFromTask<Window, HocSinh>(OnEdit, isValidObservable);

        }

        private async Task<HocSinh> OnEdit(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn sửa thông tin học sinh này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                var newHocSinh = new HocSinh
                {
                    MaHocSinh = MaHocSinh,
                    TenHocSinh = TenHocSinh,
                    NgaySinh = NgaySinh,
                    GioiTinh = GioiTinh,
                    DiaChi = DiaChi,
                    Email = Email
                };
                return newHocSinh;
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
                .Where(qd => qd.MaQuiDinh == "QD2")
                .Select(qd => qd.GiaTri)
                .FirstOrDefault();

            var tuoiToiDa = (int)context.QuiDinhs
                .Where(qd => qd.MaQuiDinh == "QD3")
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

