using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddDoctorViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private Action _closeAction;
    
    public void SetClose(Action action)
    {
        _closeAction = action;
    }

    [ObservableProperty] private string _adFullName;
    [ObservableProperty] private string _adPhone;
    [ObservableProperty] private string _adRoom;
    [ObservableProperty] private ObservableCollection<Doctor> _doctorsList;
    [ObservableProperty] private ObservableCollection<Departments> _departmentsList;
    [ObservableProperty] private ObservableCollection<Specialization> _specializationsList;
    
    [ObservableProperty] private Specialization _sekectedSpec;
    [ObservableProperty] private Departments _selectedDepartment;
  

    public AddDoctorViewModel(IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
        
        
        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
        using (var rep = serviceProvider.GetRequiredService<DepartmentRep>())
        {
            DepartmentsList = new ObservableCollection<Departments>(rep.GetDepartments());
            SelectedDepartment = DepartmentsList.FirstOrDefault();
        }

        using (var rep = serviceProvider.GetRequiredService<SpecializationRep>())
        {
            SpecializationsList = new ObservableCollection<Specialization>(rep.GetSpec());
            SekectedSpec = SpecializationsList.FirstOrDefault();
        }
    }
    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(AdFullName))
            return false;

        if (string.IsNullOrWhiteSpace(AdPhone))
            return false;
        
        if (string.IsNullOrWhiteSpace(AdRoom))
            return false;
        
        if (AdPhone.Length < 11)
            return false;
       

        return true;
    }
    
    [RelayCommand]
    async Task SaveDoctor()
    {
        if (!Validate())
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                "Заполните все поля корректно",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }
        var res = new Doctor()
        {
            FullName = AdFullName,
            SpecialtyId = SekectedSpec.Id,
            PhoneNumber = AdPhone,
            Room = AdRoom,
            DepartmentId = SelectedDepartment.Id
        };
        
        using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
        {
            rep.AddDoctor(res);
        }
        
        _closeAction?.Invoke();
    }

    
    [RelayCommand]
    void Cansel()
    {
        _closeAction?.Invoke();
    }
    
}