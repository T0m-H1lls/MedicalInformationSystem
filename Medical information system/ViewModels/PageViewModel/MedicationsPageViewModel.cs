using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class MedicationsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<Medication> _medicationList;

    public MedicationsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<MedicationRep>())
        {
            MedicationList = new ObservableCollection<Medication>(rep.GetMedication());
        }
    }
    
    
}