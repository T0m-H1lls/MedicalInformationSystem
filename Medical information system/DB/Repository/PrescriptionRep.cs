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
        string sql = @"select p.Id, p.AppointmentId,p.Medicine,d.FullName as DoctorName,p2.FullName as PatientName,p.Dosage,p.Duration
                        from prescriptions p 
                        join appointments a on p.AppointmentId = a.Id 
                        join doctors d on a.DoctorId  = p.Id 
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
                            DoctorName = reader.GetString("DoctorName"),
                            MedicalName = reader.GetString("Medicine"),
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

    public bool AddPrescription(Prescription prescription)
    {
        string sql = @"INSERT INTO prescriptions
                   (AppointmentId, Medicine, Dosage, Duration)
                   VALUES
                   (@AppointmentId, @Medicine, @Dosage, @Duration)";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))  
            {
                cm.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentID);
                cm.Parameters.AddWithValue("@Medicine", prescription.MedicalName);
                cm.Parameters.AddWithValue("@Dosage", prescription.Dosage);
                cm.Parameters.AddWithValue("@Duration", prescription.Duration);

                cm.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
    public bool UpdatePrescription(Prescription prescription)
    {
        string sql = @"UPDATE prescriptions
                   SET AppointmentId = @AppointmentId,
                       Medicine = @Medicine,
                       Dosage = @Dosage,
                       Duration = @Duration
                   WHERE Id = @Id";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Id", prescription.Id);
                cm.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentID);
                cm.Parameters.AddWithValue("@Medicine", prescription.MedicalName);
                cm.Parameters.AddWithValue("@Dosage", prescription.Dosage);
                cm.Parameters.AddWithValue("@Duration", prescription.Duration);

                cm.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
    
    public bool DeletePrescription(int id)
    {
        string sql = @"UPDATE prescriptions
                   SET IsActive = 0,
                       DeletedAt = NOW()
                   WHERE Id = @Id";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Id", id);

                cm.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}