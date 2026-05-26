using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;
using Tmds.DBus.Protocol;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddApoitmentViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ApointmentRep _apointmentRep;
    private Action _closeAction;
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList;
    [ObservableProperty] private Patient _selectedAppointmentPatient;
    [ObservableProperty] private Status _selectedAppointmentStatus;
    [ObservableProperty] private DateTimeOffset? _selectedDate;
    [ObservableProperty] private MedicalRecord? _selectedMedicalRecord;

    public AddApoitmentViewModel(IServiceProvider serviceProvider,ApointmentRep apointmentRep)
    {
        _serviceProvider = serviceProvider;
        _apointmentRep = apointmentRep;

        AppointmentsList = new ObservableCollection<Appointments>(_apointmentRep.GetAppointments());
    }
    
    
    public void SetClose(Action action)
    {
        _closeAction=action;
    }

    [RelayCommand]
    void SaveApointment()
    {
        var res = new Appointments()
        {
            
            PatientId = _selectedAppointmentPatient.Id,
            AppointmentDate = SelectedDate,
            StatusId = SelectedAppointmentStatus.Id,
            DoctorId = AccountName.User.Id,
            MedicalRecordID = SelectedMedicalRecord?.Id
        };

        _apointmentRep.AddAppointment(res);

    }

    [RelayCommand]
    void Cansel()
    {
        _closeAction?.Invoke();
    }

}