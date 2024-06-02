using FluentAvalonia.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class AddClassViewModel : ViewModelBase
    {
       private AppWindow _addClassWindow;
       public AddClassViewModel()
        {
            // Add code here
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
