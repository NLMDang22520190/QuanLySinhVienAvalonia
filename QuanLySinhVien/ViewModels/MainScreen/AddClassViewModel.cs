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

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddClassViewModel : ViewModelBase
    {
       private AppWindow _addClassWindow;

        private ObservableCollection<string> khoisCb;

        public ObservableCollection<string> KhoisCb
        {
            get => khoisCb;
            set => this.RaiseAndSetIfChanged(ref khoisCb, value);
        }
        public AddClassViewModel()
        {
            var result = DataProvider.Ins.DB.Khois.Select(k => k.TenKhoi).ToList();
            KhoisCb = new ObservableCollection<string>(result);
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
