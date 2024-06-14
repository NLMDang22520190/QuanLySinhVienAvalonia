using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using QuanLySinhVien.Models;
using ReactiveUI;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddAssignmentViewModel : ViewModelBase
    {
        private ObservableCollection<GiaoVien> listGiaoViens;

        public ObservableCollection<GiaoVien> ListGiaoViens
        {
            get => listGiaoViens;
            set => this.RaiseAndSetIfChanged(ref listGiaoViens, value);
        }
        private ObservableCollection<HocKy> listHocKys;

        public ObservableCollection<HocKy> ListHocKys
        {
            get => listHocKys;
            set => this.RaiseAndSetIfChanged(ref listHocKys, value);
        }

        private ObservableCollection<NienKhoa> listNienKhoas;

        public ObservableCollection<NienKhoa> ListNienKhoas
        {
            get => listNienKhoas;
            set => this.RaiseAndSetIfChanged(ref listNienKhoas, value);
        }

        private ObservableCollection<Khoi> listKhois;

        public ObservableCollection<Khoi> ListKhois
        {
            get => listKhois;
            set => this.RaiseAndSetIfChanged(ref listKhois, value);
        }

        private ObservableCollection<string> giaoVienCb;

        public ObservableCollection<string> GiaoVienCb
        {
            get => giaoVienCb;
            set => this.RaiseAndSetIfChanged(ref giaoVienCb, value);
        }
        private ObservableCollection<Lop> listLops;

        public ObservableCollection<Lop> ListLops
        {
            get => listLops;
            set => this.RaiseAndSetIfChanged(ref listLops, value);
        }

        private ObservableCollection<string> nienKhoaCb;

        public ObservableCollection<string> NienKhoaCb
        {
            get => nienKhoaCb;
            set => this.RaiseAndSetIfChanged(ref nienKhoaCb, value);
        }
        private ObservableCollection<string> hockyCb;

        public ObservableCollection<string> HocKyCb
        {
            get => hockyCb;
            set => this.RaiseAndSetIfChanged(ref hockyCb, value);
        }
        private ObservableCollection<MonHoc> listMonHocs;

        public ObservableCollection<MonHoc> ListMonHocs
        {
            get => listMonHocs;
            set => this.RaiseAndSetIfChanged(ref listMonHocs, value);
        }

        private ObservableCollection<string> khoiCb;

        public ObservableCollection<string> KhoiCb
        {
            get => khoiCb;
            set => this.RaiseAndSetIfChanged(ref khoiCb, value);
        }

        private ObservableCollection<string> lopCb;

        public ObservableCollection<string> LopCb
        {
            get => lopCb;
            set => this.RaiseAndSetIfChanged(ref lopCb, value);
        }
        private ObservableCollection<string> monHocCb;

        public ObservableCollection<string> MonHocCb
        {
            get => monHocCb;
            set => this.RaiseAndSetIfChanged(ref monHocCb, value);
        }
        private string _nienKhoa = string.Empty;
        public string NienKhoa
        {
            get => _nienKhoa;
            set => this.RaiseAndSetIfChanged(ref _nienKhoa, value);
        }
        private string _lop = string.Empty;
        public string Lop
        {
            get => _lop;
            set => this.RaiseAndSetIfChanged(ref _lop, value);
        }
        private string _hocKy = string.Empty;
        public string HocKy
        {
            get => _hocKy;
            set => this.RaiseAndSetIfChanged(ref _hocKy, value);
        }
        private string _monHoc = string.Empty;
        public string MonHoc
        {
            get => _monHoc;
            set => this.RaiseAndSetIfChanged(ref _monHoc, value);
        }
        private string _tenGVPT = string.Empty;
        public string TenGVPT
        {
            get => _tenGVPT;
            set => this.RaiseAndSetIfChanged(ref _tenGVPT, value);
        }

        private int selectedNienKhoaIndex = -1;

        public int SelectedNienKhoaIndex
        {
            get => selectedNienKhoaIndex;
            set
            {
                if (selectedNienKhoaIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedNienKhoaIndex, value);
                    
                }
            }
        }

        private int selectedHocKyIndex = -1;

        public int SelectedHocKyIndex
        {
            get => selectedHocKyIndex;
            set
            {
                if (selectedHocKyIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedHocKyIndex, value);

                }
            }
        }


        private int selectedLopIndex = -1;

        public int SelectedLopIndex
        {
            get => selectedLopIndex;
            set
            {
                if (selectedLopIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedLopIndex, value);
                   
                }
            }
        }

        private int selectedGiaoVienIndex = -1;

        public int SelectedGiaoVienIndex
        {
            get => selectedGiaoVienIndex;
            set
            {
                if (selectedGiaoVienIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedGiaoVienIndex, value);
                  
                }
            }
        }

        private int selectedMonHocIndex = -1;

        public int SelectedMonHocIndex
        {
            get => selectedMonHocIndex;
            set
            {
                if (selectedMonHocIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedMonHocIndex, value);
                    
                }
            }
        }

        public ReactiveCommand<Window, PhanCongGiangDay> AddCommand { get; }

        public AddAssignmentViewModel()
        {
            LoadList();
            LoadFilter();

            var isValidObservable = this.WhenAnyValue(
                x => x.SelectedNienKhoaIndex,
                x => x.SelectedHocKyIndex,
                x => x.SelectedLopIndex,
                x => x.SelectedMonHocIndex,
                x => x.SelectedGiaoVienIndex,
                (selectedNienKhoaIndex, selectedHocKyIndex, selectedLopIndex, selectedMonHocIndex, selectedGiaoVienIndex) =>
                {
                    return selectedNienKhoaIndex != -1
                            && selectedHocKyIndex != -1
                            && selectedLopIndex != -1
                            && selectedMonHocIndex != -1
                            && selectedGiaoVienIndex != -1;
                });


            AddCommand = ReactiveCommand.CreateFromTask<Window, PhanCongGiangDay>(OnAdd, isValidObservable);
        }
        private async Task<PhanCongGiangDay> OnAdd(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn thêm phân công ?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                var newPhanCong = new PhanCongGiangDay
                {
                    MaPhanCong = "PK" + (DataProvider.Ins.DB.HocSinhs.Count() + 1).ToString(),
                    MaNienKhoa = ListNienKhoas.Where(n => n.TenNienKhoa == ListNienKhoas[selectedNienKhoaIndex].TenNienKhoa).FirstOrDefault().MaNienKhoa,
                    MaHocKy = ListHocKys.Where(hk => hk.TenHocKy == ListHocKys[selectedHocKyIndex].TenHocKy).FirstOrDefault().MaHocKy,
                    MaLop = ListLops.Where(l => l.TenLop == ListLops[selectedLopIndex].TenLop).FirstOrDefault().MaLop,
                    MaMon = ListMonHocs.Where(mh => mh.TenMon == ListMonHocs[selectedMonHocIndex].TenMon).FirstOrDefault().MaMon,
                    MaGiaoVienPhuTrach = ListGiaoViens.Where(gv => gv.TenGiaoVien == ListGiaoViens[selectedGiaoVienIndex].TenGiaoVien).FirstOrDefault().MaGiaoVien,
                    TenGiaoVien = TenGVPT
                };
                return newPhanCong;
            }
            return null;

        }

        public void OnCancel(Window window)
        {
            window.Close();
        }

        public void LoadFilter()
        {
            GiaoVienCb = new ObservableCollection<string>();
            foreach (var gv in ListGiaoViens)
            {
                GiaoVienCb.Add(gv.TenGiaoVien);
            }

            NienKhoaCb = new ObservableCollection<string>();
            foreach (var nk in ListNienKhoas)
            {
                NienKhoaCb.Add(nk.TenNienKhoa);
            }

            LopCb = new ObservableCollection<string>();
            foreach (var l in ListLops)
            {
                LopCb.Add(l.TenLop);
            }

            HocKyCb = new ObservableCollection<string>();
            foreach (var hk in ListHocKys)
            {
                HocKyCb.Add(hk.TenHocKy);
            }

            MonHocCb = new ObservableCollection<string>();
            foreach (var mh in ListMonHocs)
            {
                MonHocCb.Add(mh.TenMon);
            }

        }

        public void LoadList()
        {
            // Tải danh sách giáo viên
            var giaoViens = DataProvider.Ins.DB.GiaoViens.AsNoTracking().ToList();
            ListGiaoViens = new ObservableCollection<GiaoVien>(giaoViens);

            // Tải danh sách niên khóa
            var nienKhoas = DataProvider.Ins.DB.NienKhoas.AsNoTracking().ToList();
            ListNienKhoas = new ObservableCollection<NienKhoa>(nienKhoas);

            // Tải danh sách học kỳ
            var hocKys = DataProvider.Ins.DB.HocKies.AsNoTracking().ToList();
            ListHocKys = new ObservableCollection<HocKy>(hocKys);

            // Tải danh sách lớp
            var lops = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            ListLops = new ObservableCollection<Lop>(lops);

            // Tải danh sách môn học
            var monHocs = DataProvider.Ins.DB.MonHocs.AsNoTracking().ToList();
            ListMonHocs = new ObservableCollection<MonHoc>(monHocs);
        }
    }
}
