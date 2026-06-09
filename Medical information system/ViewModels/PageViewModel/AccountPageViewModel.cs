using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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
    private Action _closeMainWindow;


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
    
    public void SetCloseAction(Action closeAction)
    {
        _closeMainWindow = closeAction;
    }


    [RelayCommand]
    async Task ExitAccount()
    {
        var authWindow = _serviceProvider.GetRequiredService<AuthorizationView>();

        var authVm = ActivatorUtilities.CreateInstance<AuthorizationViewModel>(_serviceProvider);

        authWindow.DataContext = authVm;
        var box = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Вы действительно хотите выйти из аккаунта?",
            ButtonEnum.OkCancel);
        var result = await box.ShowAsync();
        if (result == ButtonResult.Ok)
        {
            authVm.SetCloseAction(() => { authWindow.Close(); });

            authWindow.Show();

            _closeMainWindow?.Invoke();
        }
    }

}