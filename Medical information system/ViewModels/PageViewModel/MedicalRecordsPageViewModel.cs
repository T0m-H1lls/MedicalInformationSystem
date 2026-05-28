using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly MedicalRecordRep _medicalRecordRep;
    [ObservableProperty] ObservableCollection<MedicalRecord> _medicalRecordsList = new();
    [ObservableProperty]  MedicalRecord _selectedMedicalRecord;
    [ObservableProperty] private bool _viewStyle = false; 
    [ObservableProperty] private ObservableCollection<Appointments> _appointmentsList = new();
    [ObservableProperty] private ObservableCollection<Medication> _medicinesList = new();
    [ObservableProperty] private ObservableCollection<Doctor> _doctorsList = new();
    
    [ObservableProperty] private Appointments _selectedAppointmentEdit;
    [ObservableProperty] private Medication _selectedMedicineEdit;
    [ObservableProperty] private Doctor _selectedReferallDoctor;
    [ObservableProperty] private string _descriptionEdit;
    [ObservableProperty] private DateTime _recordDateEdit;
   
  
    
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

   


    public MedicalRecordsPageViewModel(IServiceProvider serviceProvider,MedicalRecordRep medicalRecordRep)
    {
        _serviceProvider = serviceProvider;
        _medicalRecordRep = medicalRecordRep;
        MedicalRecordsList = new ObservableCollection<MedicalRecord>(_medicalRecordRep.GetMedicalRecords());
            
        using (var rep = serviceProvider.GetRequiredService<ApointmentRep>())
        {
            AppointmentsList = new ObservableCollection<Appointments>(rep.GetAppointments(AccountName.User.Id));
        }

        using (var rep = serviceProvider.GetRequiredService<MedicationRep>())
        {
            MedicinesList = new ObservableCollection<Medication>(rep.GetMedication());
        }

        using (var rep = serviceProvider.GetRequiredService<DoctorRep>())
        {
            DoctorsList =
                new ObservableCollection<Doctor>(
                    rep.GetDoctors());
        }
        
    }
    
    private void SearchMedicalRecord()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(_medicalRecordRep.GetMedicalRecords());
        }
        else
        {
            MedicalRecordsList = new ObservableCollection<MedicalRecord>(
                _medicalRecordRep.GetMedicalRecords().Where(s =>
                    s.PatientName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.DoctorName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.MedicineName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                    s.DiagnoseName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
    
    

    [RelayCommand]
    void OpenAddMedicalRecord()
    {
        var win = _serviceProvider.GetRequiredService<AddMedicalRecord>();
        var vm = ActivatorUtilities.CreateInstance<AddMedicalRecordViewModel>(_serviceProvider);
        win.DataContext = vm;
        win.Show();
    }
    
    private bool Validate(out string error)
    {
        if (SelectedAppointmentEdit == null)
        {
            error = "Выберите прием";
            return false;
        }

        if (SelectedMedicineEdit == null)
        {
            error = "Выберите лекарство";
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
    void EditMedicalRecord()
    {
        
        if (SelectedMedicalRecord == null)
            return;

        SelectedAppointmentEdit = AppointmentsList.FirstOrDefault(x => x.Id == SelectedMedicalRecord.AppointmentId);
        SelectedMedicineEdit = MedicinesList.FirstOrDefault(x => x.Id == SelectedMedicalRecord.Medicineid);
        DescriptionEdit = SelectedMedicalRecord.Description;
        RecordDateEdit = SelectedMedicalRecord.RecordDate;
        SelectedReferallDoctor = DoctorsList.FirstOrDefault(x => x.FullName == SelectedMedicalRecord.ReferallDoc);
        ViewStyle = true;
    }
    
    [RelayCommand]
    async Task Save()
    {
        if (SelectedMedicalRecord == null)
            return;

        if (!Validate(out string error))
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                error,
                ButtonEnum.Ok);

            await box.ShowAsync();
            return;
        }

        SelectedMedicalRecord.AppointmentId = SelectedAppointmentEdit.Id;
        SelectedMedicalRecord.Medicineid = SelectedMedicineEdit.Id;
        SelectedMedicalRecord.Description = DescriptionEdit;
        SelectedMedicalRecord.RecordDate = RecordDateEdit;

        if (SelectedReferallDoctor != null)
        {
            SelectedMedicalRecord.ReferallDoc = SelectedReferallDoctor.FullName;
        }
        else
        {
             SelectedMedicalRecord.ReferallDoc = null;
        }
        
        _medicalRecordRep.UpdateMedicalRecord(SelectedMedicalRecord);
        MedicalRecordsList = new ObservableCollection<MedicalRecord>(_medicalRecordRep.GetMedicalRecords());
        ViewStyle = false;

        var success = MessageBoxManager.GetMessageBoxStandard(
            "Успех",
            "Запись изменена",
            ButtonEnum.Ok);

        await success.ShowAsync();
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
            _medicalRecordRep.DeleteMedicalRecord(SelectedMedicalRecord.Id);

            MedicalRecordsList = new ObservableCollection<MedicalRecord>(_medicalRecordRep.GetMedicalRecords());
        }
    }
    [RelayCommand]
    void ClearReferall()
    {
        SelectedReferallDoctor = null;
    }
}