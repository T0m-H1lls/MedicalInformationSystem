namespace Medical_information_system.Models;

public class Statistic
{
    public string PatientName { get; set; }
    
    public string DocFullName { get; set; }
    public int CountPatient { get; set; }

    
    public string SpecializationName { get; set; }
    public int ActiveDoctorsCount { get; set; }
    
    public string Gender { get; set; }
    public int PatientCount { get; set; }
}