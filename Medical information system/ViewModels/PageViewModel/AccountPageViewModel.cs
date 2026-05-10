using System;
using Medical_information_system.DB;

namespace Medical_information_system.ViewModels;

public partial class AccountPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    

    public AccountPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
}