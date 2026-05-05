using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;

namespace Medical_information_system.DB.Repository;

public class MedicalRecordRep:Base
{
    public MedicalRecordRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }
    
    public List<MedicalRecord> GetDiagnoses()
    {
        List<MedicalRecord> diagnosesList = new();
        string sql = @"select m.Id, m.PatientId, m.AppointmentId,m.DiagnosisId,m.Description,m.RecordDate 
                       from medicalrecords m 
                       join patients p ON m.PatientId = p.Id
                       join appointments a ON m.AppointmentId  = a.Id 
                       join diagnoses d ON m.DiagnosisId  = d.Id ";
        try
        {

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return diagnosesList;
    }
}