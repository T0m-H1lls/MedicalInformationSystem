using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Medical_information_system.ViewModels;
using Medical_information_system.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        
    }
    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            /*
            var vm = _serviceProvider.GetRequiredService<RegistrationViewModel>();
            var win = _serviceProvider.GetRequiredService<RegistrationWindow>();
            win.DataContext = vm;
            desktop.MainWindow = win;
            */
            
            var vm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            var win = _serviceProvider.GetRequiredService<MainWindow>();
            win.DataContext = vm;
            desktop.MainWindow = win;
            
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}