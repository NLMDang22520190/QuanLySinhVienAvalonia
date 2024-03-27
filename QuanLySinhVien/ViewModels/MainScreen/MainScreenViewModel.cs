using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class MainScreenViewModel : ViewModelBase
    {
        private static readonly List<ViewModelBase> _viewModelList = new List<ViewModelBase>();
        private readonly List<UserControl> _viewList = new List<UserControl>();

        public static List<ViewModelBase> ViewModelList => _viewModelList;

        public MainScreenViewModel()
        {
            // Add code here
        }

        public void OnNavigateViewSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            var tag = (e.SelectedItem as NavigationViewItem).Tag.ToString();
            var viewModelName = $"QuanLySinhVien.ViewModels.MainScreen.{tag}ViewModel";
            var viewName = $"QuanLySinhVien.Views.MainScreen.{tag}View";

            var viewModelInstance = CreateInstance<ViewModelBase>(_viewModelList, viewModelName);
            var viewInstance = CreateInstance<UserControl>(_viewList, viewName);

            (sender as NavigationView).Content = viewInstance;
            (sender as NavigationView).DataContext = viewModelInstance;

        }

        public static T CreateInstance<T>(List<T> instanceList, string typeName) where T : class
        {
            string bigString = typeName;
            string[] parts = bigString.Split('.'); // Tách chuỗi thành mảng các chuỗi con dựa trên ký tự '.'
            string smallString = parts[parts.Length - 1]; // Lấy phần tử cuối cùng của mảng


            var instance = instanceList.FirstOrDefault(x => x.GetType().Name == smallString);
            if (instance != null)
            {
                return instance;
            }
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException($"Type {typeName} not found");
            }
            instance = Activator.CreateInstance(type) as T;
            if (instance == null)
            {
                throw new ArgumentException($"Type {typeName} is not a {typeof(T).Name}");
            }
            instanceList.Add(instance);
            return instance;
        }
    }
}