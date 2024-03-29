﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using QuanLySinhVien.ViewModels;
using QuanLySinhVien.ViewModels.Login;
using QuanLySinhVien.Views;
using QuanLySinhVien.Views.Login;

namespace QuanLySinhVien;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //desktop.MainWindow = new MainWindow
            //{
            //    DataContext = new MainViewModel()
            //};

            desktop.MainWindow = new LoginScreenView();
            
            desktop.MainWindow.DataContext = new LoginScreenViewModel(desktop.MainWindow);
            
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
