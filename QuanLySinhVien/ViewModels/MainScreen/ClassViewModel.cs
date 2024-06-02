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
        private ReactiveCommand<Unit, Unit> addClassCommand;
        public ReactiveCommand<Unit, Unit> AddClassCommand
        {
            get => addClassCommand;
            set => this.RaiseAndSetIfChanged(ref addClassCommand, value);
        }

        public void OpenAddClassWindow() {

            AddClassView addClassView = new AddClassView();
            //addClassView.DataContext = new AddClassViewModel(addClassView);
            addClassView.Show();
        }
    }
}
