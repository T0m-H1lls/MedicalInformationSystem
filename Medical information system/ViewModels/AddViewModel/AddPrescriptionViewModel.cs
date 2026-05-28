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

public partial class AddPrescriptionViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PrescriptionRep _prescriptionRep;
    private Action _closeAction;
    [ObservableProperty] private ObservableCollection<Patient> _patientsList = new();
    [ObservableProperty] private ObservableCollection<Medication> _medicinesList = new();
    [ObservableProperty] private Patient _selectedPatient;
    [ObservableProperty] private string _dosage;
    [ObservableProperty] private string _duration;
    [ObservableProperty] private string _medicine;

    public AddPrescriptionViewModel(IServiceProvider serviceProvider, PrescriptionRep prescriptionRep)
    {
        _serviceProvider = serviceProvider;
        _prescriptionRep = prescriptionRep;

        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id)); 
        }
        SelectedPatient = PatientsList.FirstOrDefault();
        

    }

    public void SetClose(Action action)
    {
        _closeAction = action;
    }

    private bool Validate(out string error)
    {
        if (SelectedPatient.Id == null)
        {
            error = "Выберите пациента";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Medicine))
        {
            error = "Введите лекарство";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Dosage))
        {
            error = "Введите дозировку";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Duration))
        {
            error = "Введите длительность";
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

        var prescription = new Prescription
        {
            DoctorId = AccountName.User.Id,
            PatientId = SelectedPatient.Id,
            MedicalName = Medicine,
            Dosage = Dosage,
            Duration = Duration
        };

        bool result = _prescriptionRep.AddPrescription(prescription);

        if (result)
        {
            await MessageBoxManager
                .GetMessageBoxStandard(
                    "Успех",
                    "Рецепт успешно добавлен",
                    ButtonEnum.Ok)
                .ShowAsync();

            _closeAction?.Invoke();
        }
        else
        {
           return;
        }
    }

    [RelayCommand]
    void Cancel()
    {
        _closeAction?.Invoke();
    }
}