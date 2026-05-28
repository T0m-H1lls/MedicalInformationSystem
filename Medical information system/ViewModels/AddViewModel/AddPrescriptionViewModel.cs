using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddPrescriptionViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty] private string _dosage;
    [ObservableProperty] private string _duration;
    [ObservableProperty] private string _medicine;

    public AddPrescriptionViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    
    
}