using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;
using Avalonia.Controls;
using System.Reactive.Linq;

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

        private ObservableCollection<Lop> allLops;

        public ObservableCollection<Lop> AllLops
        {
            get => allLops;
            set => this.RaiseAndSetIfChanged(ref allLops, value);
        }

        private int selectedClassIndex;

        public int SelectedClassIndex
        {
            get => selectedClassIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedClassIndex, value);
            }
        }

        public ReactiveCommand<Window, Unit> OpenEditClassWindowCommand { get; }

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

        private string searchName;

        public string SearchName
        {
            get => searchName;
            set => this.RaiseAndSetIfChanged(ref searchName, value);
        }

        public ClassViewModel()
        {
            LoadListLop();
            OpenEditClassWindowCommand = ReactiveCommand.Create<Window>(OpenEditClassWindow);
            var result1 = DataProvider.Ins.DB.Lops.ToList();
            listLops = new ObservableCollection<Lop>(result1);
            var result2 = DataProvider.Ins.DB.NienKhoas.Select(nk => nk.TenNienKhoa).ToList();
            NienKhoasCb = new ObservableCollection<string>(result2);
            var result3 = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result3);
        }
        public void OpenAddClassWindow(Window window) {

            //AddClassView addClassView = new AddClassView();
            //addClassView.DataContext = new AddClassViewModel();
            //addClassView.Show();

            var addClassWindow = new AddClassView();
            var addClassViewModel = new AddClassViewModel();
            addClassWindow.DataContext = addClassViewModel;

            addClassWindow.Title = "Thêm lớp";
            addClassWindow.ShowDialog(window);

            addClassViewModel.AddCommand
                .Take(1)
                .Subscribe(newClass =>
                {
                    if (newClass != null)
                    {
                        DataProvider.Ins.DB.Lops.Add(newClass);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListLop();
                    }
                    addClassWindow.Close();
                });




        }

        public void DeleteSelectedClass()
        {
            if (SelectedClassIndex == -1)
            {
                return;
            }

            var selectedLop = ListLops[SelectedClassIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.Lops.Local.SingleOrDefault(l => l.MaLop == selectedLop.MaLop); ;
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            DataProvider.Ins.DB.Lops.Remove(selectedLop);
            DataProvider.Ins.DB.SaveChanges();
            LoadListLop();

        }


        public void SearchClass(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
            {
                LoadListClassFromMemory();
            }
            else
            {
                // Filter the list based on searchName
                var filteredList = AllLops
                    .Where(l => l.TenLop.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ListLops.Clear();
                foreach (var l in filteredList)
                {
                    ListLops.Add(l);
                }
            }
        }


        public void OpenEditClassWindow(Window window)
        {
            if (SelectedClassIndex == -1)
            {
                return;
            }

            var selectedLop = ListLops[SelectedClassIndex];
            // Ensure this entity is detached before attaching a new instance
            var existingEntity = DataProvider.Ins.DB.Lops.Local.SingleOrDefault(l => l.MaLop == selectedLop.MaLop);
            if (existingEntity != null)
            {
                DataProvider.Ins.DB.Entry(existingEntity).State = EntityState.Detached;
            }

            var editClassWindow = new EditClassView();
            var editClassViewModel = new EditClassViewModel(selectedLop);
            editClassWindow.DataContext = editClassViewModel;

            editClassWindow.Title = "Sửa thông tin lớp học";
            editClassWindow.ShowDialog(window);

            editClassViewModel.EditCommand
                .Take(1)
                .Subscribe(l =>
                {
                    if (l != null)
                    {
                        DataProvider.Ins.DB.Lops.Update(l);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadListLop();
                    }
                    editClassWindow.Close();
                });
        }

        private void LoadListLop()
        {
            var result = DataProvider.Ins.DB.Lops.AsNoTracking().ToList();
            if (ListLops == null)
            {
                ListLops = new ObservableCollection<Lop>(result);
                AllLops = new ObservableCollection<Lop>(result);
            }
            else
            {
                ListLops.Clear();
                AllLops.Clear();
                foreach (var l in result)
                {
                    ListLops.Add(l);
                    AllLops.Add(l);
                }
            }
        }



        private void LoadListClassFromMemory()
        {
            ListLops.Clear();
            foreach (var l in AllLops)
            {
                ListLops.Add(l);
            }
        }


    }
}
