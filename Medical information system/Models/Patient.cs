using System;

namespace Medical_information_system.Models;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string InsuranceNumber{ get; set; }

    public string Passport { get; set; }
    public string Snils { get; set; }
    public int DoctorId { get; set; }
}