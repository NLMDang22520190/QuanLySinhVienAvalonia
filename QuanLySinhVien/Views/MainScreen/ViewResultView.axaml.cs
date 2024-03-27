using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using Avalonia.Controls;
using HarfBuzzSharp;
using QuanLySinhVien.ViewModels;
using QuanLySinhVien.ViewModels.MainScreen;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ViewResultView : UserControl
    {
        private ViewResultViewModel viewModel;

        public ViewResultView()
        {
            InitializeComponent();

            var viewModelName = $"QuanLySinhVien.ViewModels.MainScreen.ViewResultViewModel";
            viewModel = (ViewResultViewModel)MainScreenViewModel.CreateInstance<ViewModelBase>(
                MainScreenViewModel.ViewModelList, viewModelName);

        }

        private void AttachHandler()
        {
            if (viewModel != null)
            {
                var dataGrid = this.FindControl<DataGrid>("DataGrid"); // Thay "YourDataGridName" bằng tên thật của DataGrid của bạn
                if (dataGrid != null)
                {
                    dataGrid.CellEditEnded += async (sender, e) =>
                    {
                        Debug.WriteLine("ended");
                        await viewModel.UpdateListCommand.Execute();
                    };
                }
            }
        }
    }
}
