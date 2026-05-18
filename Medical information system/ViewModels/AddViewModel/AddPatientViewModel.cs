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
    [ObservableProperty] DateOnly? _dateOfBirth;
    [ObservableProperty] bool _genderM;
    [ObservableProperty] private string _gender;
    [ObservableProperty] string _phoneNumber;
    [ObservableProperty] string _address;
    [ObservableProperty] string _insuranceNumber;
    [ObservableProperty] string _passport;
    [ObservableProperty] string _snils;
    [ObservableProperty] private ObservableCollection<Patient> _patientsList = new();
    
    public AddPatientViewModel(IServiceProvider serviceProvider,PatientRep patientRep)
    {
        _serviceProvider = serviceProvider;
        _patientRep = patientRep;
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
        };
        _patientRep.AddPatient(patient);
        using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id));
        }
       




    }
    
    
    
}