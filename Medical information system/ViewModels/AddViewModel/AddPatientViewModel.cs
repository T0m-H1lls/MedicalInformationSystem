using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddPatientViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PatientRep _patientRep;

    public string FullName
    {
        get => _fullName;
        set
        {
            if (value == _fullName) return;
            _fullName = value;
            OnPropertyChanged();
        }
    }

    public DateTimeOffset DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value.Equals(_dateOfBirth)) return;
            _dateOfBirth = value;
            OnPropertyChanged();
        }
    }

    [ObservableProperty] private bool _genderM = true; 

    public string PhoneNumber
    {
        set
        {
            if (value == _phoneNumber) return;
            _phoneNumber = value;
            OnPropertyChanged();
        }
        get => _phoneNumber;
    }

    public string Address
    {
        set
        {
            if (value == _address) return;
            _address = value;
            OnPropertyChanged();
        }
        get => _address;
    }

    public string InsuranceNumber
    {
        get => _insuranceNumber;
        set
        {
            if (value == _insuranceNumber) return;
            _insuranceNumber = value;
            OnPropertyChanged();
        }
    }

    public string Passport
    {
        get => _passport;
        set
        {
            if (value == _passport) return;
            _passport = value;
            OnPropertyChanged();
        }
    }

    public string Snils
    {
        get => _snils;
        set
        {
            if (value == _snils) return;
            _snils = value;
            OnPropertyChanged();
        }
    }

    [ObservableProperty] private string _gender;

    private string _snils;
    private Action _closeAction;
    
    private string _fullName;
    private DateTimeOffset _dateOfBirth = DateTimeOffset.Now;
    private string _phoneNumber;
    private string _address;
    private string _insuranceNumber;
    private string _passport;

    public AddPatientViewModel(IServiceProvider serviceProvider,PatientRep patientRep)
    {
        _serviceProvider = serviceProvider;
        _patientRep = patientRep;
    }

    public void SetClose(Action action)
    {
        _closeAction=action;
    }

    private bool Validate(out string error)
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            error = "Введите ФИО";
            return false;
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            error = "Введите телефон";
            return false;
        }
        

        if (string.IsNullOrWhiteSpace(Address))
        {
            error = "Введите адрес";
            return false;
        }

        if (string.IsNullOrWhiteSpace(InsuranceNumber))
        {
            error = "Введите полис";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Passport))
        {
            error = "Введите паспорт";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Snils))
        {
            error = "Введите СНИЛС";
            return false;
        }
        if (DateOfBirth>DateTimeOffset.Now)
        {
            error = "Дата рождения не может быть больше текущей даты";
            return false;
        }

        error = "";
        return true;
    }
    
    [RelayCommand]
    async Task SavePatient()
    {
        if (!Validate(out var error))
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                error,
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }

        if (GenderM)
            Gender = "М";
        else
            Gender = "Ж";

        var patient = new Patient
        {
            FullName = FullName,
            BirthDate = DateOfBirth,
            Gender = Gender,
            PhoneNumber = PhoneNumber,
            Address = Address,
            InsuranceNumber = InsuranceNumber,
            Passport = Passport,
            Snils = Snils,
            DoctorId = AccountName.User.Id
        };

        _patientRep.AddPatient(patient);

        _closeAction?.Invoke();
    }
    

    [RelayCommand]
    void Cansel()
    {
        _closeAction?.Invoke();
    }

    
    
}