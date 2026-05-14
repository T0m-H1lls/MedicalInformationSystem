using System;

namespace Medical_information_system.Models;

public class Appointments
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; }// вынести
    
    public int MedicalRecordID { get; set; }

    public string PatientName { get; set; }
    public string DoctorName { get; set; }

    public string ReferralDoctor { get; set; }


}