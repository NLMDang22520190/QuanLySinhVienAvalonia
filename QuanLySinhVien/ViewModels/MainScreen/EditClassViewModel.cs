using Avalonia.Controls;
using QuanLySinhVien.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class EditClassViewModel : ViewModelBase
    {

        private string _maLop = string.Empty;
        public string MaLop
        {
            get => _maLop;
            set => this.RaiseAndSetIfChanged(ref _maLop, value);
        }

        private string _tenLop = string.Empty;

        public string TenLop
        {
            get => _tenLop;
            set => this.RaiseAndSetIfChanged(ref _tenLop, value);
        }

        private string _maKhoi = string.Empty;
        public string MaKhoi
        {
            get => _maKhoi;
            set => this.RaiseAndSetIfChanged(ref _maKhoi, value);
        }

        private string _maNienKhoa = string.Empty;
        public string MaNienKhoa
        {
            get => _maNienKhoa;
            set => this.RaiseAndSetIfChanged(ref _maNienKhoa, value);
        }

        private int? _siSo = 0;
        public int? SiSo
        {
            get => _siSo;
            set
            {
                this.RaiseAndSetIfChanged(ref _siSo, value);
            }
        }

        private string _tenGvcn = string.Empty;
        public string TenGvcn
        {
            get => _tenGvcn;
            set => this.RaiseAndSetIfChanged(ref _tenGvcn, value);
        }


        public ReactiveCommand<Unit, Lop> EditCommand { get; }
        private readonly Lop _initialLop;

        private int _selectedKhoiIndex;
        public int SelectedKhoiIndex
        {
            get => _selectedKhoiIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedKhoiIndex, value);
        }

        private int _selectedNienKhoaIndex;
        public int SelectedNienKhoaIndex
        {
            get => _selectedNienKhoaIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedNienKhoaIndex, value);
        }

        public EditClassViewModel(Lop lop)
        {
            _initialLop = lop;

            MaLop = lop.MaLop;
            TenLop = lop.TenLop;                  
            MaKhoi = lop.MaKhoi;
            MaNienKhoa = lop.MaNienKhoa;
            SiSo = lop.SiSo;
            TenGvcn = lop.TenGvcn;

            if (lop.MaKhoi == "K10")
            {
                SelectedKhoiIndex = 0;
            }
            else if (lop.MaKhoi == "K11")
            {
                SelectedKhoiIndex = 1;
            }
            else if (lop.MaKhoi == "K12")
            {
                SelectedKhoiIndex = 2;
            }
            //Nien khoa
            if (lop.MaNienKhoa == "NK2021")
            {
                SelectedNienKhoaIndex = 0;
            }
            else if (lop.MaKhoi == "NK2022")
            {
                SelectedNienKhoaIndex = 1;
            }
            else if (lop.MaKhoi == "NK2023")
            {
                SelectedNienKhoaIndex = 2;
            }

            var isValidObservable = this.WhenAnyValue(
               x => x.TenLop,
               x => x.MaNienKhoa,
               x => x.MaKhoi,
               x => x.SiSo,
               x => x.TenGvcn,
                                                 
                (tenLop, nienkhoa, khoi, siso, tengvcn) =>
                {
                    bool isChanged = tenLop != _initialLop.TenLop
                                     || nienkhoa != _initialLop.MaNienKhoa
                                     || khoi != _initialLop.MaKhoi
                                     || siso != _initialLop.SiSo
                                     || tengvcn != _initialLop.TenGvcn;

                    bool isValid = !string.IsNullOrWhiteSpace(tenLop)
                                   && !string.IsNullOrWhiteSpace(nienkhoa)
                                   && !string.IsNullOrWhiteSpace(khoi)
                                   && !string.IsNullOrWhiteSpace(tengvcn)
                                   && SiSo.HasValue;

                    return isChanged && isValid;
                });
            EditCommand = ReactiveCommand.Create<Lop>(OnEdit, isValidObservable);
        }

        private Lop OnEdit()
        {
            var newLop = new Lop
            {
                MaLop = MaLop,
                TenLop = TenLop,
                MaKhoi = MaKhoi,
                MaNienKhoa = MaNienKhoa,
                SiSo = SiSo,
                TenGvcn = TenGvcn,
            };
            return newLop;
        }

        public void OnCancel(Window window)
        {
            window.Close();
        }
    }
}
