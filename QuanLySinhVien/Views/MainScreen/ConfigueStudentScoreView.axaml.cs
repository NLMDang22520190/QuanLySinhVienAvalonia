using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.ViewModels.MainScreen;
using System.Diagnostics;
using System.Linq;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class ConfigueStudentScoreView : UserControl
    {
        public ConfigueStudentScoreView()
        {
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            var DataGrid = this.FindControl<DataGrid>("DataGrid");
            if (DataGrid != null)
            {
                var trangthai = DataProvider.Ins.DB.QuiDinhs.ToList();
                foreach (var item in trangthai)
                {
                    if (item.MaQuiDinh == "QD1")
                    {
                        if (item.GiaTri == 1)
                        {
                            DataGrid.IsReadOnly = false;
                        }
                        else
                        {
                            DataGrid.IsReadOnly = true;
                        }
                    }
                }
            }
        }

        
    }
}
