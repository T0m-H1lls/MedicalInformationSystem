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

public partial class DoctorsPageViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isAdmin;
    private readonly IServiceProvider _serviceProvider;
 
    [ObservableProperty] ObservableCollection<Doctor> _doctorsList = new();
    [ObservableProperty] private bool _viewStyle = false;
    
    [ObservableProperty] private string _adFullName;
    [ObservableProperty] private string _adPhone;
    [ObservableProperty] private string _adRoom;
    [ObservableProperty] private ObservableCollection<Departments> _departmentsList;
    [ObservableProperty] private ObservableCollection<Specialization> _specializationsList;
    
    [ObservableProperty] private Specialization _selectedSpec;
    [ObservableProperty] private Departments _selectedDepartment;

    [ObservableProperty] private Doctor _selectedDoctor;

    private Action _closeAction;
    
    [ObservableProperty] private int _currentPageSize;
    [ObservableProperty] List<int> pageSizes;
    [ObservableProperty]private string pageInfo;
    private int currentPage = 1;
    private int totalPages;
    

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

    public DoctorsPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        PageSizes = new List<int>([5,10,20]);
        CurrentPageSize = PageSizes.First();
        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
        using (var rep = serviceProvider.GetRequiredService<DepartmentRep>())
        {
            DepartmentsList=new ObservableCollection<Departments>(rep.GetDepartments());
        }
        using (var rep = serviceProvider.GetRequiredService<SpecializationRep>())
        {
            SpecializationsList = new ObservableCollection<Specialization>(rep.GetSpec());
        }
        IsAdmin = AccountName.User.RoleId == 7;   
    }
    partial void OnCurrentPageSizeChanged(int value)
    {
        CalculatePages();
    }

    void CalculatePages()
    {
        using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
        {
            var rowsCount = rep.GetRowsCount();
            totalPages = (int)Math.Ceiling(((double)rowsCount / CurrentPageSize));
             
            currentPage = 1;
            ShowPage(currentPage);
        }
       
    }

    void ShowPage(int pageIndex)
    {
        currentPage = pageIndex;

        using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors(pageIndex, CurrentPageSize));
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

    private void SearchDoctors()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
            {
                DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
            }
        }
        else
        {
            using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
            {
                 DoctorsList = new ObservableCollection<Doctor>(
                     rep.GetDoctors().Where(s =>
                         s.FullName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                         s.DepartmentName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                         s.PhoneNumber.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                         s.Speciality.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                         s.Room.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
            }
           
        }
    }

    [RelayCommand]
    void OpenAddDoctorWindow()
    {
        var vm = ActivatorUtilities.CreateInstance<AddDoctorViewModel>(_serviceProvider);
        var win = _serviceProvider.GetService<AddDoctor>();
        win.DataContext = vm;
        win.Show();
        vm.SetClose(win.Close);
        win.Closing += WinOnClosing;
    }

    private void WinOnClosing(object? sender, WindowClosingEventArgs e)
    {
        using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
        }
    }


    [RelayCommand]
    async Task DeletePatient()
    {
        if (SelectedDoctor == null)
        {
            var box2 = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите кого хотите удалить", ButtonEnum.Ok);
            var result2 = await box2.ShowAsync();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Удалить", "Удалить выбранного доктора",
                ButtonEnum.OkCancel);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Ok)
            {
                using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
                {
                     rep.DeleteDoctor(SelectedDoctor.Id);
                }
               
                
                using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
                {
                    DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
                }
            }
        }
    }

    private bool ValidateEdit(out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(AdFullName))
        {
            errorMessage = "Введите ФИО";
            return false;
        }

        if (string.IsNullOrWhiteSpace(AdRoom))
        {
            errorMessage = "Введите кабинет";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(AdPhone))
        {
            errorMessage = "Введите телефон";
            return false;
        }
        errorMessage = "";
        return true;
    }
    

    [RelayCommand]
    async Task SwitchView()
    {
        if (SelectedDoctor == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Выберите доктора",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }
        if (SelectedDoctor == null)
            return;
        AdFullName = SelectedDoctor.FullName;
        AdPhone = SelectedDoctor.PhoneNumber;
        AdRoom = SelectedDoctor.Room;
        SelectedDepartment = DepartmentsList?.FirstOrDefault(x => x.Id == SelectedDoctor.DepartmentId);
        SelectedSpec = SpecializationsList?.FirstOrDefault(x => x.Id == SelectedDoctor.SpecialtyId);
        ViewStyle = true;
    }
    
    [RelayCommand]
    void ClosePanel()
    {
        ViewStyle = false;
    }

    [RelayCommand]
    async Task SaveEdit()
    {
        if (!ValidateEdit(out string error))
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", error,
                ButtonEnum.Ok);

            await errorBox.ShowAsync();

            return;
        }
        

        var confirmBox = MessageBoxManager.GetMessageBoxStandard(
            "Редактирование", "Сохранить изменения?",
            ButtonEnum.OkCancel);

        var result = await confirmBox.ShowAsync();

        if (result == ButtonResult.Ok)
        {
            SelectedDoctor.FullName = AdFullName;
            SelectedDoctor.PhoneNumber = AdPhone;
            SelectedDoctor.DepartmentId = SelectedDepartment.Id;
            SelectedDoctor.SpecialtyId = SelectedSpec.Id;
            SelectedDoctor.Room = AdRoom;
            
            using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
            {
                  rep.UpdateDoctor(SelectedDoctor);
            }

            using (var rep = _serviceProvider.GetRequiredService<DoctorRep>())
            {
                DoctorsList = new ObservableCollection<Doctor>(rep.GetDoctors());
            }
            ViewStyle = false;
            
            var successBox = MessageBoxManager.GetMessageBoxStandard(
                "Успех", "Доктор успешно изменен",
                ButtonEnum.Ok);

            await successBox.ShowAsync();
        }
    }

    
}