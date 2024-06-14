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
using DocumentFormat.OpenXml.Spreadsheet;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;


namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ReportSubjectViewModel : ViewModelBase
    {
        private ObservableCollection<BaoCaoMon> listBaoCaoMon;

        public ObservableCollection<BaoCaoMon> ListBaoCaoMon
        {
            get => listBaoCaoMon;
            set => this.RaiseAndSetIfChanged(ref listBaoCaoMon, value);
        }

        private ObservableCollection<BaoCaoMon> allBaoCaoMon;

        public ObservableCollection<BaoCaoMon> AllBaoCaoMon
        {
            get => allBaoCaoMon;
            set => this.RaiseAndSetIfChanged(ref allBaoCaoMon, value);
        }

        private ObservableCollection<Lop> lops;

        public ObservableCollection<Lop> Lops
        {
            get => lops;
            set => this.RaiseAndSetIfChanged(ref lops, value);
        }

        private ObservableCollection<ISeries> series;

        public ObservableCollection<ISeries> Series
        {
            get => series;
            set => this.RaiseAndSetIfChanged(ref series, value);
        }

        private List<Axis> xaxes;

        public List<Axis> XAxes
        {
            get => xaxes;
            set => this.RaiseAndSetIfChanged(ref xaxes, value);
        }

        private int selectedBaoCaoMonIndex;

        public int SelectedBaoCaoMonIndex
        {
            get => selectedBaoCaoMonIndex;
            set => this.RaiseAndSetIfChanged(ref selectedBaoCaoMonIndex, value);
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

        private ObservableCollection<string> monHocsCb;

        public ObservableCollection<string> MonHocsCb
        {
            get => monHocsCb;
            set => this.RaiseAndSetIfChanged(ref monHocsCb, value);
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

        private ObservableCollection<HocKy> hocKies;

        public ObservableCollection<HocKy> HocKies
        {
            get => hocKies;
            set => this.RaiseAndSetIfChanged(ref hocKies, value);
        }

        private ObservableCollection<MonHoc> monHocs;

        public ObservableCollection<MonHoc> MonHocs
        {
            get => monHocs;
            set => this.RaiseAndSetIfChanged(ref monHocs, value);
        }

        private bool isUpdating = false;

        public ReportSubjectViewModel()
        {
            LoadBaoCaoMon();
            LoadChart();
            LoadFilter();
            //var result = DataProvider.Ins.DB.BaoCaoMons.ToList();
            //listBaoCaoMon = new ObservableCollection<BaoCaoMon>(result);
            LoadListComboBox();
        }

        private void LoadListComboBox()
        {
            NienKhoasCb = new ObservableCollection<string>();
            foreach (var nk in nienKhoas)
            {
                nienKhoasCb.Add(nk.TenNienKhoa);
            }
            MonHocsCb = new ObservableCollection<string>();
            foreach (var mh in monHocs)
            {
                monHocsCb.Add(mh.TenMon);
            }
            HocKiesCb = new ObservableCollection<string>();
            foreach (var hk in hocKies)
            {
                hocKiesCb.Add(hk.TenHocKy);
            }

        }

        private void UpdateReportSearch()
        {
            isUpdating = true;
            string monHoc = selectedMonHocIndex != -1 ? MonHocsCb[selectedMonHocIndex] : null;
            string nienKhoa = selectedNienKhoaIndex != -1 ? NienKhoasCb[selectedNienKhoaIndex] : null;
            string hocKy = selectedHocKyIndex != -1 ? hocKiesCb[selectedHocKyIndex] : null;
            SearchAndUpdateReport(monHoc, nienKhoa, hocKy);
            isUpdating = false;
        }

        public void SearchAndUpdateReport(string monhoc = null, string nienKhoa = null, string hocKy = null)
        {
            var query = AllBaoCaoMon.AsQueryable();
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

            if (!string.IsNullOrEmpty(hocKy))
            {
                var maHocKy = HocKies
                    .Where(hk => hk.TenHocKy == hocKy)
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
            
            ListBaoCaoMon.Clear();
            foreach (var bkm in query)
            {
                ListBaoCaoMon.Add(bkm);
            }
            LoadChart();
        }

        private void LoadBaoCaoMon()
        {
            var result = DataProvider.Ins.DB.BaoCaoMons.Include("MaLopNavigation").ToList();
            if (ListBaoCaoMon == null)
            {
                ListBaoCaoMon = new ObservableCollection<BaoCaoMon>(result);
                AllBaoCaoMon = new ObservableCollection<BaoCaoMon>(result);
            }
            else
            {
                ListBaoCaoMon.Clear();
                AllBaoCaoMon.Clear();
                foreach (var item in result)
                {
                    ListBaoCaoMon.Add(item);
                    AllBaoCaoMon.Add(item);
                }
            }

        }

        private void LoadFilter()
        {
            var result1 = DataProvider.Ins.DB.NienKhoas.AsNoTracking().ToList();
            NienKhoas = new ObservableCollection<NienKhoa>(result1);
            var result2 = DataProvider.Ins.DB.MonHocs.AsNoTracking().ToList();
            MonHocs = new ObservableCollection<MonHoc>(result2);
            var result3 = DataProvider.Ins.DB.HocKies.AsNoTracking().ToList();
            HocKies = new ObservableCollection<HocKy>(result3);
            var result4 = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            Lops = new ObservableCollection<Lop>(result4);
        }

        private void LoadChart()
        {
            Series = new ObservableCollection<ISeries>
             {
                    new ColumnSeries<double>
                    {
                        Values = listBaoCaoMon.Select(d => d.TiLe ?? 0).ToArray()

                    }
             };

            //XAxes = new List<Axis>
            //{
            //    new Axis
            //    {
            //        Labels = listBaoCaoMon.Select(d => d.TenLop).ToArray(),
            //        TextSize = 13
            //    }
            //};
        }

    }
}
