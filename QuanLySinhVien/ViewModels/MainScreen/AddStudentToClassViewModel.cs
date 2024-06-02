using FluentAvalonia.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddStudentToClassViewModel : ViewModelBase
    {
        private AppWindow _addStudentToClassWindow;
        public AddStudentToClassViewModel()
        {
            // Add code here
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