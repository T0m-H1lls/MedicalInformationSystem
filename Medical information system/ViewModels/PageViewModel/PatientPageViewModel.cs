using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
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

public partial class PatientPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PatientRep _patientRep;

    [ObservableProperty] private string _edFullname;
    [ObservableProperty] private DateTimeOffset? _edDateOfBirth;
    [ObservableProperty] private string _edGender;
    [ObservableProperty] private string _edPhone;
    [ObservableProperty] private string _edAdress;
    [ObservableProperty] private string _edInsuranseNumber;
    [ObservableProperty] private string _edPasport;
    [ObservableProperty] private string _edSnils;
    
    [ObservableProperty] private bool _viewStyle = false;
    [ObservableProperty] ObservableCollection<Patient> _patients;
    
    
    private Patient _selectedPatient;
    public Patient SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            if (Equals(value, _selectedPatient)) return;
            _selectedPatient = value;
            OnPropertyChanged();
            
        }
    }

    private string _searchText;
    public string SearchText
    {
        get =>_searchText;
        set
        {
           _searchText = value;
           SearchPatient();
           SwitchView();
           OnPropertyChanged(nameof(SearchText));
        }
    }

    public PatientPageViewModel(IServiceProvider serviceProvider, PatientRep patientRep,User user)
    {
        _serviceProvider = serviceProvider;
        _patientRep = patientRep;
        
        Patients = new ObservableCollection<Patient>(patientRep.GetAllPatient(AccountName.User.Id));
       
    }
    [ObservableProperty]
    private string _gender = "М";
    
    void SearchPatient()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            Patients =  new ObservableCollection<Patient>(_patientRep.GetAllPatient(AccountName.User.Id));
        }
        else
        {
            Patients = new ObservableCollection<Patient>(
                _patientRep.GetAllPatient(AccountName.User.Id).Where(s =>
                    s.FullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.PhoneNumber.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.InsuranceNumber.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
    
    
    
    [RelayCommand]
    void OpenAddPatient()
    {
        var vm =  ActivatorUtilities.CreateInstance<AddPatientViewModel>(_serviceProvider);
        var win = _serviceProvider.GetRequiredService<AddPatient>();
        win.DataContext = vm;
        win.Show();
        vm.SetClose(win.Close);
        win.Closing += WinOnClosing;
        
    }

    private void WinOnClosing(object? sender, WindowClosingEventArgs e)
    {
        Patients = new ObservableCollection<Patient>(_patientRep.GetAllPatient(AccountName.User.Id));
    }

    [RelayCommand]
    async Task DeletePatient()
    {
        if (SelectedPatient == null)
        {
            var box2 = MessageBoxManager.GetMessageBoxStandard("Ошибка","Выберите кого хотите удалить",ButtonEnum.Ok);
            var result2 = await box2.ShowAsync();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Удалить","Удалить выбранного пациента",ButtonEnum.OkCancel);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Ok)
            {
                _patientRep.DeletePatient(SelectedPatient.Id);
                Patients = new ObservableCollection<Patient>(_patientRep.GetAllPatient(AccountName.User.Id));
            }
        }
        
       
    }
    private bool IsSnilsValid()
    {
        return Regex.IsMatch(EdSnils ?? "", @"^\d{3}-\d{3}-\d{3}-\d{2}$");
    }

    private bool IsPassportValid()
    {
        return Regex.IsMatch(EdPasport ?? "", @"^\d{2}\s\d{2}\s\d{6}$");
    }
    
    private bool ValidateEdit(out string error)
    {
        if (string.IsNullOrWhiteSpace(EdFullname))
        {
            error = "Введите ФИО";
            return false;
        }
        

        

        if (string.IsNullOrWhiteSpace(EdAdress))
        {
            error = "Введите адрес";
            return false;
        }

        if (string.IsNullOrWhiteSpace(EdPasport))
        {
            error = "Введите паспорт";
            return false;
        }

        if (string.IsNullOrWhiteSpace(EdSnils))
        {
            error = "Введите СНИЛС";
            return false;
        }
        
        if (EdDateOfBirth > DateTimeOffset.Now)
        {
            error = "Дата рождения не может быть больше текущей даты";
            return false;
        }

        error = "";
        return true;
    }
  
    [RelayCommand]
    async Task SwitchView()
    {
        if (SelectedPatient == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Выберите пациента",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }

        EdFullname = SelectedPatient.FullName;
        EdDateOfBirth = SelectedPatient.BirthDate;
        Gender = SelectedPatient.Gender;
        EdPhone = SelectedPatient.PhoneNumber;
        EdAdress = SelectedPatient.Address;
        EdInsuranseNumber = SelectedPatient.InsuranceNumber;
        EdPasport = SelectedPatient.Passport;
        EdSnils = SelectedPatient.Snils;

        ViewStyle = true;
    }
    
    [RelayCommand]
    void ClosePanel()
    {
        ViewStyle = false;
    }

    [RelayCommand]
    async Task SaveEdit()
    {
        if (SelectedPatient == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Выберите пациента",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }

        if (!ValidateEdit(out string error))
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", error,
                ButtonEnum.Ok);

            await errorBox.ShowAsync();

            return;
        }

        var confirmBox = MessageBoxManager.GetMessageBoxStandard(
            "Редактирование", "Сохранить изменения?",
            ButtonEnum.OkCancel);

        var result = await confirmBox.ShowAsync();

        if (result == ButtonResult.Ok)
        {
            SelectedPatient.FullName = EdFullname;
            SelectedPatient.BirthDate = EdDateOfBirth.Value;
            SelectedPatient.Gender = Gender;
            SelectedPatient.PhoneNumber = EdPhone;
            SelectedPatient.Address = EdAdress;
            SelectedPatient.InsuranceNumber = EdInsuranseNumber;
            SelectedPatient.Passport = EdPasport;
            SelectedPatient.Snils = EdSnils;

            _patientRep.UpdatePatient(SelectedPatient);

            Patients = new ObservableCollection<Patient>(
                _patientRep.GetAllPatient(AccountName.User.Id));

            ViewStyle = false;

            var successBox = MessageBoxManager.GetMessageBoxStandard(
                "Успех", "Пациент успешно изменен",
                ButtonEnum.Ok);

            await successBox.ShowAsync();
        }
    }

}
