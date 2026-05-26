using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.ViewModels.AddViewModel;
using Medical_information_system.Views.Add;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Medical_information_system.ViewModels;

public partial class ApoitmentsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ApointmentRep _apointmentRep;
    private Action _closeAction;
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList = new();
    [ObservableProperty] private string _fullNameDoc;
    [ObservableProperty] private string _fullNameDocRef;
    [ObservableProperty] private Appointments _selectedAppointment;
    private string _searchText;
    public string SearchText
    {
        get =>_searchText;
        set
        {
            _searchText = value;
            SearchAppointment();
            OnPropertyChanged(nameof(SearchText));
        }
    }

    public ApoitmentsPageViewModel(IServiceProvider serviceProvider,ApointmentRep apointmentRep)
    {
        _serviceProvider = serviceProvider;
        _apointmentRep = apointmentRep;
        using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments());
        }
    }
    
    void SearchAppointment()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            AppointmentsList = new ObservableCollection<Appointments>(_apointmentRep.GetAppointments());
        }
        else
        {
            AppointmentsList = new ObservableCollection<Appointments>(
                _apointmentRep.GetAppointments().Where(s =>
                    s.DoctorFullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Status.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }

    public void SetClose(Action action)
    {
        _closeAction = action;
    }
    
    [RelayCommand]
    void OpenAddAppointment()
    {
        bool flag = false;
        var vm =  ActivatorUtilities.CreateInstance<AddApoitmentViewModel>(_serviceProvider);
        var win = _serviceProvider.GetRequiredService<AddApoitment>();
        win.DataContext = vm;
        win.Show();
        vm.SetClose(win.Close);
        win.Closing += WinOnClosing;
    }

    private void WinOnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppointmentsList = new ObservableCollection<Appointments>(_apointmentRep.GetAppointments());
    }

    [RelayCommand]
    void DeleteAppointment()
    {
        async Task DeletePatient()
        {
            if (SelectedAppointment == null)
            {
                var box2 = MessageBoxManager.GetMessageBoxStandard("Ошибка","Выберите что хотите удалить",ButtonEnum.Ok);
                var result2 = await box2.ShowAsync();
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Удалить","Удалить выбранную запись",ButtonEnum.OkCancel);
                var result = await box.ShowAsync();
                if (result == ButtonResult.Ok)
                {
                    _apointmentRep.DeleteAppointment(SelectedAppointment.Id);
                    AppointmentsList = new ObservableCollection<Appointments>(_apointmentRep.GetAppointments());
                }
            }
        }
    }

    [RelayCommand]
    void UpdateAppointment()
    {
        
    }
}