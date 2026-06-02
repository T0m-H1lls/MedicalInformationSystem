using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.ViewModels.AddViewModel;
using Medical_information_system.Views.Add;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Medical_information_system.ViewModels;

public partial class ApoitmentsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private Action _closeAction;
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList = new();
    [ObservableProperty] private ObservableCollection<Patient> _patientsList = new();
    [ObservableProperty] private ObservableCollection<Status> _statusList = new();
    [ObservableProperty] private ObservableCollection<Doctor> _referralDoctorsList;
    [ObservableProperty] private ObservableCollection<Doctor> _doctorList = new();
    
    [ObservableProperty] private Patient _selectedPatientEdit;
    [ObservableProperty] private Status _selectedStatusEdit;
    [ObservableProperty] private Doctor? _selectedReferralDoctor;
    [ObservableProperty] private Doctor? _selectedDoctorEdit;
    [ObservableProperty] private Appointments _selectedAppointment;
    
    [ObservableProperty] private string _fullNameDoc;
    [ObservableProperty] private bool _viewStyle = false;
    
    [ObservableProperty] private DateTimeOffset? _edAppointmentDate;
    
    [ObservableProperty] private int _currentPageSize;
    [ObservableProperty] List<int> pageSizes;
    [ObservableProperty]private string pageInfo;
    private int currentPage = 1;
    private int totalPages;
    
    
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

    public ApoitmentsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        PageSizes = new List<int>([5,10,20]);
        CurrentPageSize = PageSizes.First();
        
        using (var rep = serviceProvider.GetRequiredService<ApointmentRep>())
        {
             AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id));
        }
        
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id));
        }
        
        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorList = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
        
        using (var rep = serviceProvider.GetRequiredService<StatusRep>())
        {
            StatusList = new ObservableCollection<Status>(rep.StatusList());
        }
        
        ReferralDoctorsList = new ObservableCollection<Doctor>(DoctorList.Where(x => x.Id != AccountName.User.Id));
    }
    partial void OnCurrentPageSizeChanged(int value)
    {
        CalculatePages();
    }

    void CalculatePages()
    {
        using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
        {
            var rowsCount = rep.GetRowsCount(AccountName.User.Id);
            totalPages = (int)Math.Ceiling(((double)rowsCount / CurrentPageSize));
             
            currentPage = 1;
            ShowPage(currentPage);
        }
       
    }

    void ShowPage(int pageIndex)
    {
        currentPage = pageIndex;

        using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id, pageIndex, CurrentPageSize));
            PageInfo = $"Страница {currentPage} из {totalPages}";
        }
    }

    [RelayCommand]
    private void ShowFirstPage()
    {
        ShowPage(1);
    }
    
    [RelayCommand]
    private void ShowLastPage()
    {
        ShowPage(totalPages);
    }

    [RelayCommand]
    private void ShowNextPage()
    {
        if (currentPage < totalPages)
            ShowPage(currentPage + 1);
    }
    
    [RelayCommand]
    private void ShowPrevPage()
    {
        if (currentPage > 1)
            ShowPage(currentPage - 1);
    }
    
    void SearchAppointment()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
            {
                AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id));
            }
        }
        else
        {
            using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
            {
                 AppointmentsList = new ObservableCollection<Appointments>(
                     rep.GetAppointments(AccountName.User.Id).Where(s =>
                         s.DoctorFullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) || 
                         s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) || 
                         s.Status.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
            }
           
        }
    }

    public void SetClose(Action action)
    {
        _closeAction = action;
    }
     [RelayCommand]
     void ClosePanel() 
     { 
         ViewStyle = false;
     }
    [RelayCommand]
    void OpenAddAppointment()
    {
        var vm =  ActivatorUtilities.CreateInstance<AddApoitmentViewModel>(_serviceProvider);
        var win = _serviceProvider.GetRequiredService<AddApoitment>();
        win.DataContext = vm;
        win.Show();
        vm.SetClose(win.Close);
        win.Closing += WinOnClosing;
    }

    private void WinOnClosing(object? sender, WindowClosingEventArgs e)
    {
        using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id));
        }
    }

    [RelayCommand]
    async Task DeleteAppointment()
    {
        if (SelectedAppointment == null)
        {
            var box2 = MessageBoxManager.GetMessageBoxStandard("Ошибка","Выберите что хотите удалить",ButtonEnum.Ok);
            var result2 = await box2.ShowAsync();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Удалить","Удалить выбранную запись",ButtonEnum.OkCancel);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Ok)
            {
                using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
                {
                    rep.DeleteAppointment(SelectedAppointment.Id);
                }
                
                using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
                {
                    AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id));
                }
            }
        }
    }

    [RelayCommand]
    async Task UpdateAppointment()
    {
        if (SelectedAppointment == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                "Выберите запись",
                ButtonEnum.Ok);

            await box.ShowAsync();
            return;
        }

        EdAppointmentDate = SelectedAppointment.AppointmentDate.Value;
        SelectedPatientEdit = PatientsList.FirstOrDefault(x => x.Id == SelectedAppointment.PatientId);
        SelectedStatusEdit = StatusList.FirstOrDefault(x => x.Id == SelectedAppointment.StatusId);
        SelectedReferralDoctor = DoctorList.FirstOrDefault(x => x.Id == SelectedAppointment.ReferralDoctorId);

        ViewStyle = true;
    }


    [RelayCommand]
    async Task SaveEdit()
    {
        if (SelectedAppointment == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                "Выберите запись",
                ButtonEnum.Ok);

            await box.ShowAsync();
            return;
        }
        

        var confirmBox = MessageBoxManager.GetMessageBoxStandard(
            "Редактирование",
            "Сохранить изменения?",
            ButtonEnum.OkCancel);

        var result = await confirmBox.ShowAsync();

        if (result == ButtonResult.Ok)
        {
            SelectedAppointment.PatientId = SelectedPatientEdit.Id;
            SelectedAppointment.DoctorId = AccountName.User.Id;
            SelectedAppointment.StatusId = SelectedStatusEdit.Id;
            SelectedAppointment.AppointmentDate = EdAppointmentDate;
            SelectedAppointment.ReferralDoctorId = SelectedReferralDoctor?.Id;
            
            using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
            {
                rep.UpdateAppointment(SelectedAppointment);
            }
           
            using (var rep = _serviceProvider.GetRequiredService<ApointmentRep>())
            {
                AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id));
            }
            
            ViewStyle = false;

            var successBox = MessageBoxManager.GetMessageBoxStandard("Успех", "Запись успешно изменена", ButtonEnum.Ok);

            await successBox.ShowAsync();
        }
    }
    [RelayCommand]
    void ClearReferall()
    {
        SelectedReferralDoctor = null;
    }
}