using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class RegistrationViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty] private ViewModelBase _currentPage; 
    [ObservableProperty]private string _name;
    [ObservableProperty]private string _surname;
    [ObservableProperty]private string  _patronymic;
    [ObservableProperty]private string _login;
    [ObservableProperty]private string _password;
    
    public RegistrationViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    [RelayCommand]
     void OpenAutorization()
    {
        CurrentPage = new AuthorizationViewModel(_serviceProvider);

    }
    
}