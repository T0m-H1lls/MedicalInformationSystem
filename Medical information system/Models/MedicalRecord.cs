using System;

namespace Medical_information_system.Models;

public class MedicalRecord
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int AppointmentId { get; set; }
    public int DiagnoseId { get; set; }
    public string Description { get; set; }
    public DateTime RecordDate { get; set; }
    public int Medicineid { get; set; }

    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public string DiagnoseName { get; set; }
    public string MedicineName { get; set; }
}