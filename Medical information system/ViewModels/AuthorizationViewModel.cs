using System;

namespace Medical_information_system.ViewModels;

public class AuthorizationViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}