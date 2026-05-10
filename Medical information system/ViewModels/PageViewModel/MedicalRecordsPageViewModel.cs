using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class MedicalRecordsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MedicalRecordRep _medicalRecordRep;
    [ObservableProperty] ObservableCollection<MedicalRecord> _medicalRecordsList = new();
    
    private string _searchText;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            SearchMedicalRecord();
            OnPropertyChanged(nameof(SearchMedicalRecord));
        }
    }

   


    public MedicalRecordsPageViewModel(IServiceProvider serviceProvider,MedicalRecordRep medicalRecordRep)
    {
        _serviceProvider = serviceProvider;
        _medicalRecordRep = medicalRecordRep;
        MedicalRecordsList = new ObservableCollection<MedicalRecord>(_medicalRecordRep.GetMedicalRecords());
    }
    
    private void SearchMedicalRecord()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(_medicalRecordRep.GetMedicalRecords());
        }
        else
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(
                _medicalRecordRep.GetMedicalRecords().Where(s =>
                    s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.DoctorName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.MedicineName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.DiagnoseName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}