using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Medical_information_system.ViewModels.AddViewModel;

public partial class AddMedicationViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    public AddMedicationViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}