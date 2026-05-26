using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class MedicalRecordRep:Base
{
    public MedicalRecordRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }
    
    public List<MedicalRecord> GetMedicalRecords()
    {
        List<MedicalRecord> diagnosesList = new();
        string sql = @"select m.Id,m.AppointmentId,m.RecordDate,m.Medicine,m.Description,m.Diagnostext, d2.FullName,m.RecordDate,p.FullName as PatientName 
                      from medicalrecords m
                      join appointments a ON m.AppointmentId  = a.Id 
                      join doctors d2 on a.DoctorId  =d2.Id
                      join patients p  on a.PatientId = p.Id";
        try
        {
            using (var rep = new MySqlCommand(sql, connection))
            {
                using (var reader = rep.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        diagnosesList.Add(new MedicalRecord()
                        {
                            Id = reader.GetInt32("Id"),
                            AppointmentId = reader.GetInt32("AppointmentId"),//есть
                            Description = reader.GetString("Description"),//есть
                            RecordDate = reader.GetDateTime("RecordDate"),//есть
                            PatientName = reader.GetString("PatientName"),
                            DoctorName = reader.GetString("FullName"),//есть
                            DiagnoseName = reader.GetString("Diagnostext"),
                            MedicineName = reader.GetString("Medicine")
                        });
                    }
                }
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return diagnosesList;
    }
    
     
    
    
     public void AddMedicalRecords(MedicalRecord medicalRecord)
    {
        string sql = @"insert into `medicalrecords` values(0,@PatientId,@AppointmentId,@DiagnoseId,@Description,@RecordDate)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id", medicalRecord.Id);
                mc.Parameters.AddWithValue("@PatientId", medicalRecord.PatientId);
                mc.Parameters.AddWithValue("@AppointmentId", medicalRecord.AppointmentId);
                mc.Parameters.AddWithValue("@Description", medicalRecord.Description);
                mc.Parameters.AddWithValue("@RecordDate", medicalRecord.RecordDate);
                mc.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public bool UpdateMedicalRecord(MedicalRecord medicalRecord)
    {
        string sql = @"UPDATE medicalrecords
                   SET AppointmentId = @AppointmentId,
                       Description = @Description,
                       RecordDate = @RecordDate,
                       MedicineId = @MedicineId
                   WHERE Id = @Id";

        try
        {
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", medicalRecord.Id);
                command.Parameters.AddWithValue("@AppointmentId", medicalRecord.AppointmentId);
                command.Parameters.AddWithValue("@Description", medicalRecord.Description);
                command.Parameters.AddWithValue("@RecordDate", medicalRecord.RecordDate);
                command.Parameters.AddWithValue("@MedicineId", medicalRecord.Medicineid);

                command.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
    
    public bool DeleteMedicalRecord(int id)
    {
        string sql = @"UPDATE medicalrecords
                   SET IsActive = 0,
                       DeletedAt = NOW()
                   WHERE Id = @Id";

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id", id);

                mc.ExecuteNonQuery();
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