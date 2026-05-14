namespace Medical_information_system.Models;

public class Doctor
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int SpecialtyId { get; set; }
    public string Speciality { get; set; }
    public string PhoneNumber { get; set; }
    public string Room { get; set; } 
    public int DepartmentId { get; set; }
    
    public string DepartmentName { get; set; }
    
    
}