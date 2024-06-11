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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;


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
            }
        }


        public ReactiveCommand<Window, Unit> OpenEditTeacherWindowCommand { get; }

        public ReactiveCommand<Window, Unit> DeleteSelectedTeacherCommand { get; }


        #endregion

        public TeacherInfoViewModel()
        {
            LoadListGiaoVien();

            UpdateCurrentTime();

            OpenEditTeacherWindowCommand = ReactiveCommand.Create<Window>(OpenEditTeacherWindow);
            DeleteSelectedTeacherCommand = ReactiveCommand.Create<Window>(DeleteSelectedTeacher);

        }

        public void SearchTeacher(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
            {
                LoadListGiaoVienFromMemory();
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
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Thêm giáo viên thành công !", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                    }
                    addTeacherWindow.Close();
                });
        }

        public async void DeleteSelectedTeacher(Window window)
        {
            if (SelectedGiaoVienIndex == -1)
            {
                return;
            }

            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn xóa giáo viên này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                var selectedGiaoVien = ListGiaoViens[SelectedGiaoVienIndex];
                // Ensure this entity is detached before attaching a new instance
                var existingEntity = DataProvider.Ins.DB.GiaoViens.Local.SingleOrDefault(gv => gv.MaGiaoVien == selectedGiaoVien.MaGiaoVien);
                if (existingEntity != null)
                {
                    DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
                }

                DataProvider.Ins.DB.GiaoViens.Remove(selectedGiaoVien);
                DataProvider.Ins.DB.SaveChanges();
                LoadListGiaoVien();
                MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa giáo viên thành công !", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
            }

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
                        MessageBoxManager.GetMessageBoxStandard("Thông báo", "Sửa thông tin giáo viên thành công !", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
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

        private void LoadListGiaoVienFromMemory()
        {
            ListGiaoViens.Clear();
            foreach (var gv in AllGiaoViens)
            {
                ListGiaoViens.Add(gv);
            }
        }


        #endregion


    }
}

