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
using System.Windows;
using Avalonia.Controls.ApplicationLifetimes;
using DocumentFormat.OpenXml.VariantTypes;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using QuanLySinhVien.Views;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ClassViewModel: ViewModelBase
    {
        private static bool isAdmin = false;
        public static bool IsAdmin
        {
            get => isAdmin;
            set => isAdmin = value;
        }

        private ObservableCollection<Lop> listLops;
        public ObservableCollection<Lop> ListLops
        {
            get => listLops; 
            set => this.RaiseAndSetIfChanged(ref listLops, value);
        }

        private ObservableCollection<Lop> allLops;

        public ObservableCollection<Lop> AllLops
        {
            get => allLops;
            set => this.RaiseAndSetIfChanged(ref allLops, value);
        }

        private int selectedClassIndex;

        public int SelectedClassIndex
        {
            get => selectedClassIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedClassIndex, value);
            }
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
                        UpdateClassSearch();
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
                        UpdateClassSearch();
                    }
                }
            }
        }
        public ReactiveCommand<Window, Unit> OpenEditClassWindowCommand { get; }
        public ReactiveCommand<Lop, Unit> OpenListClassWindowCommand { get; }

        private ReactiveCommand<Unit, Unit> addClassCommand;
        public ReactiveCommand<Unit, Unit> AddClassCommand
        {
            get => addClassCommand;
            set => this.RaiseAndSetIfChanged(ref addClassCommand, value);
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

        public ReactiveCommand<Window, Unit> DeleteSelectedClassCommand { get; }

        private bool isUpdating = false;

        private string searchName;

        public string SearchName
        {
            get => searchName;
            set => this.RaiseAndSetIfChanged(ref searchName, value);

        }

        public ClassViewModel()
        {
            LoadListLop();
            LoadFilter();
            ListLops = new ObservableCollection<Lop>();
            LoadClasses();
            OpenEditClassWindowCommand = ReactiveCommand.Create<Window>(OpenEditClassWindow);
            OpenListClassWindowCommand = ReactiveCommand.Create<Lop>(OpenListClassWindow);
            DeleteSelectedClassCommand = ReactiveCommand.Create<Window>(DeleteSelectedClass);
            var result1 = DataProvider.Ins.DB.Lops.ToList();
            listLops = new ObservableCollection<Lop>(result1);
            var result2 = DataProvider.Ins.DB.NienKhoas.Select(nk => nk.TenNienKhoa).ToList();
            NienKhoasCb = new ObservableCollection<string>(result2);
            var result3 = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result3);
        }
        public void OpenAddClassWindow(Window window) {

            //AddClassView addClassView = new AddClassView();
            //addClassView.DataContext = new AddClassViewModel();
            //addClassView.Show();

            var addClassWindow = new AddClassView();
            var addClassViewModel = new AddClassViewModel();
            addClassWindow.DataContext = addClassViewModel;

            addClassWindow.Title = "Thêm lớp";
            addClassWindow.ShowDialog(window);

            addClassViewModel.AddCommand
                .Take(1)
                .Subscribe(newClass =>
                {
                    if (newClass != null)
                    {
                        DataProvider.Ins.DB.Lops.Add(newClass);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListLop();                      
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Thêm lớp thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                    }
                    addClassWindow.Close();
                });
        }

        public void OpenListClassWindow(/*Window window*/ Lop selectedLop)
        {
            //ListClassView listClassView = new ListClassView();
            //listClassView.DataContext = new ListClassViewModel();
            //listClassView.Show(); 
            //if (selectedLop == null) return;
            var listClassWindow = new ListClassView();
            var listClassViewModel = new ListClassViewModel(selectedLop);
            listClassWindow.DataContext = listClassViewModel;

            listClassWindow.Title = "Danh sách học sinh";
            listClassWindow.Show();
        }

        public async void DeleteSelectedClass(Window window)
        {
            if (SelectedClassIndex == -1)
            {
                return;
            }

            Debug.WriteLine("DeleteSelectedClass");

            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận xóa", "Bạn có chắc chắn muốn xóa lớp này không?", ButtonEnum.YesNo, Icon.Question);
            var result = await box.ShowAsync();

            try
            {
                if (result == ButtonResult.Yes)
                {
                    var selectedLop = ListLops[SelectedClassIndex];
                    var existingEntity = DataProvider.Ins.DB.Lops.Local.SingleOrDefault(l => l.MaLop == selectedLop.MaLop);
                    if (existingEntity != null)
                    {
                        DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
                    }

                    if (DataProvider.Ins.DB.HocSinhs.Any(hs => hs.MaLop == selectedLop.MaLop))
                    {
                        await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không thể xóa lớp này vì vân còn học sinh. Hãy thử xóa hết các học sinh trong lớp", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
                        return;
                    }
                    DataProvider.Ins.DB.Lops.Remove(selectedLop);
                    DataProvider.Ins.DB.SaveChanges();
                    RefreshDbContext();
                    LoadListLop();
                    MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa lớp thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                }
            }
            catch (DbUpdateException ex)
            {
                // Kiểm tra ngoại lệ liên quan đến ràng buộc khóa ngoại
                if (ex.InnerException?.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint") == true)
                {
                    // 547 là mã lỗi của SQL Server cho vi phạm ràng buộc khóa ngoại
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", "Không thể xóa lớp này vì nó đang được sử dụng bởi các bảng khác. Hãy thử xóa ở các bảng khác trước", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
                }
                else
                {
                    // Xử lý các lỗi khác
                    await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Xảy ra lỗi khi xóa lớp: {ex.Message}", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác không phải DbUpdateException
                await MessageBoxManager.GetMessageBoxStandard("Lỗi", $"Xảy ra lỗi khi xóa lớp: {ex.Message}", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
            }

        }


        private void RefreshDbContext()
        {
            DataProvider.Ins.DB = new QlhsContext(); 
        }


        private void UpdateClassSearch()
        {
            isUpdating = true;
            string khoi = selectedKhoiIndex != -1 ? khoisCb[selectedKhoiIndex] : null;
            string nienKhoa = selectedNienKhoaIndex != -1 ? NienKhoasCb[selectedNienKhoaIndex] : null;
            SearchAndUpdateClass(khoi, nienKhoa);
            isUpdating = false;
        }
        public void SearchAndUpdateClass(string khoi = null, string nienKhoa = null)
        {
            var query = AllLops.AsQueryable();
            Debug.WriteLine("SearchAndUpdateClass");
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

            ListLops.Clear();
            foreach (var student in query.ToList())
            {
                ListLops.Add(student);
            }
        }

        public void SearchClass(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
            {
                LoadListClassFromMemory();
            }
            else
            {
                // Filter the list based on searchName
                var filteredList = AllLops
                    .Where(l => l.TenLop.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ListLops.Clear();
                foreach (var l in filteredList)
                {
                    ListLops.Add(l);
                }
            }
        }

        public void LoadClasses()
        {
            // Lấy danh sách các lớp từ cơ sở dữ liệu
            var classes = DataProvider.Ins.DB.Lops.Include(l => l.HocSinhs).ToList();

            foreach (var lop in classes)
            {
                // Cập nhật lại sĩ số dựa trên số học sinh trong lớp
                lop.SiSo = lop.HocSinhs.Count;
            }

            ListLops = new ObservableCollection<Lop>(classes);
        }

        public void OpenEditClassWindow(Window window)
        {
            if (SelectedClassIndex == -1)
            {
                return;
            }

            var selectedLop = ListLops[SelectedClassIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.Lops.Local.SingleOrDefault(l => l.MaLop == selectedLop.MaLop);
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            var editClassWindow = new EditClassView();
            var editClassViewModel = new EditClassViewModel(selectedLop);
            editClassWindow.DataContext = editClassViewModel;

            editClassWindow.Title = "Sửa thông tin lớp học";
            editClassWindow.ShowDialog(window);

            editClassViewModel.EditCommand
                .Take(1)
                .Subscribe(l =>
                {
                    if (l != null)
                    {
                        DataProvider.Ins.DB.Lops.Update(l);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListLop();
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Sửa thông tin lớp thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                    }
                    editClassWindow.Close();
                });
        }

        private void LoadListLop()
        {
            var result = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            if (ListLops == null)
            {
                ListLops = new ObservableCollection<Lop>(result);
                AllLops = new ObservableCollection<Lop>(result);
            }
            else
            {
                ListLops.Clear();
                AllLops.Clear();
                foreach (var l in result)
                {
                    ListLops.Add(l);
                    AllLops.Add(l);
                }
            }
        }



        private void LoadListClassFromMemory()
        {
            ListLops.Clear();
            foreach (var l in AllLops)
            {
                ListLops.Add(l);
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
    }
}
