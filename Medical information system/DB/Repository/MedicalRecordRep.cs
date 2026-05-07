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
        string sql = @"select m.Id, m.PatientId, m.AppointmentId,m.DiagnosisId,m.Description,m.RecordDate,p.FullName as PatientName,d2.FullName,d.Name as DiagnoseName,m3.Name as MedicineName, m.RecordDate 
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
                var reader = rep.ExecuteReader();
                while (reader.Read())
                {
                    diagnosesList.Add(new MedicalRecord()
                    {
                        Id = reader.GetInt32("Id"),
                        PatientId = reader.GetInt32("PatientId"),
                        AppointmentId = reader.GetInt32("AppointmentId"),
                        DiagnoseId =  reader.GetInt32("DiagnosisId"),
                        Description = reader.GetString("Description"),
                        RecordDate = reader.GetDateTime("RecordDate"),
                        PatientName = reader.GetString("PatientName"),
                        DoctorName =  reader.GetString("FullName"),
                        DiagnoseName = reader.GetString("DiagnoseName"),
                        MedicineName = reader.GetString("MedicineName")
                    });
                }
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return diagnosesList;
    }
}