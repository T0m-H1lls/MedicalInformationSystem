using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    
    private readonly IServiceProvider _serviceProvider;
    
    [ObservableProperty] private bool _isPaneOpen = true;
    [ObservableProperty] private ListItemTemplate? _selectedListItem;
    [ObservableProperty] private ViewModelBase _currentPage = new PatientPageViewModel();
    
    public ObservableCollection<ListItemTemplate> ListItemTemplates { get; } = new()
    {
        new ListItemTemplate(typeof(PatientPageViewModel),"person_regular","Пациенты"),
        new ListItemTemplate(typeof(DoctorsPageViewModel),"doctors_d ","Врачи"),
        new ListItemTemplate(typeof(ApoitmentsPageViewModel),"calendar_month_regular ","Приемы"),
        new ListItemTemplate(typeof(MedicalRecordsPageViewModel)," reading_mode_mobile_regular","Медицинские записи"),
        new ListItemTemplate(typeof(DiagnosPageViewModel),"diagnose ","Диагнозы"),
        new ListItemTemplate(typeof(PrescriptionsPageViewModel)," ","Назначения"),
        new ListItemTemplate(typeof(MedicationsPageViewModel),"medical ","Лекарства"),
        
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


    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
       
    }
    
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}
public class ListItemTemplate
{
    public ListItemTemplate(Type type,string Icon, string label = null)
    {
        ModelType = type;
        Label = label??type.Name.Replace("PageViewModel", "");
        Application.Current.TryFindResource(Icon, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }
    public string Label { get; set; }
    public Type ModelType  { get; set; }
    public StreamGeometry ListItemIcon { get; set; }
}