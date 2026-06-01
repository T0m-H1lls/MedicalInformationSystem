using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class MedicalRecordRep:Base,IDisposable
{
    public MedicalRecordRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }
    
    public List<MedicalRecord> GetMedicalRecords(int? pageNumber = null, int? pageSize = null)
    {
        List<MedicalRecord> diagnosesList = new();
        string sql = @"select m.Id,m.PatientId,m.DoctorId,m.RecordDate,m.Medicine,m.Description,m.Diagnostext, d.FullName,m.RecordDate,p.FullName as PatientName 
                      from medicalrecords m
                      join doctors d on m.DoctorId = d.Id
                      join patients p  on m.PatientId = p.Id
                      Where m.IsActive = 1";
        if (pageNumber != null && pageSize != null)
        {
            sql+= " limit @limit offset @offset";
        }
        try
        {
            using (var rep = new MySqlCommand(sql, connection))
            {
                if (pageNumber != null && pageSize != null)
                {
                    rep.Parameters.AddWithValue("@limit", pageSize);
                    rep.Parameters.AddWithValue("@offset", (pageNumber.Value - 1) * pageSize.Value);
                }
                using (var reader = rep.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        diagnosesList.Add(new MedicalRecord()
                        {
                            Id = reader.GetInt32("Id"),
                            PatientId = reader.GetInt32("PatientId"),
                            DoctorId = reader.GetInt32("DoctorId"),
                            Description = reader.GetString("Description"),
                            RecordDate = reader.GetDateTime("RecordDate"),
                            PatientName = reader.GetString("PatientName"),
                            DoctorName = reader.GetString("FullName"),
                            DiagnoseName = reader.GetString("Diagnostext"),
                            MedicineName = reader.GetString("Medicine"),
                            
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
    public int GetRowsCount()
    {
        string sql = @"SELECT COUNT(Id)
                       FROM `medicalrecords`
                       Where IsActive = 1";
        try
        {
            using var mc = new MySqlCommand(sql, connection);
            return Convert.ToInt32(mc.ExecuteScalar());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }
    
     public void AddMedicalRecords(MedicalRecord medicalRecord)
    {
        string sql = @"insert into `medicalrecords` values(0,@DoctorId,@PatientId,@DiagnoseText,@Description,@RecordDate,@Medicine,null,1)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id", medicalRecord.Id);
                mc.Parameters.AddWithValue("@DoctorId", medicalRecord.DoctorId);
                mc.Parameters.AddWithValue("@PatientId", medicalRecord.PatientId);
                mc.Parameters.AddWithValue("@Description", medicalRecord.Description);
                mc.Parameters.AddWithValue("@RecordDate", medicalRecord.RecordDate);
                mc.Parameters.AddWithValue("@DiagnoseText", medicalRecord.DiagnoseName);
                mc.Parameters.AddWithValue("@Medicine", medicalRecord.MedicineName);
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
                   SET DoctorId = @DoctorId,PatientId = @PatientId,DiagnosText = @DiagnosText,Description = @Description,RecordDate = @RecordDate,Medicine = @Medicine
                   WHERE Id = @Id";

        try
        {
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", medicalRecord.Id);
                command.Parameters.AddWithValue("@Description", medicalRecord.Description);
                command.Parameters.AddWithValue("@RecordDate", medicalRecord.RecordDate);
                command.Parameters.AddWithValue("@Medicine", medicalRecord.MedicineName);
                command.Parameters.AddWithValue("@DoctorId", medicalRecord.DoctorId);
                command.Parameters.AddWithValue("@PatientId", medicalRecord.PatientId);
                command.Parameters.AddWithValue("@DiagnosText", medicalRecord.DiagnoseName);

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

    public void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}