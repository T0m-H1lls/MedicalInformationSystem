using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_information_system.ViewModels;

public partial class DoctorsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DoctorRep _doctorRep;
    [ObservableProperty] ObservableCollection<Doctor> _doctorsList = new();
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            SearchDoctors();
            OnPropertyChanged(nameof(SearchText));
        }
    }

    public DoctorsPageViewModel(IServiceProvider serviceProvider,DoctorRep doctorRep)
    {
        _serviceProvider = serviceProvider;
        _doctorRep = doctorRep;
        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
    }
    private void SearchDoctors()
    { 
        if (string.IsNullOrWhiteSpace(SearchText))
        {
           DoctorsList= new ObservableCollection<Doctor>(_doctorRep.GetDoctors());
        }
        else
        {
            DoctorsList= new ObservableCollection<Doctor>(
                _doctorRep.GetDoctors().Where(s =>
                    s.FullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.DepartmentName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.PhoneNumber.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Speciality.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Room.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}