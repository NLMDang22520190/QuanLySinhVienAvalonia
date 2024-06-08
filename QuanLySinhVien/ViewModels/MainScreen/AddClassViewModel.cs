using FluentAvalonia.UI.Windowing;
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
using Avalonia.Controls;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddClassViewModel : ViewModelBase
    {
       private AppWindow _addClassWindow;


        private string _tenLop = string.Empty;

        public string TenLop
        {
            get => _tenLop;
            set => this.RaiseAndSetIfChanged(ref _tenLop, value);
        }

        private string _maNienKhoa = string.Empty;

        public string MaNienKhoa
        {
            get => _maNienKhoa;
            set => this.RaiseAndSetIfChanged(ref _maNienKhoa, value);
        }


        private int? _siSo = 0;

        public int? SiSo
        {
            get => _siSo;
            set
            {
                this.RaiseAndSetIfChanged(ref _siSo, value);
            }
        }


        private string _tenGvcn = string.Empty;
        public string TenGvcn
        {
            get => _tenGvcn;
            set => this.RaiseAndSetIfChanged(ref _tenGvcn, value);
        }


        private ObservableCollection<string> khoisCb;

        public ObservableCollection<string> KhoisCb
        {
            get => khoisCb;
            set => this.RaiseAndSetIfChanged(ref khoisCb, value);
        }


        private string _maKhoi = string.Empty;

        public string MaKhoi
        {
            get => _maKhoi;
            set => this.RaiseAndSetIfChanged(ref _maKhoi, value);
        }
        public ReactiveCommand<Unit, Lop> AddCommand { get; }
        public AddClassViewModel()
        {
            var result = DataProvider.Ins.DB.Khois.Select(k => k.MaKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result);

            var isValidObservable = this.WhenAnyValue(
                x => x.TenLop,
                
                x => x.MaKhoi,
                (tenLop,  khoi) =>
                {
                    return !string.IsNullOrWhiteSpace(tenLop)                       
                           && !string.IsNullOrWhiteSpace(khoi);                       
                });

            AddCommand = ReactiveCommand.Create<Lop>(OnAdd, isValidObservable);
        }


        private Lop OnAdd()
        {
            var currentYear = DateTime.Now.Year;
            var newLop = new Lop
            {
                MaLop = "Lop" + (DataProvider.Ins.DB.Lops.Count() + 1).ToString(),
                TenLop = TenLop,
                MaNienKhoa = "NK" + currentYear.ToString(),
                SiSo = 0,              
                MaKhoi = MaKhoi,
            };
            return newLop;
        }

        public void OnCancel(Window window)
        {
            window.Close();
        }
        public AddClassViewModel(AppWindow addClassWindow)
        {
            _addClassWindow = addClassWindow;
        }

        public void CloseAddCClassWindow()
        {
            _addClassWindow.Close();
        }
    }
}
