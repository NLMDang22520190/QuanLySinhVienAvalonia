using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using QuanLySinhVien.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddStudentToClassViewModel : ViewModelBase
    {
        private AppWindow _addStudentToClassWindow;

        private ListClassViewModel _parentViewModel;

        private ObservableCollection<HocSinh> listHocSinhs;

        public ObservableCollection<HocSinh> ListHocSinhs
        {
            get => listHocSinhs;
            set => this.RaiseAndSetIfChanged(ref listHocSinhs, value);
        }

        private ObservableCollection<HocSinh> allHocSinhs;

        public ObservableCollection<HocSinh> AllHocSinhs
        {
            get => allHocSinhs;
            set => this.RaiseAndSetIfChanged(ref allHocSinhs, value);
        }

        private ObservableCollection<int> years;
        public ObservableCollection<int> Years
        {
            get => years;
            set => this.RaiseAndSetIfChanged(ref years, value);
        }

        private int? selectedYear;
        public int? SelectedYear
        {
            get => selectedYear;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedYear, value);
                FilterHocSinhsByYear();
            }
        }

        private HocSinh selectedHocSinh;
        public HocSinh SelectedHocSinh
        {
            get => selectedHocSinh;
            set => this.RaiseAndSetIfChanged(ref selectedHocSinh, value);
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
        public AddStudentToClassViewModel()
        {
            ListHocSinhs = new ObservableCollection<HocSinh>();
            AllHocSinhs = new ObservableCollection<HocSinh>();
            Years = new ObservableCollection<int>();
            LoadHocSinhs();
        }

        public AddStudentToClassViewModel(ListClassViewModel parentViewModel)
        {
            //_parentViewModel = parentViewModel;
            //LoadHocSinhs();
            _parentViewModel = parentViewModel ?? throw new ArgumentNullException(nameof(parentViewModel));
            LoadHocSinhs();
        }

        private void LoadHocSinhs()
        {
            var result = DataProvider.Ins.DB.HocSinhs
                            .Where(hs => hs.MaLop == null)
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

            var yearsList = result
                .Where(hs => hs.NgaySinh.HasValue)
                .Select(hs => hs.NgaySinh.Value.Year)
                .Distinct()
                .OrderBy(year => year)
                .ToList();
            Years = new ObservableCollection<int>(yearsList);
        }

        private void FilterHocSinhsByYear()
        {
            if (SelectedYear.HasValue)
            {
                var filteredList = AllHocSinhs
                    .Where(hs => hs.NgaySinh.HasValue && hs.NgaySinh.Value.Year == SelectedYear.Value)
                    .ToList();

                ListHocSinhs.Clear();
                foreach (var hs in filteredList)
                {
                    ListHocSinhs.Add(hs);
                }
            }
            else
            {
                ListHocSinhs.Clear();
                foreach (var hs in AllHocSinhs)
                {
                    ListHocSinhs.Add(hs);
                }
            }
        }
        public AddStudentToClassViewModel(AppWindow addStudentToClassWindow)
        {
            _addStudentToClassWindow = addStudentToClassWindow;
        }

        public void CloseAddStudentToClassWindow()
        {
            _addStudentToClassWindow.Close();
        }

        public async Task ConfirmAddStudent(Window window)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Thông báo", "Bạn có chắc chắn muốn thêm học sinh ?", ButtonEnum.YesNo, Icon.Question);

            var result = await box.ShowWindowDialogAsync(window);

            if (result == ButtonResult.Yes)
            {
                if (SelectedHocSinh != null)
                {
                    _parentViewModel.AddStudentToClass(SelectedHocSinh);
                    ListHocSinhs.Remove(SelectedHocSinh);
                }
            }
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