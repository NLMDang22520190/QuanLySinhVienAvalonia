using FluentAvalonia.UI.Windowing;
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
    }
}