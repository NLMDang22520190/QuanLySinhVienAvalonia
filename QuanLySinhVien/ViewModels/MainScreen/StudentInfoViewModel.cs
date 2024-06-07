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
    public class StudentInfoViewModel : ViewModelBase
    {
        #region Time

        private string _currentTime;

        public string CurrentTime
        {
            get => _currentTime;
            set => this.RaiseAndSetIfChanged(ref _currentTime, value);
        }

        private async void UpdateCurrentTime()
        {
            while (true)
            {
                CurrentTime = DateTime.Now.ToString("HH:mm dd/MM/yy", CultureInfo.InvariantCulture);
                await Task.Delay(1000);
            }
        }

        #endregion Time

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
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHocSinhIndex, value);
            }
        }

        private string searchName;

        public string SearchName
        {
            get => searchName;
            set => this.RaiseAndSetIfChanged(ref searchName, value);
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
            set => this.RaiseAndSetIfChanged(ref lopsCb, value);
        }

        public ReactiveCommand<Window, Unit> OpenEditStudentWindowCommand { get; }


        #endregion

        public StudentInfoViewModel()
        {
            LoadListHocSinh();
            LoadListComboBox();
            UpdateCurrentTime();
            OpenEditStudentWindowCommand = ReactiveCommand.Create<Window>(OpenEditStudentWindow);
        }

        public void ShowAllStudent()
        {
            SearchName = string.Empty;
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
                    }
                    addStudentWindow.Close();
                });
        }

        public void DeleteSelectedStudent()
        {
            if (SelectedHocSinhIndex == -1)
            {
                return;
            }

            var selectedHocSinh = ListHocSinhs[SelectedHocSinhIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.HocSinhs.Local.SingleOrDefault(hs => hs.MaHocSinh == selectedHocSinh.MaHocSinh);
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            DataProvider.Ins.DB.HocSinhs.Remove(selectedHocSinh);
            DataProvider.Ins.DB.SaveChanges();
            LoadListHocSinh();
        }

        public void OpenEditStudentWindow(Window window)
        {
            if (SelectedHocSinhIndex == -1)
            {
                return;
            }

            var selectedHocSinh = ListHocSinhs[SelectedHocSinhIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.HocSinhs.Local.SingleOrDefault(hs => hs.MaHocSinh == selectedHocSinh.MaHocSinh);
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
                    }
                    editStudentWindow.Close();
                });
        }

        private void LoadListHocSinh()
        {
            var result = DataProvider.Ins.DB.HocSinhs.AsNoTracking().ToList();
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
            var result2 = DataProvider.Ins.DB.NienKhoas.Select(nk => nk.TenNienKhoa).ToList();
            NienKhoasCb = new ObservableCollection<string>(result2);
            var result3 = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result3);
            var result4 = DataProvider.Ins.DB.Lops.Select(l => l.TenLop).ToList();
            LopsCb = new ObservableCollection<string>(result4);
        }

        #endregion
    }
}
