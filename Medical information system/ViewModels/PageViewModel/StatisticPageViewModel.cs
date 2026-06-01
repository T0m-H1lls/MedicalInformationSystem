using System;
using System.IO;
using Avalonia.Controls.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spire.Xls;

namespace Medical_information_system.ViewModels;

public partial class StatisticPageViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Workbook _workbook;

    [ObservableProperty] private DateTimeOffset? _date =  DateTimeOffset.Now;
    [ObservableProperty] private string _countPatient;
    [ObservableProperty] private string _countMedicalRecord;
    

    public StatisticPageViewModel(IServiceProvider serviceProvider,Workbook workbook)
    {
        _serviceProvider = serviceProvider;
        _workbook = workbook;
    }

    [RelayCommand]
    void GenerateStatisticsFromToExel()
    {
        Worksheet sheet = _workbook.Worksheets[0];
        sheet.Name = "Статистика";
        FileStream file_stream = new FileStream("Статистика.xls", FileMode.Create);
        
        sheet.Range["A1"].Text = "Статистика (больница)";
        sheet.Range["A1"].Style.Font.IsBold = true;
        sheet.Range["A1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
        
        sheet.Range["A3"].Text = $"Дата: {Date.ToString()}";
        
        sheet.Range["A4"].Text = "Кол-во добавленных пациентов";
        sheet.Range["B4"].Text = "Кол-во добавленных записей";
        sheet.Range["A4"].Style.Font.IsBold = true;
        sheet.Range["B4"].Style.Font.IsBold = true;
        
        sheet.Range["A5"].Text = CountPatient;
        sheet.Range["B5"].Text =CountMedicalRecord;
        
        sheet.Range["A4:B5"].Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
        sheet.Range["A4:B5"].Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
        sheet.Range["A4:B5"].Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
        sheet.Range["A4:B5"].Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
        
        sheet.AllocatedRange.AutoFitColumns();
        
        _workbook.SaveToStream(file_stream);
        file_stream.Close();
        System.Diagnostics.Process.Start("Статистика.xls");

    }
    
}