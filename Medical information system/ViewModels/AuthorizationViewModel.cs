using System;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class AuthorizationViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Navigation _navigation;

    public AuthorizationViewModel(IServiceProvider serviceProvider,Navigation navigation)
    {
        _serviceProvider = serviceProvider;
        _navigation = navigation;
    }

    [RelayCommand]
    void OpenRegistration()
    {

        var vm = _serviceProvider.GetRequiredService<RegistrationViewModel>();
        _navigation.Navigate(vm);
    }
}