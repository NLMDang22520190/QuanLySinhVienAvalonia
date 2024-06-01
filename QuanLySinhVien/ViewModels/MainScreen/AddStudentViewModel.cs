using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAvalonia.UI.Windowing;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddStudentViewModel :ViewModelBase
    {
        private AppWindow _addStudentWindow;

        public AddStudentViewModel()
        {
            // Add code here
        }

        public AddStudentViewModel(AppWindow addStudentWindow)
        {
            _addStudentWindow = addStudentWindow;
        }

        public void CloseAddStudentWindow()
        {
            _addStudentWindow.Close();
        }

    }
}
