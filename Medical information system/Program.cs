using Avalonia;
using System;
using Medical_information_system.DB;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.ViewModels;
using Medical_information_system.ViewModels.AddViewModel;
using Medical_information_system.Views;
using Medical_information_system.Views.Add;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Medical_information_system;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().
            ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
            }).
            ConfigureServices((c,s) =>
            {
                s.Configure<DataBaseConnection>(c.Configuration.
                    GetSection("DataBaseConnection"));
                
                s.AddTransient<MainWindowViewModel>();
                s.AddTransient<MainWindow>();
                
                s.AddTransient<PatientPageViewModel>();
                s.AddTransient<PatientPageView>();
                s.AddTransient<PatientRep>();
                
                s.AddTransient<DoctorsPageViewModel>();
                s.AddTransient<DoctorsPageView>();
                s.AddTransient<DoctorRep>();
                
                s.AddTransient<ApointmentRep>();
                s.AddTransient<ApoitmentsPageViewModel>();
                s.AddTransient<ApoitmentsPageView>();
                s.AddTransient<AddApoitmentViewModel>();
                s.AddTransient<AddApoitment>();
                
                
                s.AddTransient<MedicalRecordRep>();
                s.AddTransient<MedicalRecordsPageViewModel>();
                s.AddTransient<MedicalRecordsPageView>();

                s.AddTransient<MedicationRep>();
                s.AddTransient<MedicationsPageViewModel>();
                s.AddTransient<MedicationsPageView>();

                s.AddTransient<PrescriptionRep>();
                s.AddTransient<PrescriptionsPageView>();
                s.AddTransient<PrescriptionsPageViewModel>();

                s.AddTransient<AddPatientViewModel>();
                s.AddTransient<AddPatient>();

                s.AddTransient<AddApoitmentViewModel>();
                s.AddTransient<AddApoitment>();
                
                s.AddTransient<AddDoctorViewModel>();
                s.AddTransient<AddDoctor>();
                
                s.AddTransient<AddMedicalRecordViewModel>();
                s.AddTransient<AddMedicalRecord>();
                
                s.AddTransient<AddPrescriptionViewModel>();
                s.AddTransient<AddPrescription>();
                
                s.AddTransient<DepartmentRep>();

                s.AddTransient<SpecializationRep>();

                s.AddTransient<AccountPageViewModel>();
                s.AddTransient<AccountPageView>();
                
                s.AddTransient<StatusRep>();
               
                
                s.AddTransient<AuthorizationViewModel>();
                s.AddTransient<AuthorizationView>();
                
                
                s.AddTransient<StartViewModel>();
                s.AddTransient<StartView>();
                
                s.AddSingleton<Navigation>();

                s.AddTransient<StatisticPageViewModel>();
                s.AddTransient<StatisticPageView>();
                s.AddTransient<StatisticRep>();

                s.AddTransient<UserRep>();
                s.AddSingleton<User>();

            }).
            Build();
            BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

   
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure(() => new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
   
}