using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.ViewModels.MainScreen;
using System.Diagnostics;
using System.Linq;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ViewStudentsAccomplishmentView : UserControl
    {
        public ViewStudentsAccomplishmentView()
        {
            InitializeComponent();
            ChangeDataGridReadOnlyState();
            Debug.WriteLine("loaded");
            this.Loaded += ConfigueStudentScoreView_Loaded;
        }

        private void ConfigueStudentScoreView_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeDataGridReadOnlyState();
        }


        private void ChangeDataGridReadOnlyState()
        {
            var dataGrid = this.FindControl<DataGrid>("DataGrid");
            // Ensure the DataGrid is found before proceeding
            if (dataGrid != null)
            {
                dataGrid.GotFocus += (sender, e) =>
                {
                    // Access DataContext (ViewModel)
                    var viewModel = this.DataContext as ViewStudentsAccomplishmentViewModel;

                    // Check if ViewModel is null
                    if (viewModel == null)
                    {
                        // Handle the case where ViewModel is null (optional)
                        Debug.WriteLine("ViewModel is null.");
                        return;
                    }

                    // ViewModel is not null, you can perform actions with it
                    // For example, check some property or call a method
                    dataGrid.IsReadOnly = viewModel.IsEnableEditing; // Example method call
                };

            }
        }


    }
}
