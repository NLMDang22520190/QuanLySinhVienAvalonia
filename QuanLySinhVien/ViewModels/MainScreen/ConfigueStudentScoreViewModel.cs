using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ConfigueStudentScoreViewModel : ViewModelBase
    {
        #region Properties

        private ObservableCollection<HeThongDiem> heThongDiems;

        public ObservableCollection<HeThongDiem> HeThongDiems
        {
            get => heThongDiems;
            set => this.RaiseAndSetIfChanged(ref heThongDiems, value);
        }

        private ObservableCollection<HeThongDiem> allDiems;

        public ObservableCollection<HeThongDiem> AllDiems
        {
            get => allDiems;
            set => this.RaiseAndSetIfChanged(ref allDiems, value);
        }

        private int selectedDiemIndex;

        public int SelectedDiemIndex
        {
            get => selectedDiemIndex;
            set => this.RaiseAndSetIfChanged(ref selectedDiemIndex, value);
        }


        private bool isUpdating = false;

        private int selectedNienKhoaIndex = -1;

        public int SelectedNienKhoaIndex
        {
            get => selectedNienKhoaIndex;
            set
            {
                if (selectedNienKhoaIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedNienKhoaIndex, value);
                    if (!isUpdating)
                    {
                        UpdateScoreSearch();
                    }
                }
            }
        }

        private int selectedKhoiIndex = -1;

        public int SelectedKhoiIndex
        {
            get => selectedKhoiIndex;
            set
            {
                if (selectedKhoiIndex != value)
                {
                    this.RaiseAndSetIfChanged(ref selectedKhoiIndex, value);
                    if (!isUpdating)
                    {
                        UpdateScoreSearch();
                    }
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
                    if (!isUpdating)
                    {
                        UpdateScoreSearch();
                    }
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
                    if (!isUpdating)
                    {
                        UpdateScoreSearch();
                    }
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
                    if (!isUpdating)
                    {
                        UpdateScoreSearch();
                    }
                }
            }
        }

        private ObservableCollection<string> nienKhoasCb;

        public ObservableCollection<string> NienKhoasCb
        {
            get => nienKhoasCb;
            set => this.RaiseAndSetIfChanged(ref nienKhoasCb, value);
        }

        private ObservableCollection<string> khoisCb;

        public ObservableCollection<string> KhoisCb
        {
            get => khoisCb;
            set => this.RaiseAndSetIfChanged(ref khoisCb, value);
        }

        private ObservableCollection<string> lopsCb;

        public ObservableCollection<string> LopsCb
        {
            get => lopsCb;
            set
            {
                this.RaiseAndSetIfChanged(ref lopsCb, value);
            }
        }

        private ObservableCollection<string> hocKysCb;

        public ObservableCollection<string> HocKysCb
        {
            get => hocKysCb;
            set => this.RaiseAndSetIfChanged(ref hocKysCb, value);
        }

        private ObservableCollection<string> monHocsCb;

        public ObservableCollection<string> MonHocsCb
        {
            get => monHocsCb;
            set => this.RaiseAndSetIfChanged(ref monHocsCb, value);
        }

        private ObservableCollection<NienKhoa> nienKhoas;

        public ObservableCollection<NienKhoa> NienKhoas
        {
            get => nienKhoas;
            set => this.RaiseAndSetIfChanged(ref nienKhoas, value);
        }

        private ObservableCollection<Khoi> khois;

        public ObservableCollection<Khoi> Khois
        {
            get => khois;
            set => this.RaiseAndSetIfChanged(ref khois, value);
        }

        private ObservableCollection<Lop> lops;

        public ObservableCollection<Lop> Lops
        {
            get => lops;
            set => this.RaiseAndSetIfChanged(ref lops, value);
        }

        private ObservableCollection<HocKy> hocKys;

        public ObservableCollection<HocKy> HocKys
        {
            get => hocKys;
            set => this.RaiseAndSetIfChanged(ref hocKys, value);
        }

        private ObservableCollection<MonHoc> monHocs;

        public ObservableCollection<MonHoc> MonHocs
        {
            get => monHocs;
            set => this.RaiseAndSetIfChanged(ref monHocs, value);
        }


        #endregion

        public ConfigueStudentScoreViewModel()
        {
            LoadHeThongDiem();
            LoadFilter();
            LoadListComboBox();
        }

        #region Filter
        private void UpdateScoreSearch()
        {
            isUpdating = true;
            string khoi = selectedKhoiIndex != -1 ? khoisCb[selectedKhoiIndex] : null;
            string nienKhoa = selectedNienKhoaIndex != -1 ? NienKhoasCb[selectedNienKhoaIndex] : null;
            string lop = selectedLopIndex != -1 ? lopsCb[selectedLopIndex] : null;
            string hocKy = selectedHocKyIndex != -1 ? HocKysCb[selectedHocKyIndex] : null;
            string monHoc = selectedMonHocIndex != -1 ? MonHocsCb[selectedMonHocIndex] : null;
            SearchAndUpdateScores(khoi, nienKhoa, lop, hocKy, monHoc);
            isUpdating = false;
        }
        public void SearchAndUpdateScores(string khoi = null, string nienKhoa = null, string lop = null, string hocky = null, string monhoc = null)
        {
            var query = AllDiems.AsQueryable();
            if (!string.IsNullOrEmpty(nienKhoa))
            {
                var maNienKhoa = nienKhoas
                    .Where(nk => nk.TenNienKhoa == nienKhoa)
                    .Select(nk => nk.MaNienKhoa)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maNienKhoa))
                {
                    var danhSachMaLop = Lops
                        .Where(l => l.MaNienKhoa == maNienKhoa)
                        .Select(l => l.MaLop)
                        .ToList();

                    query = query.Where(hs => danhSachMaLop.Contains(hs.MaLop));
                }
            }

            if (!string.IsNullOrEmpty(khoi))
            {
                var maKhoi = Khois
                    .Where(k => k.TenKhoi == khoi)
                    .Select(k => k.MaKhoi)
                    .FirstOrDefault();


                var danhSachMaLop = Lops
                    .Where(l => l.MaKhoi == maKhoi)
                    .Select(l => l.MaLop)
                    .ToList();

                query = query.Where(hs => danhSachMaLop.Contains(hs.MaLop));

                UpdateLopComboBox(danhSachMaLop);

            }

            if (!string.IsNullOrEmpty(lop))
            {
                var maLop = Lops
                    .Where(l => l.TenLop == lop)
                    .Select(l => l.MaLop)
                    .FirstOrDefault();

                query = query.Where(hs => hs.MaLop == maLop);
            }

            if (!string.IsNullOrEmpty(hocky))
            {
                var maHocKy = HocKys
                    .Where(hk => hk.TenHocKy == hocky)
                    .Select(hk => hk.MaHocKy)
                    .FirstOrDefault();

                query = query.Where(hk => hk.MaHocKy == maHocKy);
            }

            if (!string.IsNullOrEmpty(monhoc))
            {
                var maMonHoc = MonHocs
                    .Where(mh => mh.TenMon == monhoc)
                    .Select(mh => mh.MaMon)
                    .FirstOrDefault();

                query = query.Where(mh => mh.MaMon == maMonHoc);
            }


            HeThongDiems.Clear();
            foreach (var student in query.ToList())
            {
                HeThongDiems.Add(student);
            }
        }

        private void UpdateLopComboBox(List<string> danhSachMaLop)
        {
            lopsCb.Clear();
            var filteredLops = Lops
                .Where(l => danhSachMaLop.Contains(l.MaLop))
                .Select(l => l.TenLop)
                .ToList();

            foreach (var lop in filteredLops)
            {
                lopsCb.Add(lop);
            }
        }
        #endregion


        #region DB Commands

        public void SaveData()
        {
            var _context = DataProvider.Ins.DB;
            foreach (var item in HeThongDiems)
            {
                var existingItem = _context.HeThongDiems.Find(item.Stt);
                if (existingItem != null)
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                }
                else
                {
                    _context.HeThongDiems.Add(item);
                }
            }
            _context.SaveChanges();
        }

        private void LoadListComboBox()
        {
            NienKhoasCb = new ObservableCollection<string>();
            foreach (var nk in nienKhoas)
            {
                nienKhoasCb.Add(nk.TenNienKhoa);
            }
            KhoisCb = new ObservableCollection<string>();
            foreach (var k in khois)
            {
                khoisCb.Add(k.TenKhoi);
            }
            LopsCb = new ObservableCollection<string>();
            foreach (var l in lops)
            {
                lopsCb.Add(l.TenLop);
            }
            HocKysCb = new ObservableCollection<string>();
            foreach (var hk in hocKys)
            {
                hocKysCb.Add(hk.TenHocKy);
            }
            MonHocsCb = new ObservableCollection<string>();
            foreach (var mh in monHocs)
            {
                monHocsCb.Add(mh.TenMon);
            }
        }

        private void LoadFilter()
        {
            var result1 = DataProvider.Ins.DB.NienKhoas.AsNoTracking().ToList();
            NienKhoas = new ObservableCollection<NienKhoa>(result1);
            var result2 = DataProvider.Ins.DB.Khois.AsNoTracking().ToList();
            Khois = new ObservableCollection<Khoi>(result2);
            var result3 = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            Lops = new ObservableCollection<Lop>(result3);
            var result4 = DataProvider.Ins.DB.HocKies.AsNoTracking().ToList();
            HocKys = new ObservableCollection<HocKy>(result4);
            var result5 = DataProvider.Ins.DB.MonHocs.AsNoTracking().ToList();
            MonHocs = new ObservableCollection<MonHoc>(result5);
        }

        private void LoadHeThongDiem()
        {
            var result = DataProvider.Ins.DB.HeThongDiems.Include("MaHocSinhNavigation").ToList();

            if (HeThongDiems == null)
            {
                HeThongDiems = new ObservableCollection<HeThongDiem>(result);
                AllDiems = new ObservableCollection<HeThongDiem>(result);
            }
            else
            {
                HeThongDiems.Clear();
                AllDiems.Clear();
                foreach (var hs in result)
                {
                    HeThongDiems.Add(hs);
                    AllDiems.Add(hs);
                }
            }
        }

        #endregion
    }
}
