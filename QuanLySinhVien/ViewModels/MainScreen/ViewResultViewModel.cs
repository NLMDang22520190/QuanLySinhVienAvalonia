using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ViewResultViewModel : ViewModelBase
    {
        private string _currentTime;

        public String CurrentTime
        {
            get => _currentTime;
            set => this.RaiseAndSetIfChanged(ref _currentTime, value);
        }
        public ViewResultViewModel()
        {
            // Khởi tạo và bắt đầu cập nhật thời gian
            UpdateCurrentTime();
        }

        private async void UpdateCurrentTime()
        {
            while (true)
            {
                // Cập nhật thời gian mỗi giây
                CurrentTime = DateTime.Now.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Chờ 1 giây trước khi cập nhật lại
                await Task.Delay(1000);
            }
        }
    }
}
