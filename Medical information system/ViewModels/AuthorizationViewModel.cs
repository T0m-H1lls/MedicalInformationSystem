using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class AuthorizationViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Navigation _navigation;
    private readonly User _users;

    [ObservableProperty] private ObservableCollection<User> _userList;
    [ObservableProperty] private string _usLogin;
    [ObservableProperty]private string _usPassword;
    [ObservableProperty] private bool _flag =  false;
    [ObservableProperty] private string _error;
    [ObservableProperty] private DispatcherTimer _timer;
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

    [RelayCommand]
    void Authorizations()
    {
        Flag = false;
        using (var rep = _serviceProvider.GetRequiredService<UserRep>())
        {
            UserList = new ObservableCollection<User>(rep.CheckUser(UsLogin,UsPassword));
        }
        foreach (var us in UserList)
        {
            if (us.Login == UsLogin & us.Password == UsPassword)
            {
                Flag = true;
            }
            else
            {
                Flag = false;
            }
        }


        if (Flag == true) {
            var vm = ActivatorUtilities.CreateInstance<MainWindowViewModel>(_serviceProvider);
            var win = _serviceProvider.GetRequiredService<MainWindow>();
            win.DataContext = vm;
            Error = "Вход выполнен";
            win.Show();
            _navigation.Close();
        }
        else {
            Error = "Неверный Логин или Пароль";
        }
        
    }
}