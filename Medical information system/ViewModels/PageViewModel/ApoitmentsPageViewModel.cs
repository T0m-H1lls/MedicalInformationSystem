using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class ApoitmentsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList = new();

    public ApoitmentsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using (var rep = _serviceProvider.GetRequiredService<AppointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments());
        }
    }
    
}