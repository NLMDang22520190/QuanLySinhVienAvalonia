﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using QuanLySinhVien.Models;
using ReactiveUI;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddStudentViewModel :ViewModelBase
    {
        #region Properties

        private string _tenHocSinh = string.Empty;
        public string TenHocSinh
        {
            get => _tenHocSinh;
            set => this.RaiseAndSetIfChanged(ref _tenHocSinh, value);
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

        public ReactiveCommand<Window, HocSinh> AddCommand { get; }
        public AddStudentViewModel()
        {
            var isValidObservable = this.WhenAnyValue(
                x => x.TenHocSinh,
                x => x.DiaChi,
                x => x.Email,
                x => x.NgaySinh,
                x => x.GioiTinh,
                (tenHocSinh, diaChi, email, ngaySinh, gioiTinh) =>
                {
                    return !string.IsNullOrWhiteSpace(tenHocSinh)
                           && !string.IsNullOrWhiteSpace(diaChi)
                           && !string.IsNullOrWhiteSpace(email)
                           && ngaySinh != DateTime.MinValue
                           && gioiTinh.HasValue;
                });

            AddCommand = ReactiveCommand.CreateFromTask<Window, HocSinh>(OnAdd, isValidObservable);
        }
        private async Task<HocSinh> OnAdd(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn thêm học sinh ?", ButtonEnum.YesNo, Icon.Question);
            
            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                var newHocSinh = new HocSinh
                {
                    MaHocSinh = "HS" + (DataProvider.Ins.DB.HocSinhs.Count() + 1).ToString(),
                    TenHocSinh = TenHocSinh,
                    NgaySinh = NgaySinh,
                    GioiTinh = GioiTinh,
                    DiaChi = DiaChi,
                    Email = Email
                };
                return newHocSinh;
            }
            return null;
            
        }

        public void OnCancel(Window window)
        {
            window.Close();
        }
    }
}
