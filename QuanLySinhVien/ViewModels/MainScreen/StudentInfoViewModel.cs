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
using MsBox.Avalonia;
using Avalonia.Controls;
using System.Reactive.Linq;
using DocumentFormat.OpenXml.VariantTypes;
using MsBox.Avalonia.Enums;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class StudentInfoViewModel : ViewModelBase
    {

        #region Properties

        private ObservableCollection<HocSinh> listHocSinhs;

        public ObservableCollection<HocSinh> ListHocSinhs
        {
            get => listHocSinhs;
            set => this.RaiseAndSetIfChanged(ref listHocSinhs, value);
        }

        private ObservableCollection<HocSinh> allHocSinhs;

        public ObservableCollection<HocSinh> AllHocSinhs
        {
            get => allHocSinhs;
            set => this.RaiseAndSetIfChanged(ref allHocSinhs, value);
        }

        private int selectedHocSinhIndex;

        public int SelectedHocSinhIndex
        {
            get => selectedHocSinhIndex;
            set { this.RaiseAndSetIfChanged(ref selectedHocSinhIndex, value); }
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
                        UpdateStudentSearch();
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
                        UpdateStudentSearch();
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
                        UpdateStudentSearch();
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

        private bool isUpdating = false;

        public ReactiveCommand<Window, Unit> OpenEditStudentWindowCommand { get; }

        public ReactiveCommand<Window, Unit> DeleteSelectedStudentCommand { get; }


        #endregion

        public StudentInfoViewModel()
        {
            LoadListHocSinh();
            LoadFilter();
            LoadListComboBox();
            OpenEditStudentWindowCommand = ReactiveCommand.Create<Window>(OpenEditStudentWindow);
            DeleteSelectedStudentCommand = ReactiveCommand.Create<Window>(DeleteSelectedStudent);
        }

        public void ShowAllStudent()
        {
            SearchName = string.Empty;
            SelectedNienKhoaIndex = -1;
            SelectedKhoiIndex = -1;
            SelectedLopIndex = -1;
        }
        private void UpdateStudentSearch()
        {
            isUpdating = true;
            string khoi = selectedKhoiIndex != -1 ? khoisCb[selectedKhoiIndex] : null;
            string nienKhoa = selectedNienKhoaIndex != -1 ? NienKhoasCb[selectedNienKhoaIndex] : null;
            string lop = selectedLopIndex != -1 ? lopsCb[selectedLopIndex] : null;
            SearchAndUpdateStudents(khoi, nienKhoa, lop);
            isUpdating = false;
        }
        public void SearchAndUpdateStudents(string khoi = null, string nienKhoa = null, string lop = null)
        {
            var query = AllHocSinhs.AsQueryable();
            Debug.WriteLine("SearchAndUpdateStudents");
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


            ListHocSinhs.Clear();
            foreach (var student in query.ToList())
            {
                ListHocSinhs.Add(student);
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
        public void SearchStudent(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
            {
                LoadListHocSinhFromMemory();
            }
            else
            {
                // Filter the list based on searchName
                var filteredList = AllHocSinhs
                    .Where(hs => hs.TenHocSinh.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ListHocSinhs.Clear();
                foreach (var hs in filteredList)
                {
                    ListHocSinhs.Add(hs);
                }
            }
        }


        #region DB Commands

        public void OpenAddStudentWindow(Window window)
        {
            var addStudentWindow = new AddStudentView();
            var addStudentViewModel = new AddStudentViewModel();
            addStudentWindow.DataContext = addStudentViewModel;

            addStudentWindow.Title = "Thêm Học Sinh";
            addStudentWindow.ShowDialog(window);

            addStudentViewModel.AddCommand
                .Take(1)
                .Subscribe(newHocSinh =>
                {
                    if (newHocSinh != null)
                    {
                        DataProvider.Ins.DB.HocSinhs.Add(newHocSinh);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListHocSinh();
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Thêm học sinh thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                    }

                    addStudentWindow.Close();
                });
        }

        public async void DeleteSelectedStudent(Window window)
        {
            if (SelectedHocSinhIndex == -1)
            {
                return;
            }

            Debug.WriteLine("DeleteSelectedStudent");

            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận xóa", "Bạn có chắc chắn muốn xóa học sinh này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                var selectedHocSinh = ListHocSinhs[SelectedHocSinhIndex];
                // Ensure this entity is detached before attaching a new instance
                var existingEntity =
                    DataProvider.Ins.DB.HocSinhs.Local.SingleOrDefault(hs => hs.MaHocSinh == selectedHocSinh.MaHocSinh);
                if (existingEntity != null)
                {
                    DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
                }

                DataProvider.Ins.DB.HocSinhs.Remove(selectedHocSinh);
                DataProvider.Ins.DB.SaveChanges();
                LoadListHocSinh();
                MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa học sinh thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
            }

        }

        public void OpenEditStudentWindow(Window window)
        {
            if (SelectedHocSinhIndex == -1)
            {
                return;
            }

            var selectedHocSinh = ListHocSinhs[SelectedHocSinhIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity =
                DataProvider.Ins.DB.HocSinhs.Local.SingleOrDefault(hs => hs.MaHocSinh == selectedHocSinh.MaHocSinh);
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            var editStudentWindow = new EditStudentView();
            var editStudentViewModel = new EditStudentViewModel(selectedHocSinh);
            editStudentWindow.DataContext = editStudentViewModel;

            editStudentWindow.Title = "Sửa thông tin học sinh";
            editStudentWindow.ShowDialog(window);

            editStudentViewModel.EditCommand
                .Take(1)
                .Subscribe(hs =>
                {
                    if (hs != null)
                    {
                        DataProvider.Ins.DB.HocSinhs.Update(hs);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListHocSinh();
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Sửa thông tin học sinh thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                    }

                    editStudentWindow.Close();
                });
        }

        private void LoadListHocSinh()
        {
            var result = DataProvider.Ins.DB.HocSinhs.AsNoTracking().Include("HeThongDiems").ToList();
            if (ListHocSinhs == null)
            {
                ListHocSinhs = new ObservableCollection<HocSinh>(result);
                AllHocSinhs = new ObservableCollection<HocSinh>(result);
            }
            else
            {
                ListHocSinhs.Clear();
                AllHocSinhs.Clear();
                foreach (var hs in result)
                {
                    ListHocSinhs.Add(hs);
                    AllHocSinhs.Add(hs);
                }
            }
        }

        private void LoadListHocSinhFromMemory()
        {
            ListHocSinhs.Clear();
            foreach (var hs in AllHocSinhs)
            {
                ListHocSinhs.Add(hs);
            }
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
        }

        private void LoadFilter()
        {
            var result1 = DataProvider.Ins.DB.NienKhoas.AsNoTracking().ToList();
            NienKhoas = new ObservableCollection<NienKhoa>(result1);
            var result2 = DataProvider.Ins.DB.Khois.AsNoTracking().ToList();
            Khois = new ObservableCollection<Khoi>(result2);
            var result3 = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            Lops = new ObservableCollection<Lop>(result3);
        }

        #endregion
    }
}
