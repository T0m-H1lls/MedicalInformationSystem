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

    private readonly Navigation _navigation;
    [ObservableProperty] private string _surname;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _role;
    [ObservableProperty] private string _patronymic;
    [ObservableProperty] private ObservableCollection<User> _usersList;
    private Action _exitAccount;


    public AccountPageViewModel(IServiceProvider serviceProvider,Navigation navigation)
    {
        _serviceProvider = serviceProvider;
        _navigation = navigation;

        using (var rep = _serviceProvider.GetRequiredService<UserRep>())
        {
             UsersList = new ObservableCollection<User>(rep.GetFullNameAndRole(AccountName.User.Login,AccountName.User.Password));
        }
       
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
        var authVm = ActivatorUtilities.CreateInstance<AuthorizationViewModel>(_serviceProvider);
        _navigation.Navigate(authVm);
    }
    
}