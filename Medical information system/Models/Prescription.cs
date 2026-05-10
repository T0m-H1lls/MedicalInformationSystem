using System;

namespace Medical_information_system.Models;

public class Prescription
{
    
    public int Id {get; set;}
    public int AppointmentID {get; set;}
    public int MedicationID {get; set;}
    public string Dosage {get; set;}
    public string Duration {get; set;}
    
    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public string MedicalName { get; set; }
    
}