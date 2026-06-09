using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Spire.Xls;

namespace Medical_information_system.ViewModels;

public partial class StatisticPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    
    [ObservableProperty] private DateTimeOffset? _selectedDate = DateTimeOffset.Now;
    [ObservableProperty] private bool _useDateFilter = false;
    [ObservableProperty] private int _selectedMonth = DateTime.Now.Month;
    [ObservableProperty] private int _selectedYear = DateTime.Now.Year;
    [ObservableProperty] private string[] _months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
    
    
    

    [ObservableProperty] private DateTimeOffset? _date =  DateTimeOffset.Now;
    [ObservableProperty] private int _countPatient;
    [ObservableProperty] private string _countMedicalRecord;
    [ObservableProperty] private string _docFullName;
    [ObservableProperty] private string _completedText; 
    [ObservableProperty] private ObservableCollection<Statistic>  _statistics = new();
    [ObservableProperty] private ObservableCollection<Statistic> _activeDoctorCount = new();
    [ObservableProperty] private ObservableCollection<Statistic> _patientsByGender = new();
    [ObservableProperty] private string _statisticsPeriod = "За всё время";
    

    public StatisticPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        LoadStatistics();      
        LoadActiveDoctors();  
        LoadPatientsByGender();
    }
    [RelayCommand]
    private void LoadStatistics()
    {
        using (var rep = _serviceProvider.GetRequiredService<StatisticRep>())
        {
            if (UseDateFilter)
            {
                int monthNumber = SelectedMonth + 1;
                Statistics = new ObservableCollection<Statistic>(rep.GetPatientColor(monthNumber, SelectedYear));
                StatisticsPeriod = $"За {Months[SelectedMonth]} {SelectedYear} г.";
            }
            else
            {
                Statistics = new ObservableCollection<Statistic>(rep.GetPatientColor());
                StatisticsPeriod = "За всё время";
            }
        }
    }

    
    [RelayCommand]
    private void LoadActiveDoctors()
    {
        using (var rep = _serviceProvider.GetRequiredService<StatisticRep>())
        {
            if (UseDateFilter)
            {
                int monthNumber = SelectedMonth + 1;
                ActiveDoctorCount = new ObservableCollection<Statistic>(rep.GetActiveDoctorsCount(monthNumber,SelectedYear));
            }
            else
            {
                ActiveDoctorCount = new ObservableCollection<Statistic>(rep.GetActiveDoctorsCount());
            }
        }
    }
    
    [RelayCommand]
    private void LoadPatientsByGender()
    {
        using (var rep = _serviceProvider.GetRequiredService<StatisticRep>())
        {
            if (UseDateFilter)
            {
                int monthNumber = SelectedMonth + 1;
                PatientsByGender = new ObservableCollection<Statistic>(rep.GetPatientsByGender(monthNumber,SelectedYear));
            }
            else
            {
                PatientsByGender = new ObservableCollection<Statistic>(rep.GetPatientsByGender());
            }
        }
    }
    
    [RelayCommand]
    private void ApplyDateFilter()
    {
        LoadStatistics();
        LoadActiveDoctors();
        LoadPatientsByGender();
    }
    
    [RelayCommand]
    private void SetCurrentMonth()
    {
        UseDateFilter = true;
        SelectedMonth = DateTime.Now.Month-1;
        SelectedYear = DateTime.Now.Year;
        ApplyDateFilter();
    }
    
    [RelayCommand]
    private void ClearDateFilter()
    {
        UseDateFilter = false;
        ApplyDateFilter();
    }

    [RelayCommand]
    async Task GenerateStatisticsFromToExel()
    {
        Workbook workbook = new();
        Worksheet sheet = workbook.Worksheets[0];
        sheet.Name = "Статистика";
        FileStream file_stream = new FileStream("Статистика.xls", FileMode.Create);
        
        sheet.Range["A1"].Text = "Статистика (больница)";
        sheet.Range["A1"].Style.Font.IsBold = true;
        sheet.Range["A1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
        
        sheet.Range["A3"].Text = $"Дата: {Date.ToString()}";
        
        sheet.Range["A4"].Text = "ФИО доктора";
        sheet.Range["B4"].Text = "Кол-во обслуживаемых пациентов";
        sheet.Range["A4"].Style.Font.IsBold = true;
        sheet.Range["B4"].Style.Font.IsBold = true;

        sheet.Range["D4"].Text = "Название специлизации";
        sheet.Range["E4"].Text = "Сколько докторов в данной\nспециализации";
        sheet.Range["D4"].Style.Font.IsBold = true;
        sheet.Range["E4"].Style.Font.IsBold = true;
        
        sheet.Range["G4"].Text = "Пол";
        sheet.Range["H4"].Text = "Кол-во пациентов";
        sheet.Range["G4"].Style.Font.IsBold = true;
        sheet.Range["H4"].Style.Font.IsBold = true;
            
        
        int currentRow = 5;
        foreach (var statistic in Statistics)
        {
            string doctorName = statistic.DocFullName;
            int patientCount = statistic.CountPatient;
            sheet.Range[$"A{currentRow}"].Text = doctorName;
            sheet.Range[$"B{currentRow}"].Text = patientCount.ToString();

            currentRow++;
        }
        
        int currentRow2 = 5;
        foreach (var statistic in ActiveDoctorCount)
        {
            string specName = statistic.SpecializationName;
            int activeDocCount = statistic.ActiveDoctorsCount;
            sheet.Range[$"D{currentRow2}"].Text = specName;
            sheet.Range[$"E{currentRow2}"].Text = activeDocCount.ToString();
            
            currentRow2++;
        }
        
        int currentRow3 = 5;
        foreach (var statistic in PatientsByGender)
        {
            string gender = statistic.Gender;
            int patientCount = statistic.PatientCount;
            sheet.Range[$"G{currentRow3}"].Text = gender;
            sheet.Range[$"H{currentRow3}"].Text = patientCount.ToString();
            
            
            currentRow3++;
        }
       
        
        string lastRow = (4 + Statistics.Count).ToString();
        var dataRange = sheet.Range[$"A4:B{lastRow}"];
        
        dataRange.Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
        dataRange.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
        dataRange.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
        dataRange.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
        
        string lastRow2 = (4 + ActiveDoctorCount.Count).ToString();
        var dataRange2 = sheet.Range[$"D4:E{lastRow2}"];
        
        dataRange2.Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
        dataRange2.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
        dataRange2.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
        dataRange2.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
        
        string lastRow3 = (4 + PatientsByGender.Count).ToString();
        var dataRange3 = sheet.Range[$"G4:H{lastRow3}"];
        
        dataRange3.Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
        dataRange3.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
        dataRange3.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
        dataRange3.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
        
        sheet.AllocatedRange.AutoFitColumns();
        
        workbook.SaveToStream(file_stream);
        await MessageBoxManager.GetMessageBoxStandard("Успех", "Экспорт успешно завершён!", ButtonEnum.Ok).ShowAsync();
        file_stream.Close();
    }
    
}