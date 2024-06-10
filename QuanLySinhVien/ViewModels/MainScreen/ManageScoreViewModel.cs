using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using ReactiveUI;
using Microsoft.EntityFrameworkCore; // Ensure you include this namespace for async operations

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ManageScoreViewModel : ViewModelBase
    {
        #region Properties

        private ObservableCollection<HeThongDiem> heThongDiems;

        public ObservableCollection<HeThongDiem> HeThongDiems
        {
            get => heThongDiems;
            set => this.RaiseAndSetIfChanged(ref heThongDiems, value);
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
                        //UpdateStudentSearch();
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
                        //UpdateStudentSearch();
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
                        //UpdateStudentSearch();
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
                        //UpdateStudentSearch();
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
                        //UpdateStudentSearch();
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


        public ManageScoreViewModel()
        {
            LoadHeThongDiem();
            LoadFilter();
            LoadListComboBox();
        }



        #region DB Commands

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
            HeThongDiems = new ObservableCollection<HeThongDiem>(result);
        }
    }
    #endregion

}
