using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public ClassViewModel ClassVM { get; private set; }

    public MainViewModel()
    {
        ClassVM = new ClassViewModel();
    }

    // Phương thức để xử lý khi người dùng đăng nhập
    public void OnUserLogin()
    {
        ClassVM.LoadClasses();
    }
}
