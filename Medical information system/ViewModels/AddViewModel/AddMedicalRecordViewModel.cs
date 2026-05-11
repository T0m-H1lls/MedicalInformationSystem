using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddMedicalRecordViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] private string _medicine;
    [ObservableProperty] private string _diagnose;
    [ObservableProperty] private string _description;
    [ObservableProperty] private DateTime _recordDate; 
    [ObservableProperty] private ObservableCollection<MedicalRecord> _medicalRecordsList;
    
    
    public AddMedicalRecordViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<MedicalRecordRep>())
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
        }
    }
}