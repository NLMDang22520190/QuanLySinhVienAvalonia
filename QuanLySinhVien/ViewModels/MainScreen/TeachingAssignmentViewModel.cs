using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class TeachingAssignmentViewModel : ViewModelBase
    {
        private ObservableCollection<PhanCongGiangDay> _danhSachPhanCong;
        public ObservableCollection<PhanCongGiangDay> DanhSachPhanCong
        {
            get => _danhSachPhanCong;
            set => this.RaiseAndSetIfChanged(ref _danhSachPhanCong, value);
        }

        private ObservableCollection<PhanCongGiangDay> allPhanCongs;

        public ObservableCollection<PhanCongGiangDay> AllPhanCongs
        {
            get => allPhanCongs;
            set => this.RaiseAndSetIfChanged(ref allPhanCongs, value);
        }

        private int selectedMonHocIndex;

        public int SelectedMonHocIndex
        {
            get => selectedMonHocIndex;
            set { this.RaiseAndSetIfChanged(ref selectedMonHocIndex, value); }
        }

        private int selectedPhanCongIndex;

        public int SelectedPhanCongIndex
        {
            get => selectedPhanCongIndex;
            set { this.RaiseAndSetIfChanged(ref selectedPhanCongIndex, value); }
        }

        private string searchName;

        public string SearchName
        {
            get => searchName;
            set => this.RaiseAndSetIfChanged(ref searchName, value);
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
                        UpdateAssignmentSearch();
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
                        UpdateAssignmentSearch();
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
                        UpdateAssignmentSearch();
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

        private ObservableCollection<string> hocKysCb;

        public ObservableCollection<string> HocKysCb
        {
            get => hocKysCb;
            set => this.RaiseAndSetIfChanged(ref hocKysCb, value);
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

        private ObservableCollection<NienKhoa> nienKhoas;

        public ObservableCollection<NienKhoa> NienKhoas
        {
            get => nienKhoas;
            set => this.RaiseAndSetIfChanged(ref nienKhoas, value);
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

        private ObservableCollection<Lop> lops;

        public ObservableCollection<Lop> Lops
        {
            get => lops;
            set => this.RaiseAndSetIfChanged(ref lops, value);
        }
        private bool isUpdating = false;

        public ReactiveCommand<Window, Unit> OpenEditAssignmentWindowCommand { get; }

        public ReactiveCommand<Window, Unit> DeleteSelectedAssignmentCommand { get; }

        public TeachingAssignmentViewModel()
        {
            LoadDanhSachPhanCong();
            LoadFilter();
            LoadListComboBox();
            DeleteSelectedAssignmentCommand = ReactiveCommand.Create<Window>(DeleteSelectedAssignment);
        }

        public void ShowAllS()
        {
            SearchName = string.Empty;
            SelectedNienKhoaIndex = -1;
            SelectedHocKyIndex = -1;
            SelectedLopIndex = -1;
        }

        private void UpdateAssignmentSearch()
        {
            isUpdating = true;
            string khoi = selectedHocKyIndex != -1 ? hocKysCb[selectedHocKyIndex] : null;
            string nienKhoa = selectedNienKhoaIndex != -1 ? NienKhoasCb[selectedNienKhoaIndex] : null;
            string lop = selectedLopIndex != -1 ? lopsCb[selectedLopIndex] : null;
            SearchAndUpdateAssignments(khoi, nienKhoa, lop);
            isUpdating = false;
        }
        public void SearchAndUpdateAssignments(string hocky = null, string nienKhoa = null, string lop = null)
        {
            var query = AllPhanCongs.AsQueryable();

            Debug.WriteLine("SearchAndUpdateAssignments");

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

                    query = query.Where(pc => danhSachMaLop.Contains(pc.MaLop));
                }
            }

            if (!string.IsNullOrEmpty(hocky))
            {
                var maHocKy = HocKys
                    .Where(hk => hk.TenHocKy == hocky)
                    .Select(hk => hk.MaHocKy)
                    .FirstOrDefault();

                query = query.Where(pc => pc.MaHocKy == maHocKy);
            }

            if (!string.IsNullOrEmpty(lop))
            {
                var maLop = Lops
                    .Where(l => l.TenLop == lop)
                    .Select(l => l.MaLop)
                    .FirstOrDefault();

                query = query.Where(pc => pc.MaLop == maLop);
            }

            DanhSachPhanCong.Clear();
            foreach (var phanCong in query.ToList())
            {
                DanhSachPhanCong.Add(phanCong);
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

        private void LoadDanhSachPhanCong()
        {
            var result = DataProvider.Ins.DB.PhanCongGiangDays.AsNoTracking()
                .Include("MaLopNavigation")
                .Include("MaMonNavigation").ToList();
            if (DanhSachPhanCong == null)
            {
                DanhSachPhanCong = new ObservableCollection<PhanCongGiangDay>(result);
                AllPhanCongs = new ObservableCollection<PhanCongGiangDay>(result);
            }
            else
            {
                DanhSachPhanCong.Clear();
                AllPhanCongs.Clear();
                foreach (var pc in result)
                {
                    DanhSachPhanCong.Add(pc);
                    AllPhanCongs.Add(pc);
                }
            }
        }

        private void LoadListPhanCongFromMemory()
        {
            DanhSachPhanCong.Clear();
            foreach (var pc in AllPhanCongs)
            {
                DanhSachPhanCong.Add(pc);
            }
        }

        public void SearchSubject(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
            {
                LoadListPhanCongFromMemory();
            }
            else
            {
                // Filter the list based on searchName
                var filteredList = AllPhanCongs
                    .Where(pc => pc.TenMon.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                DanhSachPhanCong.Clear();
                foreach (var pc in filteredList)
                {
                    DanhSachPhanCong.Add(pc);
                }
            }
        }

        public void OpenAddAssignmentWindow(Window window)
        {
            var addAssignmentWindow = new AddAssignmentView();
            var addAssignmentViewModel = new AddAssignmentViewModel();
            addAssignmentWindow.DataContext = addAssignmentViewModel;

            addAssignmentWindow.Title = "Thêm Phân Công";
            addAssignmentWindow.ShowDialog(window);

            addAssignmentViewModel.AddCommand
                .Take(1)
                .Subscribe(newPhanCong =>
                {
                    if (newPhanCong != null)
                    {
                        DataProvider.Ins.DB.PhanCongGiangDays.Add(newPhanCong);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadDanhSachPhanCong();
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Thêm phân công thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                    }

                    addAssignmentWindow.Close();
                });
        }
        public async void DeleteSelectedAssignment(Window window)
        {
            if (SelectedPhanCongIndex == -1)
            {
                return;
            }

            Debug.WriteLine("DeleteSelectedAssignment");

            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận xóa", "Bạn có chắc chắn muốn xóa phân công này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                var selectedPhanCong = DanhSachPhanCong[SelectedPhanCongIndex];
                // Ensure this entity is detached before attaching a new instance
                var existingEntity =
                    DataProvider.Ins.DB.PhanCongGiangDays.Local.SingleOrDefault(pc => pc.MaPhanCong == selectedPhanCong.MaPhanCong);
                if (existingEntity != null)
                {
                    DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
                }

                DataProvider.Ins.DB.PhanCongGiangDays.Remove(selectedPhanCong);
                DataProvider.Ins.DB.SaveChanges();
                LoadDanhSachPhanCong();
                MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa phân công thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
            }

        }
        private void LoadListComboBox()
        {
            NienKhoasCb = new ObservableCollection<string>();
            foreach (var nk in nienKhoas)
            {
                nienKhoasCb.Add(nk.TenNienKhoa);
            }
            HocKysCb = new ObservableCollection<string>();
            foreach (var hk in hocKys)
            {
                hocKysCb.Add(hk.TenHocKy);
            }
            LopsCb = new ObservableCollection<string>();
            foreach (var l in lops)
            {
                lopsCb.Add(l.TenLop);
            }
        }

        private void LoadFilter()
        {
            var result1 = DataProvider.Ins.DB.NienKhoas.AsNoTracking().ToList();
            NienKhoas = new ObservableCollection<NienKhoa>(result1);
            var result2 = DataProvider.Ins.DB.HocKies.AsNoTracking().ToList();
            HocKys = new ObservableCollection<HocKy>(result2);
            var result3 = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            Lops = new ObservableCollection<Lop>(result3);
        }
    }
}
