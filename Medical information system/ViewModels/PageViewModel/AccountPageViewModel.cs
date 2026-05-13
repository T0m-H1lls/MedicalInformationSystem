using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;

namespace Medical_information_system.ViewModels;

public partial class AccountPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountName _accountName;
    private readonly UserRep _userRep;
    [ObservableProperty] private string _surname;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _role;
    [ObservableProperty] private string _patronymic;
    [ObservableProperty] private ObservableCollection<User> _usersList;


    public AccountPageViewModel(IServiceProvider serviceProvider,AccountName accountName,UserRep userRep)
    {
        _serviceProvider = serviceProvider;
        _accountName = accountName;
        _userRep = userRep;
        
        UsersList = new ObservableCollection<User>(userRep.GetNameAndSurname(accountName.Login,accountName.Password));
        foreach (var user in UsersList)
        {
            Name = user.Name;
            Surname = user.Surname;
            Role = user.Role;
        }
    }
    
}