using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class DiagnosPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<Diagnose> _diagnoseList = new();

    public DiagnosPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = _serviceProvider.GetRequiredService<DiagnoseRep>())
        {
            DiagnoseList = new ObservableCollection<Diagnose>(rep.GetDiagnoses());
        }
    }
}