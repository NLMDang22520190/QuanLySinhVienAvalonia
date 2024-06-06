using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
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

        private ReactiveCommand<Unit, Unit> addTeacherCommand;

        public ReactiveCommand<Unit, Unit> AddTeacherCommand
        {
            get => addTeacherCommand;
            set => this.RaiseAndSetIfChanged(ref addTeacherCommand, value);
        }

        public TeacherInfoViewModel()
        {
            var result = DataProvider.Ins.DB.GiaoViens.ToList();
            listGiaoViens = new ObservableCollection<GiaoVien>(result);

            UpdateCurrentTime();

        }

        public void OpenAddTeacherWindow(Window window)
        {
            var addTeacherWindow = new AddTeacherView();
            addTeacherWindow.DataContext = new AddTeacherViewModel();
            addTeacherWindow.ShowDialog(window);
        }
    }
}
