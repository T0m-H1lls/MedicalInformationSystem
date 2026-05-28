using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    [ObservableProperty] private ObservableCollection<Patient> _patientsList;
    [ObservableProperty] private ObservableCollection<Status> _statusList;
    [ObservableProperty] private ObservableCollection<Doctor> _doctorsList;
    [ObservableProperty] private ObservableCollection<MedicalRecord> _medicalRecordList;
    
    [ObservableProperty] private Patient _selectedAppointmentPatient;
    [ObservableProperty] private Status _selectedAppointmentStatus;
    [ObservableProperty] private DateTimeOffset? _selectedDate = DateTimeOffset.Now;
    [ObservableProperty] private Doctor? _selectedDoctor;
    [ObservableProperty] private Doctor? _selectedReferallDoctor;

    public AddApoitmentViewModel(IServiceProvider serviceProvider,ApointmentRep apointmentRep)
    {
        _serviceProvider = serviceProvider;
        _apointmentRep = apointmentRep;
        
        
        AppointmentsList = new ObservableCollection<Appointments>(_apointmentRep.GetAppointments(AccountName.User.Id));
        
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id));
        }

        SelectedAppointmentPatient = PatientsList.FirstOrDefault();
        
        using (var rep = serviceProvider.GetRequiredService<StatusRep>())
        {
            StatusList = new ObservableCollection<Status>(rep.StatusList());
        }

        SelectedAppointmentStatus = StatusList.FirstOrDefault();
        
        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
    }
    
    public void SetClose(Action action)
    {
        _closeAction=action;
    }

    [RelayCommand]
    void SaveApointment()
    {
        var res = new Appointments
        {
            PatientId = SelectedAppointmentPatient.Id,
            AppointmentDate = SelectedDate,
            StatusId = SelectedAppointmentStatus.Id,
            DoctorId = AccountName.User.Id,
            ReferralDoctorId = SelectedReferallDoctor?.Id
        };

        _apointmentRep.AddAppointment(res);
        _closeAction?.Invoke();

    }
    [RelayCommand]
    void ClearReferall()
    {
        SelectedReferallDoctor = null;
    }

    [RelayCommand]
    void Cansel()
    {
        _closeAction?.Invoke();
    }

}