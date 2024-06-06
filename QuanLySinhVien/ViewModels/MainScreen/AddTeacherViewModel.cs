using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using QuanLySinhVien.Models;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddTeacherViewModel : ViewModelBase
    {
        #region Properties

        private string _tenGiaoVien = string.Empty;

        public string TenGiaoVien
        {
            get => _tenGiaoVien;
            set => this.RaiseAndSetIfChanged(ref _tenGiaoVien, value);
        }

        private DateTime _ngaySinh;

        public DateTime NgaySinh
        {
            get => _ngaySinh;
            set
            {
                this.RaiseAndSetIfChanged(ref _ngaySinh, value);
                Debug.WriteLine(_ngaySinh);
            }
        }

        private bool? _gioiTinh;

        public bool? GioiTinh
        {
            get => _gioiTinh;
            set
            {
                this.RaiseAndSetIfChanged(ref _gioiTinh, value);
            }
        }

        private string _diaChi = string.Empty;

        public string DiaChi
        {
            get => _diaChi;
            set => this.RaiseAndSetIfChanged(ref _diaChi, value);
        }

        private string _email = string.Empty;

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }


        #endregion

        //public ReactiveCommand<Window, Unit> AddCommand { get; }
        public ReactiveCommand<Unit, GiaoVien> AddCommand { get; }

        public AddTeacherViewModel()
        {
            var isValidObservable = this.WhenAnyValue(
                x => x.TenGiaoVien,
                x => x.DiaChi,
                x => x.Email,
                x => x.NgaySinh,
                x => x.GioiTinh,
                (tenGiaoVien, diaChi, email, ngaySinh, gioiTinh) =>
                {
                    return !string.IsNullOrWhiteSpace(tenGiaoVien)
                           && !string.IsNullOrWhiteSpace(diaChi)
                           && !string.IsNullOrWhiteSpace(email)
                           && ngaySinh != DateTime.MinValue
                           && gioiTinh.HasValue;
                });

            //AddCommand = ReactiveCommand.Create<Window>(OnAdd, isValidObservable);
            //AddCommand = ReactiveCommand.Create(() => new GiaoVien
            //{
            //    MaGiaoVien = "GV" + (DataProvider.Ins.DB.GiaoViens.Count() + 1).ToString(),
            //    TenGiaoVien = TenGiaoVien,
            //    NgaySinh = NgaySinh,
            //    GioiTinh = GioiTinh,
            //    DiaChi = DiaChi,
            //    Email = Email
            //}, isValidObservable);

            AddCommand = ReactiveCommand.Create<GiaoVien>(OnAdd, isValidObservable);

        }

        //private void OnAdd(Window window)
        //{
        //    var newGiaoVien = new GiaoVien
        //    {
        //        MaGiaoVien = "GV" + (DataProvider.Ins.DB.GiaoViens.Count() + 1).ToString(),
        //        TenGiaoVien = TenGiaoVien,
        //        NgaySinh = NgaySinh,
        //        GioiTinh = GioiTinh,
        //        DiaChi = DiaChi,
        //        Email = Email
        //    };
        //    DataProvider.Ins.DB.GiaoViens.Add(newGiaoVien);
        //    DataProvider.Ins.DB.SaveChanges();
        //    window.Close();

        //}

        private GiaoVien OnAdd()
        {
            var newGiaoVien = new GiaoVien
            {
                MaGiaoVien = "GV" + (DataProvider.Ins.DB.GiaoViens.Count() + 1).ToString(),
                TenGiaoVien = TenGiaoVien,
                NgaySinh = NgaySinh,
                GioiTinh = GioiTinh,
                DiaChi = DiaChi,
                Email = Email
            };
            return newGiaoVien;
        }

        public void OnCancel(Window window)
        {
            window.Close();
        }
    }
}
