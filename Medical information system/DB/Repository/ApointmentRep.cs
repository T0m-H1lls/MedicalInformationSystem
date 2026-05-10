using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class ApointmentRep:Base
{
    public ApointmentRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Appointments> GetAppointments()
    {
        List<Appointments> appointmentsList = new();
        string sql = @"SELECT a.id,a.PatientId,a.DoctorId,a.AppointmentDate,a.Status,p.FullName as PatientName,d.FullName as DoctorName
                       from appointments a
                       join patients p ON a.PatientId = p.Id 
                       join doctors d ON a.DoctorId = d.Id  ";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointmentsList.Add(new Appointments()
                        {
                           Id = reader.GetInt32("id"),
                           PatientId = reader.GetInt32("PatientId"),
                           DoctorId = reader.GetInt32("DoctorId"),
                           AppointmentDate = reader.GetDateTime("AppointmentDate"),
                           Status = reader.GetString("Status"),
                           PatientName = reader.GetString("PatientName"),
                           DoctorName = reader.GetString("DoctorName"),
                        });
                        
                    }
                    
                }
                
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return appointmentsList;
    }

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}