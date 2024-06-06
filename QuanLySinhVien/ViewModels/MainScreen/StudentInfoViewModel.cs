using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;

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

        private ObservableCollection<HocSinh> listHocSinhs;

        public ObservableCollection<HocSinh> ListHocSinhs
        {
            get => listHocSinhs;
            set => this.RaiseAndSetIfChanged(ref listHocSinhs, value);
        }

        private ReactiveCommand<Unit, Unit> addStudentCommand;

        public ReactiveCommand<Unit, Unit> AddStudentCommand
        {
            get => addStudentCommand;
            set => this.RaiseAndSetIfChanged(ref addStudentCommand, value);
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

        public StudentInfoViewModel()
        {
            var result1 = DataProvider.Ins.DB.HocSinhs.ToList();
            listHocSinhs = new ObservableCollection<HocSinh>(result1);
            var result2 = DataProvider.Ins.DB.NienKhoas.Select(nk => nk.TenNienKhoa).ToList();
            NienKhoasCb = new ObservableCollection<string>(result2);
            var result3 = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result3);
            var result4 = DataProvider.Ins.DB.Lops.Select(l => l.TenLop).ToList();
            LopsCb = new ObservableCollection<string>(result4);

            UpdateCurrentTime();

        }

        public void OpenAddStudentWindow()
        {
            AddStudentView addStudentView = new AddStudentView();
            addStudentView.DataContext = new AddStudentViewModel(addStudentView);
            addStudentView.ShowDialog(addStudentView);


        }
    }
}
