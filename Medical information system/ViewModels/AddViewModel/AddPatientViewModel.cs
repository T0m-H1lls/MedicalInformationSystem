using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddPatientViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PatientRep _patientRep;

    [ObservableProperty] string _fullName;
    [ObservableProperty] DateTimeOffset? _dateOfBirth;
    [ObservableProperty] bool _genderM;
    [ObservableProperty] private string _gender;
    [ObservableProperty] string _phoneNumber;
    [ObservableProperty] string _address;
    [ObservableProperty] string _insuranceNumber;
    [ObservableProperty] string _passport;
    [ObservableProperty] string _snils;
    private Action _closeAction;
    [ObservableProperty] private ObservableCollection<Patient> _patientsList;
    
    public AddPatientViewModel(IServiceProvider serviceProvider,PatientRep patientRep)
    {
        _serviceProvider = serviceProvider;
        _patientRep = patientRep;
    }

    public void SetClose(Action action)
    {
        _closeAction=action;
    }

    [RelayCommand]
    void SavePatient()
    {
        if (GenderM == true)
        {
            Gender = "М";
        }
        else
        {   
            Gender = "Ж";
        }
        
        var patient = new Patient
        {
            FullName = FullName,
            DateOfBirth = DateOfBirth,
            Gender = Gender ,
            PhoneNumber = PhoneNumber,
            Address = Address,
            InsuranceNumber = InsuranceNumber,
            Passport = Passport,
            Snils = Snils,
            DoctorId = AccountName.User.Id
        };
        _patientRep.AddPatient(patient);
        PatientsList = new ObservableCollection<Patient>(_patientRep.GetAllPatient(AccountName.User.Id));
        _closeAction?.Invoke();
       
    }

    [RelayCommand]
    void Cansel()
    {
        _closeAction?.Invoke();
    }

    
    
}