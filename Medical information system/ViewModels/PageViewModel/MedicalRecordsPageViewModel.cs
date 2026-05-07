using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class MedicalRecordsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<MedicalRecord> _medicalRecordsList = new();

    public MedicalRecordsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<MedicalRecordRep>())
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
        }
    }
}