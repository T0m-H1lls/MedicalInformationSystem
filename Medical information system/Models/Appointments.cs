using System;

namespace Medical_information_system.Models;

public class Appointments
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTimeOffset? AppointmentDate { get; set; }
    public string Status { get; set; }
    
    public int StatusId { get; set; }
    
    public int? MedicalRecordID { get; set; }

    public string PatientName { get; set; }
    public string DoctorFullName { get; set; }
    public string DocFullNameReferall { get; set; }
   


}