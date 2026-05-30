using System;
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

public partial class MedicalRecordsPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] ObservableCollection<MedicalRecord> _medicalRecordsList = new();
    [ObservableProperty]  MedicalRecord _selectedMedicalRecord;
    [ObservableProperty] private bool _viewStyle = false; 
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList = new();
    [ObservableProperty] private ObservableCollection<Medication> _medicinesList = new();
    [ObservableProperty] private ObservableCollection<Patient> _patientsList = new();
    
    [ObservableProperty] private Appointments _selectedAppointmentEdit;
    [ObservableProperty] private Medication _selectedMedicineEdit;
    
    [ObservableProperty] private string _descriptionEdit;
    [ObservableProperty] private string _diagnoseEdit;
    [ObservableProperty] private string _medicineEdit;
    [ObservableProperty] private DateTimeOffset _recordDateEdit = DateTime.Now;
   [ObservableProperty] private Patient _selectedPatient;
  
    
    [RelayCommand]
    void ClosePanel()
    {
        ViewStyle = false;
    }
    
    private string _searchText;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            SearchMedicalRecord();
            OnPropertyChanged(nameof(SearchMedicalRecord));
        }
    }

   


    public MedicalRecordsPageViewModel(IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
        using (var rep = serviceProvider.GetRequiredService<MedicalRecordRep>())
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
        }
        

        using (var rep = serviceProvider.GetRequiredService<PatientRep>())
        {
            PatientsList = new ObservableCollection<Patient>(rep.GetAllPatient(AccountName.User.Id));
        }

        SelectedPatient = PatientsList.FirstOrDefault();

    }
    
    private void SearchMedicalRecord()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
            {
                MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
            }
        }
        else
        {
            using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
            {
               MedicalRecordsList = new ObservableCollection<MedicalRecord>(
                              rep.GetMedicalRecords().Where(s =>
                                  s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                  s.DoctorName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                  s.MedicineName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                                  s.DiagnoseName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
            }
           
        }
    }
    
    

    [RelayCommand]
    void OpenAddMedicalRecord()
    {
        var win = _serviceProvider.GetRequiredService<AddMedicalRecord>();
        var vm = ActivatorUtilities.CreateInstance<AddMedicalRecordViewModel>(_serviceProvider);
        win.DataContext = vm;
        win.Show();
        vm.SetClose(win.Close);
        win.Closing += WinOnClosing;
    }

    private void WinOnClosing(object? sender, WindowClosingEventArgs e)
    {
        using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
        }
    }
    private bool Validate(out string error)
    {

        if (string.IsNullOrWhiteSpace(MedicineEdit))
        {
            error = "Введите лекарство";
            return false;
        }

        if (string.IsNullOrWhiteSpace(DiagnoseEdit))
        {
            error = "Введите диагноз";
            return false;
        }
        if (SelectedPatient == null)
        {
            error = "Выберите пациента";
            return false;
        }
        if (RecordDateEdit>DateTimeOffset.Now)
        {
            error = "Введеная дата не может быть больше текущей даты";
            return false;
        }

        if (string.IsNullOrWhiteSpace(DescriptionEdit))
        {
            error = "Введите описание";
            return false;
        }

        error = "";
        return true;
    }
    
    [RelayCommand]
    async Task DeleteMedicalRecord()
    {
        if (SelectedMedicalRecord == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                "Выберите запись",
                ButtonEnum.Ok);

            await box.ShowAsync();
            return;
        }

        var confirm = MessageBoxManager.GetMessageBoxStandard(
            "Удаление",
            "Удалить запись?",
            ButtonEnum.OkCancel);

        var result = await confirm.ShowAsync();

        if (result == ButtonResult.Ok)
        {
            using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
            {
               rep.DeleteMedicalRecord(SelectedMedicalRecord.Id);
            }
            
          
            using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
            {
                MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
            }
        }
    }
    
    [RelayCommand]
    async Task SwitchView()
    {
        if (SelectedMedicalRecord==null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка", "Выберите запись",
                ButtonEnum.Ok);

            await box.ShowAsync();

            return;
        }
        if (SelectedMedicalRecord == null)
            return;

        MedicineEdit = SelectedMedicalRecord.MedicineName;
        DiagnoseEdit = SelectedMedicalRecord.DiagnoseName;
        DescriptionEdit = SelectedMedicalRecord.Description;
        RecordDateEdit = SelectedMedicalRecord.RecordDate.Value;
        SelectedPatient = PatientsList.FirstOrDefault(x => x.Id == SelectedMedicalRecord.PatientId);
        ViewStyle = true;
    }
    [RelayCommand]
    async Task SaveEdit()
    {
        if (!Validate(out string error))
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
            SelectedMedicalRecord.MedicineName = MedicineEdit;
            SelectedMedicalRecord.Description = DescriptionEdit;
            SelectedMedicalRecord.DoctorId = AccountName.User.Id;
            SelectedMedicalRecord.PatientId = SelectedPatient.Id;
            SelectedMedicalRecord.RecordDate = SelectedMedicalRecord.RecordDate.Value;
            SelectedMedicalRecord.DiagnoseName = DiagnoseEdit;
            
            using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
            {
                rep.UpdateMedicalRecord(SelectedMedicalRecord);
            }
           

            using (var rep = _serviceProvider.GetRequiredService<MedicalRecordRep>())
            {
                MedicalRecordsList = new ObservableCollection<MedicalRecord>(rep.GetMedicalRecords());
            }
            
            ViewStyle = false;
            
            var successBox = MessageBoxManager.GetMessageBoxStandard(
                "Успех", "Запись успешно изменена",
                ButtonEnum.Ok);

            await successBox.ShowAsync();
        }
    }
    
    
}