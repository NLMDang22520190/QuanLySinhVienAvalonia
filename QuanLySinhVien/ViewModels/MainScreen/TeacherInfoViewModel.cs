using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class TeacherInfoViewModel : ViewModelBase
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

        private ObservableCollection<GiaoVien> listGiaoViens;

        public ObservableCollection<GiaoVien> ListGiaoViens
        {
            get => listGiaoViens;
            set => this.RaiseAndSetIfChanged(ref listGiaoViens, value);
        }

        private ObservableCollection<GiaoVien> allGiaoViens;

        public ObservableCollection<GiaoVien> AllGiaoViens
        {
            get => allGiaoViens;
            set => this.RaiseAndSetIfChanged(ref allGiaoViens, value);
        }


        private int selectedGiaoVienIndex;

        public int SelectedGiaoVienIndex
        {
            get => selectedGiaoVienIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedGiaoVienIndex, value);
                Debug.WriteLine(selectedGiaoVienIndex);
            }
        }


        public ReactiveCommand<Window, Unit> OpenEditTeacherWindowCommand { get; }


        #endregion

        public TeacherInfoViewModel()
        {
            LoadListGiaoVien();

            UpdateCurrentTime();

            OpenEditTeacherWindowCommand = ReactiveCommand.Create<Window>(OpenEditTeacherWindow);

        }

        public void SearchTeacher(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
            {
                // If searchName is null or whitespace, show all teachers
                ListGiaoViens.Clear();
                foreach (var gv in AllGiaoViens)
                {
                    ListGiaoViens.Add(gv);
                }
            }
            else
            {
                // Filter the list based on searchName
                var filteredList = AllGiaoViens
                    .Where(gv => gv.TenGiaoVien.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ListGiaoViens.Clear();
                foreach (var gv in filteredList)
                {
                    ListGiaoViens.Add(gv);
                }
            }
        }


        #region DB Commands

        public void OpenAddTeacherWindow(Window window)
        {
            var addTeacherWindow = new AddTeacherView();
            var addTeacherViewModel = new AddTeacherViewModel();
            addTeacherWindow.DataContext = addTeacherViewModel;

            addTeacherWindow.Title = "Thêm giáo viên";
            addTeacherWindow.ShowDialog(window);

            addTeacherViewModel.AddCommand
                .Take(1)
                .Subscribe(newGiaoVien =>
                {
                    if (newGiaoVien != null)
                    {
                        DataProvider.Ins.DB.GiaoViens.Add(newGiaoVien);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListGiaoVien();
                    }
                    addTeacherWindow.Close();
                });
        }

        public void DeleteSelectedTeacher()
        {
            if (SelectedGiaoVienIndex == -1)
            {
                return;
            }

            var selectedGiaoVien = ListGiaoViens[SelectedGiaoVienIndex];
            DataProvider.Ins.DB.GiaoViens.Remove(selectedGiaoVien);
            DataProvider.Ins.DB.SaveChanges();
            LoadListGiaoVien();

        }

        public void OpenEditTeacherWindow(Window window)
        {
            if (SelectedGiaoVienIndex == -1)
            {
                return;
            }

            var selectedGiaoVien = ListGiaoViens[SelectedGiaoVienIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.GiaoViens.Local.SingleOrDefault(gv => gv.MaGiaoVien == selectedGiaoVien.MaGiaoVien);
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            var editTeacherWindow = new EditTeacherView();
            var editTeacherViewModel = new EditTeacherViewModel(selectedGiaoVien);
            editTeacherWindow.DataContext = editTeacherViewModel;

            editTeacherWindow.Title = "Sửa thông tin giáo viên";
            editTeacherWindow.ShowDialog(window);

            editTeacherViewModel.EditCommand
                .Take(1)
                .Subscribe(gv =>
                {
                    if (gv != null)
                    {
                        DataProvider.Ins.DB.GiaoViens.Update(gv);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListGiaoVien();
                    }
                    editTeacherWindow.Close();
                });
        }

        private void LoadListGiaoVien()
        {
            var result = DataProvider.Ins.DB.GiaoViens.AsNoTracking().ToList();
            if (ListGiaoViens == null)
            {
                ListGiaoViens = new ObservableCollection<GiaoVien>(result);
                AllGiaoViens = new ObservableCollection<GiaoVien>(result);
            }
            else
            {
                ListGiaoViens.Clear();
                AllGiaoViens.Clear();
                foreach (var gv in result)
                {
                    ListGiaoViens.Add(gv);
                    AllGiaoViens.Add(gv);
                }
            }
        }


        #endregion


    }
}

