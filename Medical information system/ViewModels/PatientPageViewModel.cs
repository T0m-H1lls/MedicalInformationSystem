using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class PatientPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty] ObservableCollection<Patient> _patients;
    
    public PatientPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            Patients = new ObservableCollection<Patient>(rep.GetAllPatient());
        }
    }
    
    
    
}