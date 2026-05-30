using System;
using System.Collections.Generic;
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
    
    [ObservableProperty] private int _currentPageSize;
    [ObservableProperty] List<int> pageSizes;
    [ObservableProperty]private string pageInfo;
    private int currentPage = 1;
    private int totalPages;
    
    
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
           OnPropertyChanged(nameof(SearchText));
        }
    }

    public PatientPageViewModel(IServiceProvider serviceProvider, User user)
    {
        _serviceProvider = serviceProvider;
        PageSizes = new List<int>([5,10,20]);
        CurrentPageSize = PageSizes.First();
        using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
        {
            Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id,currentPage,CurrentPageSize));
        }
       
        
       
    }
    
    partial void OnCurrentPageSizeChanged(int value)
    {
        CalculatePages();
    }

    void CalculatePages()
    {
        using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
        {
             var rowsCount = rep.GetRowsCount(AccountName.User.Id);
             totalPages = (int)Math.Ceiling(((double)rowsCount / CurrentPageSize));
             
             currentPage = 1;
             ShowPage(currentPage);
        }
       
    }

    void ShowPage(int pageIndex)
    {
        currentPage = pageIndex;

        using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
        {
            Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id, pageIndex, CurrentPageSize));
            PageInfo = $"Страница {currentPage} из {totalPages}";
        }
    }

    [RelayCommand]
    private void ShowFirstPage()
    {
        ShowPage(1);
    }
    
    [RelayCommand]
    private void ShowLastPage()
    {
        ShowPage(totalPages);
    }

    [RelayCommand]
    private void ShowNextPage()
    {
        if (currentPage < totalPages)
            ShowPage(currentPage + 1);
    }
    
    [RelayCommand]
    private void ShowPrevPage()
    {
        if (currentPage > 1)
            ShowPage(currentPage - 1);
    }
    [ObservableProperty]
    private string _gender = "М";
    
    void SearchPatient()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
            {
                Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id,currentPage,CurrentPageSize));

            }
        }
        else
        {
            using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
            {
                Patients = new ObservableCollection<Patient>(
                    rep.GetAllPatient(AccountName.User.Id,currentPage,CurrentPageSize).Where(s =>
                        s.FullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                        s.PhoneNumber.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                        s.InsuranceNumber.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
                   
            }
           
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
        using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
        {
            Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id,currentPage,CurrentPageSize));

        }
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
                using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
                {
                      rep.DeletePatient(SelectedPatient.Id);

                }
              
                using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
                {
                    Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id,currentPage,CurrentPageSize));

                }
            }
        }
        
       
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

            using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
            {
                
                rep.UpdatePatient(SelectedPatient);

            }
            
            using (var rep = _serviceProvider.GetRequiredService<PatientRep>())
            {
                Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id,currentPage,CurrentPageSize));

            }

            ViewStyle = false;

            var successBox = MessageBoxManager.GetMessageBoxStandard(
                "Успех", "Пациент успешно изменен",
                ButtonEnum.Ok);

            await successBox.ShowAsync();
        }
    }

}
