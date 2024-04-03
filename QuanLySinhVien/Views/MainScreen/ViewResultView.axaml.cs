using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.VisualTree;
using HarfBuzzSharp;
using QuanLySinhVien.Models;
using QuanLySinhVien.TemplatedControls;
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
                BindDataGrid();
                BindSearchText();
            }
        }

        private void BindSearchText()
        {
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

        private void BindDataGrid()
        {
            var dataGrid = this.FindControl<DataGrid>("DataGrid"); // Thay "YourDataGridName" bằng tên thật của DataGrid của bạn
            var resultIdTextBox = this.FindControl<TextBox>("ResultIdTextBox");
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
                        if (resultIdTextBox != null && resultNameTextBox != null)
                        {
                            var result = (ResultModel)dataGrid.SelectedItem;
                            resultIdTextBox.Text = result.ResultID;
                            resultNameTextBox.Text = result.ResultName;
                        }
                    }

                };
            }
        }
    }
}