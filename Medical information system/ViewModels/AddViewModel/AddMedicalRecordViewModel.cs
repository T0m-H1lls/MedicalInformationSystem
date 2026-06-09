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

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddMedicalRecordViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] private string _medicine;
    [ObservableProperty] private string _diagnose;
    [ObservableProperty] private string _description;
    private Action _closeAction;
    
    [ObservableProperty] private DateTimeOffset _recordDate = DateTime.Now;
    [ObservableProperty] private TimeSpan _recordTime = DateTime.Now.TimeOfDay;
    
    public DateTime RecordDateTime => RecordDate.Date + RecordTime;
    
    
    [ObservableProperty] private ObservableCollection<MedicalRecord> _medicalRecordsList;
    [ObservableProperty] private ObservableCollection<Patient> _patientsList;
    
    [ObservableProperty] private MedicalRecord _selectedMedicalRecord;
    [ObservableProperty] private Patient _selectedPatient;
    
    public AddMedicalRecordViewModel(IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.DoctorId));
        }
        SelectedPatient = PatientsList.FirstOrDefault();
    }
    
    public void SetClose(Action action)
    {
        _closeAction = action;
    }

    private bool Validate(out string error)
    {

        if (string.IsNullOrWhiteSpace(Medicine))
        {
            error = "Введите лекарство";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Diagnose))
        {
            error = "Введите диагноз";
            return false;
        }
        if (SelectedPatient == null)
        {
            error = "Выберите пациента";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            error = "Введите описание";
            return false;
        }
        if (RecordDate > DateTime.Now)
        {
            error = "Введеная дата не может быть больше текущей";
            return false;
        }

        error = "";
        return true;
    }

    [RelayCommand]
    async Task SavePrescription()
    {
        if (!Validate(out string error))
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", error, ButtonEnum.Ok).ShowAsync();
            return;
        }

        var medrecord = new MedicalRecord()
        {
            MedicineName = Medicine,
            Description = Description,
            RecordDate = RecordDateTime,
            PatientId = SelectedPatient.Id,
            DoctorId = AccountName.User.DoctorId,
            DiagnoseName = Diagnose
        };

        using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
        {
             rep.AddMedicalRecords(medrecord);
        }

       
        _closeAction?.Invoke();
    }

    [RelayCommand]
    void Cancel()
    {
        _closeAction?.Invoke();
    }
}