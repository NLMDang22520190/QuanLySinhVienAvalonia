using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using ReactiveUI;
using Microsoft.EntityFrameworkCore; // Ensure you include this namespace for async operations

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ManageScoreViewModel : ViewModelBase
    {
        private ObservableCollection<string> nienKhoas;
        private readonly QlhsContext db;

        public ObservableCollection<string> NienKhoas
        {
            get => nienKhoas;
            set => this.RaiseAndSetIfChanged(ref nienKhoas, value);
        }

        public ManageScoreViewModel()
        {
            db = new QlhsContext();
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                var result = await db.NienKhoas.Select(nk => nk.TenNienKhoa).ToListAsync();
                NienKhoas = new ObservableCollection<string>(result);
            }
            catch (Exception ex)
            {
                // Handle exception (log it, show message to user, etc.)
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }
    }
}