using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class DiagnosPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiagnoseRep _diagnoseRep;
    [ObservableProperty] ObservableCollection<Diagnose> _diagnoseList = new();
    
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                SearchDiagnose();
                OnPropertyChanged(nameof(SearchText));
            }
        }
    }

    public DiagnosPageViewModel(IServiceProvider serviceProvider,DiagnoseRep diagnoseRep)
    {
        _serviceProvider = serviceProvider;
        _diagnoseRep = diagnoseRep;
        DiagnoseList = new ObservableCollection<Diagnose>(_diagnoseRep.GetDiagnoses());
        /*
        using (var rep = _serviceProvider.GetRequiredService<DiagnoseRep>())
        {
            DiagnoseList = new ObservableCollection<Diagnose>(rep.GetDiagnoses());
        }*/
    }

    void SearchDiagnose()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            DiagnoseList = new ObservableCollection<Diagnose>(_diagnoseRep.GetDiagnoses());
        }
        else
        {
            DiagnoseList = new ObservableCollection<Diagnose>(
                _diagnoseRep.GetDiagnoses().Where(s=>
                    s.Name.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Description.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
    
}