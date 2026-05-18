using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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
    private readonly User _user;

    [ObservableProperty] private bool _viewStyle = false;
    
    [ObservableProperty] ObservableCollection<Patient> _patients;
    [ObservableProperty] private Patient _selectedPatient;

   

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


    public PatientPageViewModel(IServiceProvider serviceProvider, PatientRep patientRep,User user)
    {
        _serviceProvider = serviceProvider;
        _patientRep = patientRep;
        _user = user;
       
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            Patients = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id));
        }
    }
    
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

    [RelayCommand]
    void UpdatePatient()
    {
        
    }
    
    private Classes view;
    public void SetView(Classes view)
    {
        this.view = view;
    }
    private Classes viewClose;
    public void SetViewClose(Classes viewClose)
    {
        this.viewClose = viewClose;
    }
    
}
