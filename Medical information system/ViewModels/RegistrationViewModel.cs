using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class RegistrationViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Navigation _navigation;
    private readonly UserControl _userControl;

    [ObservableProperty] private string _name;
    [ObservableProperty] private string _surname;
    [ObservableProperty] private string _patronymic;
    [ObservableProperty] private string _login;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _roles;

    public RegistrationViewModel(IServiceProvider serviceProvider, Navigation navigation )
    {
        _serviceProvider = serviceProvider;
        _navigation = navigation; 
    }

    [RelayCommand]
    void OpenAutorization()
    {
        var vm = _serviceProvider.GetRequiredService<AuthorizationViewModel>();
        _navigation.Navigate(vm);
    }
    
    [RelayCommand]
    void Registration()
    {
        var user = new User()
        {
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
            Login = Login,
            Password = Password,
            Role = Roles
        };

        using (var rep = _serviceProvider.GetRequiredService<UserRep>())
        {
            rep.AddUser(user);
        }
        
        var vm =  ActivatorUtilities.CreateInstance<MainWindowViewModel>(_serviceProvider);
        var win = _serviceProvider.GetRequiredService<MainWindow>();
        win.DataContext = vm;
        win.Show();
        _navigation.Close();
    }
}