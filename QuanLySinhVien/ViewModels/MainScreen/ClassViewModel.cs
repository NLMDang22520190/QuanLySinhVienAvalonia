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
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ClassViewModel: ViewModelBase
    {

        private ObservableCollection<Lop> listLops;
        public ObservableCollection<Lop> ListLops
        {
            get => listLops; 
            set => this.RaiseAndSetIfChanged(ref listLops, value);
        }

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

        public ClassViewModel()
        {
            var result1 = DataProvider.Ins.DB.Lops.ToList();
            listLops = new ObservableCollection<Lop>(result1);
            var result2 = DataProvider.Ins.DB.NienKhoas.Select(nk => nk.TenNienKhoa).ToList();
            NienKhoasCb = new ObservableCollection<string>(result2);
            var result3 = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result3);
        }
        public void OpenAddClassWindow() {

            AddClassView addClassView = new AddClassView();
            addClassView.DataContext = new AddClassViewModel();
            addClassView.Show();
        }
    }
}
