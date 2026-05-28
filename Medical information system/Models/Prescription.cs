public class Prescription
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string MedicalName { get; set; }
    public string Dosage { get; set; }
    public string Duration { get; set; }
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
}