using Avalonia;
using System;
using Medical_information_system.DB;
using Medical_information_system.DB.Repository;
using Medical_information_system.Models;
using Medical_information_system.ViewModels;
using Medical_information_system.Views;
using Medical_information_system.Views.PageView;
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
                
                s.AddTransient<DiagnosPageViewModel>();
                s.AddTransient<DiagnoseRep>();
                s.AddTransient<DiagnosPageView>();
                
                s.AddTransient<MedicalRecordRep>();
                s.AddTransient<MedicalRecordsPageViewModel>();
                s.AddTransient<MedicalRecordsPageView>();

                s.AddTransient<MedicationRep>();
                s.AddTransient<MedicationsPageViewModel>();
                s.AddTransient<MedicationsPageView>();

                s.AddTransient<PrescriptionRep>();
                s.AddTransient<PrescriptionsPageView>();
                s.AddTransient<PrescriptionsPageViewModel>();
                
                s.AddTransient<RegistrationViewModel>();
                s.AddTransient<RegistrationView>();

                s.AddTransient<AccountPageViewModel>();
                s.AddTransient<AccountPageView>();
                s.AddSingleton<AccountName>();
               
                
                s.AddTransient<AuthorizationViewModel>();
                s.AddTransient<AuthorizationView>();
                
                s.AddTransient<StartViewModel>();
                s.AddTransient<StartView>();
                
                s.AddSingleton<Navigation>();

                s.AddTransient<UserRep>();

            }).
            Build();
            BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
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