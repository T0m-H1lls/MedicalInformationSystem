using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class StartViewModel:ViewModelBase
{
    private readonly Navigation _navigation;
    [ObservableProperty] private ViewModelBase _currentPage;

    public StartViewModel(IServiceProvider sv, Navigation navigation)
    {
        _navigation = navigation;
        _navigation.SetCurrentView(this);
        _navigation.Navigate(sv.GetRequiredService<RegistrationViewModel>());
    }
}