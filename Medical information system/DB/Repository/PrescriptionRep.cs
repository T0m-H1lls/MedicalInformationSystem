using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class PrescriptionRep:Base
{
    public PrescriptionRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Prescription> GetPrescriptions()
    {
        List<Prescription> prescriptionsList = new();
        string sql = @"select p.Id, p.AppointmentId,p.MedicationId,d.FullName as DoctorName,m.Name as Medical,p2.FullName as PatientName,p.Dosage,p.Duration
                        from prescriptions p 
                        join appointments a on p.AppointmentId = a.Id 
                        join doctors d on a.DoctorId  = p.Id 
                        join medications m on p.MedicationId = m.Id 
                        join patients p2 on a.PatientId = p2.Id ";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        prescriptionsList.Add(new Prescription()
                        {
                            Id = reader.GetInt32("Id"),
                            AppointmentID = reader.GetInt32("AppointmentId"),
                            MedicationID = reader.GetInt32("MedicationId"),
                            DoctorName = reader.GetString("DoctorName"),
                            MedicalName = reader.GetString("Medical"),
                            PatientName = reader.GetString("PatientName"),
                            Dosage = reader.GetString("Dosage"),
                            Duration = reader.GetString("Duration"),
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }

        return prescriptionsList;
    }
}