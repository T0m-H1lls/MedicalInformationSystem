using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Tmds.DBus.Protocol;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddApoitmentViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private Action _closeAction;
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList;
    [ObservableProperty] private ObservableCollection<Patient> _patientsList;
    [ObservableProperty] private ObservableCollection<Status> _statusList;
    [ObservableProperty] private ObservableCollection<Doctor> _doctorsList;
    [ObservableProperty] private ObservableCollection<MedicalRecord> _medicalRecordList;
    
    [ObservableProperty] private DateTimeOffset _recordDate = DateTime.Now;
    [ObservableProperty] private TimeSpan _recordTime = DateTime.Now.TimeOfDay;
    
    public DateTime RecordDateTime => RecordDate.Date + RecordTime;
    
    [ObservableProperty] private Patient _selectedAppointmentPatient;
    [ObservableProperty] private Status _selectedAppointmentStatus;
    [ObservableProperty] private Doctor? _selectedDoctor;
    [ObservableProperty] private Doctor? _selectedReferallDoctor;

    public AddApoitmentViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        bool isChiefDoctor = AccountName.User.Role == "Главный врач";
       
        
        using (var rep = serviceProvider.GetRequiredService<ApointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.DoctorId,isChiefDoctor));
        }
        
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.DoctorId,isChiefDoctor));
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
    async Task SaveApointment()
    {
      
        var res = new Appointments
        {
            PatientId = SelectedAppointmentPatient.Id,
            AppointmentDate = RecordDateTime,
            StatusId = SelectedAppointmentStatus.Id,
            DoctorId = AccountName.User.DoctorId,
            ReferralDoctorId = SelectedReferallDoctor?.Id
        };

        using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
        {
             rep.AddAppointment(res);
        }
       
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