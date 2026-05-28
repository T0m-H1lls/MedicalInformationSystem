using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class PrescriptionsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PrescriptionRep _prescriptionRep;
    [ObservableProperty] ObservableCollection<Prescription> _prescriptionsList = new();
    
    private string _searchText;
    public string SearchText
    {
        get =>_searchText;
        set
        {
            _searchText = value;
            SearchPrescreption();
            OnPropertyChanged(nameof(SearchText));
        }
    }

    void SearchPrescreption()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            PrescriptionsList = new ObservableCollection<Prescription>(_prescriptionRep.GetPrescriptions());
        }
        else
        {
            PrescriptionsList = new ObservableCollection<Prescription>(
                _prescriptionRep.GetPrescriptions().Where(s =>
                    s.DoctorName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Dosage.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Duration.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.MedicalName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));

        }
    }

    public PrescriptionsPageViewModel(IServiceProvider serviceProvider, PrescriptionRep prescriptionRep)
    {
        _serviceProvider = serviceProvider;
        _prescriptionRep = prescriptionRep;
        using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
        {
            PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions());
        }
    }
}