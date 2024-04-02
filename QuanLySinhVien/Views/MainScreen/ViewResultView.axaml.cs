using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using HarfBuzzSharp;
using QuanLySinhVien.Models;
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
            AttachHandler();

        }

        private void AttachHandler()
        {
            if (viewModel != null)
            {
                var dataGrid = this.FindControl<DataGrid>("DataGrid"); // Thay "YourDataGridName" bằng tên thật của DataGrid của bạn
                var resultIDTextBox = this.FindControl<TextBox>("ResultIDTextBox");
                var resultNameTextBox = this.FindControl<TextBox>("ResultNameTextBox");
                if (dataGrid != null)
                {
                    dataGrid.CellEditEnding += (object sender, DataGridCellEditEndingEventArgs e) =>
                    {
                        viewModel.BackupData();
                    };

                    dataGrid.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                    {
                        viewModel.SelectedItemRow = dataGrid.SelectedIndex;
                        if (dataGrid.SelectedItem != null)
                        {
                            if (resultIDTextBox != null)
                            {
                                resultIDTextBox.Text = (dataGrid.SelectedItem as ResultModel).ResultID;
                            }
                            if (resultNameTextBox != null)
                            {
                                resultNameTextBox.Text = (dataGrid.SelectedItem as ResultModel).ResultName;
                            }
                        }
                    };
                }
                var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
                if (searchTextBox != null)
                {
                    searchTextBox.KeyDown += (object sender, KeyEventArgs e) =>
                    {
                        if (e.Key == Key.Enter)
                        {
                            viewModel.SearchText = searchTextBox.Text;
                            // Thực hiện tìm kiếm ở đây
                        }
                    };
                    searchTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
                    {
                        viewModel.SearchText = searchTextBox.Text;
                    };
                }
            }
        }
    }
}
