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
        string sql = @"select m.Id, m.PatientId, m.AppointmentId,m.DiagnosisId,m.Description,m.RecordDate,m.MedicineId,p.FullName as PatientName,d2.FullName,d.Name as DiagnoseName,m3.Name as MedicineName, m.RecordDate 
                      from medicalrecords m
					  join patients p ON m.PatientId = p.Id
                      join appointments a ON m.AppointmentId  = a.Id 
                      join diagnoses d ON m.DiagnosisId  = d.Id 
                      join medications m3 on m.MedicineId  = m3.Id
                      join doctors d2 on a.DoctorId  =d2.Id";
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
                            PatientId = reader.GetInt32("PatientId"),
                            AppointmentId = reader.GetInt32("AppointmentId"),
                            DiagnoseId = reader.GetInt32("DiagnosisId"),
                            Description = reader.GetString("Description"),
                            Medicineid = reader.GetInt32("MedicineId"),
                            RecordDate = reader.GetDateTime("RecordDate"),
                            PatientName = reader.GetString("PatientName"),
                            DoctorName = reader.GetString("FullName"),
                            DiagnoseName = reader.GetString("DiagnoseName"),
                            MedicineName = reader.GetString("MedicineName")
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
                mc.Parameters.AddWithValue("@DiagnoseId", medicalRecord.DiagnoseId);
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
        string sql = @"update `medicalrecords` set PatientId = @PatientId, ApointmentId = @AppointmentId, DiagnoseId = @DiagnoseId, Description = @Description, RecordDate = @RecordDate,MedicineId = @MedicineId ";
        try
        {
             using (var command = new MySqlCommand(sql, connection))
             {
                 command.Parameters.AddWithValue("@PatientId", medicalRecord.PatientId);
                 command.Parameters.AddWithValue("@AppointmentId", medicalRecord.AppointmentId);
                 command.Parameters.AddWithValue("@DiagnoseId", medicalRecord.DiagnoseId);
                 command.Parameters.AddWithValue("@Description", medicalRecord.Description);
                 command.Parameters.AddWithValue("@RecordDate", medicalRecord.RecordDate);
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
        string sql = @"delete from `medicalrecords` where `id` = @id";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@id",id);
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