using QuanLySinhVien.Views.MainScreen;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ClassDetailViewModel: ViewModelBase
    {
        private ReactiveCommand<Unit, Unit> addStudentToClassCommand;
        public ReactiveCommand<Unit, Unit> AddStudentToClassCommand
        {
            get => addStudentToClassCommand;
            set => this.RaiseAndSetIfChanged(ref addStudentToClassCommand, value);
        }

        public void OpenAddStudentToClassWindow()
        {

            AddStudentToClassView addStudentToClassView = new AddStudentToClassView();
            //addClassView.DataContext = new AddClassViewModel(addClassView);
            addStudentToClassView.Show();
        }
    }
}
