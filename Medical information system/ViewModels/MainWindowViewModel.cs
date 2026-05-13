using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Medical_information_system.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    
    private readonly IServiceProvider _serviceProvider;
    private readonly PatientPageViewModel _patientPageViewModel;
    private readonly PatientRep _patientRep;
    private readonly UserRep _userRep;
    private readonly AccountName _accountName;
    private Action _closeAction;

    [ObservableProperty] private bool _isPaneOpen = true;
    [ObservableProperty] private ListItemTemplate? _selectedListItem;
    [ObservableProperty] private ViewModelBase _currentPage;
    [ObservableProperty] private ObservableCollection<User> _usersList = new();

    [ObservableProperty] private string _docName;
    [ObservableProperty] private string _docSurname;
    [ObservableProperty] private string _role;
    
    
    public MainWindowViewModel(IServiceProvider serviceProvider, PatientPageViewModel patientPageViewModel,PatientRep patientRep,UserRep userRep,AccountName accountName)
    {
        _serviceProvider = serviceProvider;
        _patientPageViewModel = patientPageViewModel;
        _patientRep = patientRep;
        _userRep = userRep;
        _accountName = accountName;
        CurrentPage = new PatientPageViewModel(serviceProvider,patientRep);
        
        UsersList = new ObservableCollection<User>(userRep.GetNameAndSurname(accountName.Login,accountName.Password));
        foreach (var user in UsersList)
        {
            DocName = user.Name;
            DocSurname = user.Surname;
            Role = user.Role;
        }
        
    }

    public ObservableCollection<ListItemTemplate> ListItemTemplates { get; } = new()
    {
        new ListItemTemplate(typeof(PatientPageViewModel), "person_regular", "Пациенты"),
        new ListItemTemplate(typeof(DoctorsPageViewModel), "doctors_d", "Врачи"),
        new ListItemTemplate(typeof(ApoitmentsPageViewModel), "calendar_month_regular", "Приемы"),
        new ListItemTemplate(typeof(MedicalRecordsPageViewModel), "reading_mode_mobile_regular", "Медицинские записи"),
        new ListItemTemplate(typeof(DiagnosPageViewModel), "diagnose", "Диагнозы"),
        new ListItemTemplate(typeof(PrescriptionsPageViewModel), "naznachenie", "Назначения"),
        new ListItemTemplate(typeof(MedicationsPageViewModel), "medical", "Лекарства"),
        new ListItemTemplate(typeof(AccountPageViewModel),"person_regular","Профиль")

    };

  

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null)
            return;
        var vm = ActivatorUtilities.CreateInstance(_serviceProvider, value.ModelType);
        if (vm is null)
            return;
        CurrentPage = (ViewModelBase)vm;
    }


  
    
    public void SetCloseAction(Action action)
    {
        _closeAction = action;
    }
    
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    [RelayCommand]
    async Task CloseProgram()
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Выход","Выйти из программы?",ButtonEnum.OkCancel);
        var result = await box.ShowAsync();
        if (result == ButtonResult.Ok)
        {
            _closeAction?.Invoke();
        }
       
    }
    
}
public class ListItemTemplate
{
    public ListItemTemplate(Type type,string icon, string label = null)
    {
        ModelType = type;
        Label = label??type.Name.Replace("PageViewModel", "");
        Application.Current.TryFindResource(icon, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }
    public string Label { get; set; }
    public Type ModelType  { get; set; }
    public StreamGeometry ListItemIcon { get; set; }
}