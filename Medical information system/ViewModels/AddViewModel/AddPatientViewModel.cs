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

    public bool GenderM { set; get; }

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
    private DateTimeOffset _dateOfBirth;
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

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(FullName))
            return false;

        if (string.IsNullOrWhiteSpace(PhoneNumber))
            return false;

        if (PhoneNumber.Length < 11)
            return false;

        if (string.IsNullOrWhiteSpace(Address))
            return false;

        if (string.IsNullOrWhiteSpace(InsuranceNumber))
            return false;

        if (string.IsNullOrWhiteSpace(Passport))
            return false;

        if (string.IsNullOrWhiteSpace(Snils))
            return false;

        return true;
    }
    
    [RelayCommand]
    async Task SavePatient()
    {
        if (!Validate())
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                "Заполните все поля корректно",
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