using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class PatientPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PatientRep _patientRep;

    [ObservableProperty] ObservableCollection<Patient> _patients;
    [ObservableProperty] private Patient _selectedPatient;
    [ObservableProperty] private string _searchText;
    
    public PatientPageViewModel(IServiceProvider serviceProvider, PatientRep patientRep)
    {
        _serviceProvider = serviceProvider;
        _patientRep = patientRep;
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            Patients = new ObservableCollection<Patient>(rep.GetAllPatient());
        }
    }
    
    
    [RelayCommand]
    void SearchPatient()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            Patients =  new ObservableCollection<Patient>(_patientRep.GetAllPatient());
        }
        else
        {
            Patients = new ObservableCollection<Patient>(
                _patientRep.GetAllPatient().Where(s=>
                    s.FullName.Contains(SearchText,StringComparison.CurrentCultureIgnoreCase)));
        }
    }
    
    
    
    
}