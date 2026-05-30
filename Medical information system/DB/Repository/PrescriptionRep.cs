using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class PrescriptionRep : Base, IDisposable
{
    public PrescriptionRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Prescription> GetPrescriptions()
    {
        List<Prescription> prescriptionsList = new();

        string sql = @"SELECT p.Id,p.DoctorId,p.PatientId,p.Medicine,p.Dosage,p.Duration,d.FullName AS DoctorName,pt.FullName AS PatientName
                   FROM prescriptions p
                   JOIN doctors d ON p.DoctorId = d.Id
                   JOIN patients pt ON p.PatientId = pt.Id
                   WHERE p.IsActive = 1";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescriptionsList.Add(new Prescription()
                        {
                            Id = reader.GetInt32("Id"),
                            DoctorId = reader.GetInt32("DoctorId"),
                            PatientId = reader.GetInt32("PatientId"),
                            DoctorName = reader.GetString("DoctorName"),
                            PatientName = reader.GetString("PatientName"),
                            MedicalName = reader.GetString("Medicine"),
                            Dosage = reader.GetString("Dosage"),
                            Duration = reader.GetString("Duration")
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
        string sql = @"INSERT INTO prescriptions VALUES(0,@DoctorId, @Medicine, @PatientId, @Dosage, @Duration, 1, null)";
                       

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@DoctorId", prescription.DoctorId);
                cm.Parameters.AddWithValue("@PatientId", prescription.PatientId);
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
                       SET DoctorId = @DoctorId, PatientId = @PatientId, Medicine = @Medicine, Dosage = @Dosage, Duration = @Duration
                       WHERE Id = @Id";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Id", prescription.Id);
                cm.Parameters.AddWithValue("@DoctorId", prescription.DoctorId);
                cm.Parameters.AddWithValue("@PatientId", prescription.PatientId);
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
    public void Dispose()
    {
       CloseConnection();  
       base.Dispose();
       
    }
}