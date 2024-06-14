using Avalonia.Controls;
using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using System.IO;

namespace QuanLySinhVien.Views.MainScreen
{
    public partial class HomeView : UserControl
    {

        private List<string> _imageSources = new List<string>
        {
            "Assets/Images/THPTThuDuc11.jpg",
            "Assets/Images/THPTThuDuc2.jpg",
            "Assets/Images/THPTThucDuc3.jpg",
            "Assets/Images/THPTThucDuc4.jpg"

        };
        public HomeView()
        {
            InitializeComponent();
            this.Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SetRandomImage();
        }

        private void SetRandomImage()
        {
            Random random = new Random();
            int index = random.Next(_imageSources.Count);
            string relativePath = _imageSources[index];
            string appDirectory = GetAppDirectory();
            string absolutePath = Path.Combine(appDirectory, relativePath);
            DynamicImage.Source = new Avalonia.Media.Imaging.Bitmap(absolutePath);
        }

        private string GetAppDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            return Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..")); // ?i hai c?p lên ?? l?y th? m?c g?c c?a d? án
        }
    }
}
