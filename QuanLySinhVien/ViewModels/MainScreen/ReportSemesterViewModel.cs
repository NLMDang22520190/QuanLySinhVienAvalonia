using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;
using Avalonia.Controls;
using System.Reactive.Linq;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ReportSemesterViewModel : ViewModelBase
    {
        private ObservableCollection<BaoCaoHocKy> listBaoCaoHocKies;

        public ObservableCollection<BaoCaoHocKy> ListBaoCaoHocKies
        {
            get => listBaoCaoHocKies;
            set => this.RaiseAndSetIfChanged(ref listBaoCaoHocKies, value);
        }

        private ObservableCollection<BaoCaoHocKy> allBaoCaoHocKies;

        public ObservableCollection<BaoCaoHocKy> AllBaoCaoHocKies
        {
            get => allBaoCaoHocKies;
            set => this.RaiseAndSetIfChanged(ref allBaoCaoHocKies, value);
        }

        private ObservableCollection<Lop> lops;

        public ObservableCollection<Lop> Lops
        {
            get => lops;
            set => this.RaiseAndSetIfChanged(ref lops, value);
        }

        private int selectedBaoCaoHocKyIndex;

        public int SelectedBaoCaoHocKyIndex
        {
            get => selectedBaoCaoHocKyIndex;
            set => this.RaiseAndSetIfChanged(ref selectedBaoCaoHocKyIndex, value);
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
                    if (!isUpdating)
                    {
                        UpdateReportSearch();
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
                        UpdateReportSearch();
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
                        UpdateReportSearch();
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

        private ObservableCollection<string> hocKiesCb;

        public ObservableCollection<string> HocKiesCb
        {
            get => hocKiesCb;
            set
            {
                this.RaiseAndSetIfChanged(ref hocKiesCb, value);
            }
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

        private ObservableCollection<HocKy> hocKies;

        public ObservableCollection<HocKy> Hockies
        {
            get => hocKies;
            set => this.RaiseAndSetIfChanged(ref hocKies, value);
        }

        private bool isUpdating = false;

        public ReportSemesterViewModel()
        {
            LoadListBaoCaoHocKies();
            LoadFilter();
            var result = DataProvider.Ins.DB.BaoCaoHocKies.ToList();
            listBaoCaoHocKies = new ObservableCollection<BaoCaoHocKy>(result);
            var result1 = DataProvider.Ins.DB.HocKies.Select(hk => hk.TenHocKy).ToList();
            HocKiesCb = new ObservableCollection<string>(result1);
            var result2 = DataProvider.Ins.DB.NienKhoas.Select(nk => nk.TenNienKhoa).ToList();
            NienKhoasCb = new ObservableCollection<string>(result2);
            var result3 = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result3);

        }

        private void UpdateReportSearch ()
        {
            isUpdating = true;
            string khoi = selectedKhoiIndex != -1 ? khoisCb[selectedKhoiIndex] : null;
            string nienKhoa = selectedNienKhoaIndex != -1 ? NienKhoasCb[selectedNienKhoaIndex] : null;
            string hocKy = selectedHocKyIndex != -1 ? hocKiesCb[selectedHocKyIndex] : null;
            SearchAndUpdateReport(khoi, nienKhoa,hocKy);
            isUpdating = false;
        }

        public void SearchAndUpdateReport(string khoi = null, string nienKhoa = null, string hocKy = null)
        {
            var query = AllBaoCaoHocKies.AsQueryable();
            Debug.WriteLine("SearchAndUpdateReport");
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

                query = query.Where(l => danhSachMaLop.Contains(l.MaLop));


            }

            if (!string.IsNullOrEmpty(hocKy))
            {
               var maHocKy = Hockies
                    .Where(hk => hk.TenHocKy == hocKy)
                    .Select(hk => hk.MaHocKy)
                    .FirstOrDefault();

                query = query.Where(hs => hs.MaHocKy == maHocKy);
            }


        }

        private void LoadListBaoCaoHocKies()
        {
            var result = DataProvider.Ins.DB.BaoCaoHocKies.Include("MaLopNavigation").ToList();
            if (ListBaoCaoHocKies == null)
            {
                ListBaoCaoHocKies = new ObservableCollection<BaoCaoHocKy>(result);
                AllBaoCaoHocKies = new ObservableCollection<BaoCaoHocKy>(result);
            }
            else
            {
                ListBaoCaoHocKies.Clear();
                AllBaoCaoHocKies.Clear();
                foreach (var bkhk in result)
                {
                    ListBaoCaoHocKies.Add(bkhk);
                    AllBaoCaoHocKies.Add(bkhk);
                }
            }

        }

        private void LoadFilter()
        {
            var result1 = DataProvider.Ins.DB.NienKhoas.AsNoTracking().ToList();
            NienKhoas = new ObservableCollection<NienKhoa>(result1);
            var result2 = DataProvider.Ins.DB.Khois.AsNoTracking().ToList();
            Khois = new ObservableCollection<Khoi>(result2);
            var result3 = DataProvider.Ins.DB.HocKies.AsNoTracking().ToList();
            Hockies = new ObservableCollection<HocKy>(result3);
        }
    }
}
