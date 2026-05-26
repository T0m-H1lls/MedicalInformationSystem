using System;
using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class AccountPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UserRep _userRep;
    private readonly Navigation _navigation;
    [ObservableProperty] private string _surname;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _role;
    [ObservableProperty] private string _patronymic;
    [ObservableProperty] private ObservableCollection<User> _usersList;
    private Action _exitAccount;


    public AccountPageViewModel(IServiceProvider serviceProvider,UserRep userRep,Navigation navigation)
    {
        _serviceProvider = serviceProvider;
        _userRep = userRep;
        _navigation = navigation;

        UsersList = new ObservableCollection<User>(userRep.GetFullNameAndRole(AccountName.User.Login,AccountName.User.Password));
        foreach (var user in UsersList)
        {
            
            Name = user.Name;
            Surname = user.Surname;
            Role = user.Role;
            Patronymic = user.Patronymic;
            
        }
    }
    

    [RelayCommand]
    void ExitAccount()
    {
        var vm = ActivatorUtilities.CreateInstance<AuthorizationViewModel>(_serviceProvider);
        _navigation.Navigate(vm);
    }
    
}