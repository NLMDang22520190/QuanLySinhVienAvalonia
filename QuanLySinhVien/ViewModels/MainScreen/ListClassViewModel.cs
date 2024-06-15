using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ListClassViewModel: ViewModelBase
    {
        private static bool isAdmin = false;
        public static bool IsAdmin
        {
            get => isAdmin;
            set => isAdmin = value;
        }
        private ObservableCollection<Lop> listLops;
        public ObservableCollection<Lop> ListLops
        {
            get => listLops;
            set => this.RaiseAndSetIfChanged(ref listLops, value);
        }
        private ObservableCollection<HocSinh> listHocSinhs;

        public ObservableCollection<HocSinh> ListHocSinhs
        {
            get => listHocSinhs;
            set => this.RaiseAndSetIfChanged(ref listHocSinhs, value);
        }

        private Lop selectedLop;

        public Lop SelectedLop
        {
            get => selectedLop;
            set => this.RaiseAndSetIfChanged(ref selectedLop, value);
        }

        private Lop currentLop;
        public Lop CurrentLop
        {
            get => currentLop;
            set => this.RaiseAndSetIfChanged(ref currentLop, value);
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
            set { this.RaiseAndSetIfChanged(ref selectedHocSinhIndex, value); }
        }

        private string searchName;

        public string SearchName
        {
            get => searchName;
            set => this.RaiseAndSetIfChanged(ref searchName, value);

        }

        private ReactiveCommand<Unit, Unit> addStudentToClassCommand;
        public ReactiveCommand<Unit, Unit> AddStudentToClassCommand
        {
            get => addStudentToClassCommand;
            set => this.RaiseAndSetIfChanged(ref addStudentToClassCommand, value);
        }

        public ReactiveCommand<Window, Unit> DeleteStudentFromClassCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ListClassViewModel(Lop lop)
        {
            ListLops = new ObservableCollection<Lop>();
            SelectedLop = lop;
            ListHocSinhs = new ObservableCollection<HocSinh>();
            AllHocSinhs = new ObservableCollection<HocSinh>();
            DeleteStudentFromClassCommand = ReactiveCommand.Create<Window>(DeleteStudentFromClass);
            LoadHocSinhs();
        }

        private void LoadHocSinhs()
        {
            var result = DataProvider.Ins.DB.HocSinhs
                            .Where(hs => hs.MaLop == SelectedLop.MaLop)
                            .ToList();
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


        public void AddStudentToClass(HocSinh hocSinh)
        {
            if (hocSinh != null)
            {
                // Đảm bảo SelectedLop không phải là null
                if (SelectedLop == null)
                {
                    throw new InvalidOperationException("SelectedLop không thể là null.");
                }

                // Gán học sinh vào lớp đã chọn
                hocSinh.MaLop = SelectedLop.MaLop;

                // Thêm học sinh vào các danh sách
                ListHocSinhs.Add(hocSinh);
                AllHocSinhs.Add(hocSinh);

                // Tăng sĩ số lớp
                SelectedLop.SiSo += 1;

                // Lưu các thay đổi vào cơ sở dữ liệu
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Ghi lại chi tiết lỗi hoặc xử lý theo cách phù hợp
                    // Ví dụ: LogError(ex); 
                    throw new InvalidOperationException("Đã xảy ra lỗi khi lưu thay đổi vào cơ sở dữ liệu. Xem chi tiết trong lỗi nội tại.", ex);
                }

                // Thông báo thay đổi thuộc tính
                OnPropertyChanged(nameof(ListLops));
            }
        }

        public async void DeleteStudentFromClass(Window window)
        {
            var hocSinh = ListHocSinhs[SelectedHocSinhIndex];
           
            var box = MessageBoxManager.GetMessageBoxStandard("Xác nhận xóa", "Bạn có chắc chắn muốn xóa học sinh này không?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                if (hocSinh != null)
                {
                    hocSinh.MaLop = null; // Remove the student from the class
                    DataProvider.Ins.DB.SaveChanges();
                    ListHocSinhs.Remove(hocSinh);
                    AllHocSinhs.Remove(hocSinh);
                    SelectedLop.SiSo -= 1;
                    OnPropertyChanged(nameof(ListLops));
                }
                MessageBoxManager.GetMessageBoxStandard("Thông báo", "Xóa học sinh thành công", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
            }
        }
        public void OpenAddStudentToClassWindow(Window parentWindow)
        {
            //var addStudentToClassWindow = new AddStudentToClassView();
            //var addStudenttoClassViewModel = new AddStudentToClassViewModel();
            //addStudentToClassWindow.DataContext = addStudenttoClassViewModel;

            //addStudentToClassWindow.Title = "Thêm học sinh vào lớp";
            //addStudentToClassWindow.ShowDialog(window);
            var addStudentWindow = new AddStudentToClassView
            {
                DataContext = new AddStudentToClassViewModel(this)
            };
            addStudentWindow.ShowDialog(parentWindow);
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


        private void LoadListHocSinhFromMemory()
        {
            ListHocSinhs.Clear();
            foreach (var hs in AllHocSinhs)
            {
                ListHocSinhs.Add(hs);
            }
        }

        public void OnCancel(Window window)
        {
            window.Close();
        }
    }
}
