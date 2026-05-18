using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class ApoitmentsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ApointmentRep _apointmentRep;
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList = new();
    [ObservableProperty] private string _fullNameDoc;
    [ObservableProperty] private string _fullNameDocRef;
    private string _searchText;
    public string SearchText
    {
        get =>_searchText;
        set
        {
            _searchText = value;
            SearchAppointment();
            OnPropertyChanged(nameof(SearchText));
        }
    }

    public ApoitmentsPageViewModel(IServiceProvider serviceProvider,ApointmentRep apointmentRep)
    {
        _serviceProvider = serviceProvider;
        _apointmentRep = apointmentRep;
        using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments());
        }
    }
    
    void SearchAppointment()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            AppointmentsList = new ObservableCollection<Appointments>(_apointmentRep.GetAppointments());
        }
        else
        {
            AppointmentsList = new ObservableCollection<Appointments>(
                _apointmentRep.GetAppointments().Where(s =>
                    s.DoctorFullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Status.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
    
}