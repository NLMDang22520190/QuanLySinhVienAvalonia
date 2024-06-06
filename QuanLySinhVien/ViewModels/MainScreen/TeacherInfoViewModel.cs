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

        private ObservableCollection<GiaoVien> listGiaoViens;

        public ObservableCollection<GiaoVien> ListGiaoViens
        {
            get => listGiaoViens;
            set => this.RaiseAndSetIfChanged(ref listGiaoViens, value);
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

        public TeacherInfoViewModel()
        {
            LoadListGiaoVien();

            UpdateCurrentTime();

        }

        public void OpenAddTeacherWindow(Window window)
        {
            var addTeacherWindow = new AddTeacherView();
            var addTeacherViewModel = new AddTeacherViewModel();
            addTeacherWindow.DataContext = addTeacherViewModel;

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
        private void LoadListGiaoVien()
        {
            var result = DataProvider.Ins.DB.GiaoViens.ToList();
            if (ListGiaoViens == null)
            {
                ListGiaoViens = new ObservableCollection<GiaoVien>(result);
            }
            else
            {
                ListGiaoViens.Clear();
                foreach (var gv in result)
                {
                    ListGiaoViens.Add(gv);
                }
            }
        }
    }
}

