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
using Medical_information_system.Views;
using Medical_information_system.Views.Add;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Medical_information_system.ViewModels;

public partial class PrescriptionsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<Prescription> _prescriptionsList = new();
    [ObservableProperty] private ObservableCollection<Patient> _patientsList = new();
    [ObservableProperty] private ObservableCollection<Medication> _medicinesList = new();
    [ObservableProperty] private Patient _selectedPatient;
    [ObservableProperty] private Medication _selectedMedicine;
    [ObservableProperty] private Prescription _selectedPrescription;

    [ObservableProperty] private string _edMedicine;
    [ObservableProperty] private string _edDosage;
    [ObservableProperty] private string _EdDuration;
    
    [ObservableProperty] private int _currentPageSize;
    [ObservableProperty] List<int> pageSizes;
    [ObservableProperty]private string pageInfo;
    private int currentPage = 1;
    private int totalPages;
    
    
    [ObservableProperty] private bool _viewStyle = false;
    
    
    private string _searchText;
    public string SearchText
    {
        get =>_searchText;
        set
        {
            _searchText = value;
            SearchPrescreption();
            OnPropertyChanged(nameof(SearchText));
        }
    }

    void SearchPrescreption()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
            {
                PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions());
            }
        }
        else
        {
            using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
            {
                PrescriptionsList = new ObservableCollection<Prescription>(
                                rep.GetPrescriptions().Where(s =>
                                    s.DoctorName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                    s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                    s.Dosage.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                    s.Duration.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                    s.MedicalName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
            }
            

        }
    }

    public PrescriptionsPageViewModel(IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
        PageSizes = new List<int>([5,10,20]);
        CurrentPageSize = PageSizes.First();
        using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
        {
            PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions());
        }
        
        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id)); 
        }
        SelectedPatient = PatientsList.FirstOrDefault();
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

        using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
        {
            PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions(pageIndex, CurrentPageSize));
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

    [RelayCommand]
    void AddOpenWin()
    {
        var vm = ActivatorUtilities.CreateInstance<AddPrescriptionViewModel>(_serviceProvider);
        var win = _serviceProvider.GetRequiredService<AddPrescription>();
        win.DataContext = vm;
        win.Show();
        vm.SetClose(win.Close);
        win.Closing += WinOnClosing;
        
    }

    private void WinOnClosing(object? sender, WindowClosingEventArgs e)
    {
        using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
        {
            PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions());
        }
    }

    [RelayCommand]
    async Task DeletePrescription()
    {
        if (SelectedPrescription==null)
        {
            var box2 = MessageBoxManager.GetMessageBoxStandard("Ошибка","Выберите что хотите удалить",ButtonEnum.Ok);
            var result2 = await box2.ShowAsync();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Удалить","Удалить выбранное назначение",ButtonEnum.OkCancel);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Ok)
            {
                using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
                {
                     rep.DeletePrescription(SelectedPrescription.Id);
                }
               
                using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
                {
                    PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions());
                }
            }
        }
        
       
    }
    
    
    [RelayCommand]
    async Task SwitchView()
    {
        if (SelectedPrescription==null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Выберите назначение",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }
        else if (SelectedPrescription.DoctorId != AccountName.User.Id)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Вы не можете изменять чужие записи",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }
        SelectedPatient = PatientsList.FirstOrDefault(x => x.Id == SelectedPrescription.PatientId);
        EdDosage = SelectedPrescription.Dosage;
        EdDuration = SelectedPrescription.Duration;
        EdMedicine = SelectedPrescription.MedicalName;
        ViewStyle = true;
    }
    private bool ValidateEdit( out string error)
    {
        if (string.IsNullOrWhiteSpace(EdDosage))
        {
              error = "Введите дозу";
              return false;
        }
       
        if (string.IsNullOrWhiteSpace(EdDuration))
        {
            error = "Введите продолжительность";
            return false;
        }
        if (string.IsNullOrWhiteSpace(EdMedicine))
        {
            error = "Введите лекарство";
            return false;
        }

        error = "";
        return true;
    }

    [RelayCommand]
    async Task SaveEdit()
    {
        if (SelectedPrescription==null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Выберите назначение",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }

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
            SelectedPrescription.DoctorId = AccountName.User.Id;
            SelectedPrescription.PatientId = SelectedPatient.Id;
            SelectedPrescription.Dosage = EdDosage;
            SelectedPrescription.Duration = EdDuration;
            SelectedPrescription.MedicalName = EdMedicine;
           

            using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
            {
                rep.UpdatePrescription(SelectedPrescription);
            }
           

            using (var rep = _serviceProvider.GetRequiredService<PrescriptionRep>())
            {
                PrescriptionsList = new ObservableCollection<Prescription>(rep.GetPrescriptions());
            }

            ViewStyle = false;

            var successBox = MessageBoxManager.GetMessageBoxStandard(
                "Успех", "Назначение успешно изменено",
                ButtonEnum.Ok);

            await successBox.ShowAsync();
        }
    }
    [RelayCommand]
    void ClosePanel()
    {
        ViewStyle = false;
    }
    
}