using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class DoctorsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<Doctor> _doctors = new();

    public DoctorsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            Doctors = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
    }
}