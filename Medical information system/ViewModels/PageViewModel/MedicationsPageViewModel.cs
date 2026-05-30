using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class MedicationsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<Medication> _medicationList;
    [ObservableProperty] private bool _viewStyle = false; 
    
    [RelayCommand]
    void ClosePanel()
    {
    
        ViewStyle = false;
        
    }
    
    private string _searchText;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            SearchMedications();
            OnPropertyChanged(nameof(SearchMedications));
        }
    }
    

    public MedicationsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<MedicationRep>())
        {
            MedicationList = new ObservableCollection<Medication>(rep.GetMedication());
        }
    }
    private void SearchMedications()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            using (var rep = _serviceProvider.GetRequiredService<MedicationRep>())
            {
                MedicationList = new ObservableCollection<Medication>(rep.GetMedication());
            }
        }
        else
        {
            using (var rep = _serviceProvider.GetRequiredService<MedicationRep>())
            {
                 MedicationList = new ObservableCollection<Medication>(
                                rep.GetMedication().Where(s =>
                                    s.Name.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                    s.Description.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                    s.Manufacturer.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
            }

           

        }
    }

    [RelayCommand]
    void UpdateMedication()
    {
        
    }
    [RelayCommand]
    void Save()
    {
        
    }
}