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
        string sql = @"SELECT a.id,a.PatientId,a.DoctorId,a.AppointmentDate,a.MedicalRecordId,a.StatusId,p.FullName as PatientName,d.FullName as DoctorFullName,d2.FullName as DocFullNameReferall,s.Name as Status
                       from appointments a
                       join patients p ON a.PatientId = p.Id 
                       join doctors d ON a.DoctorId = d.Id
                       LEFT  join medicalrecords m ON a.MedicalRecordId = m.Id
                       LEFT  join appointments a2 on m.AppointmentId = a2.Id 
                       LEFT  join doctors d2 on d2.id = a2.DoctorId 
                       Left Join status s ON a.StatusId = s.id";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int recId = 0;
                        if (!reader.IsDBNull(4))
                            recId = reader.GetInt32("MedicalRecordId");
                        
                        string refDoctorName = null;
                        if (!reader.IsDBNull(8))
                            refDoctorName = reader.GetString("DocFullNameReferall");
                        string refDoctorSurname = null;
                        
                        appointmentsList.Add(new Appointments()
                        {
                           Id = reader.GetInt32("id"),//e
                           PatientId = reader.GetInt32("PatientId"),//e
                           DoctorId = reader.GetInt32("DoctorId"),//e
                           AppointmentDate = reader.GetDateTimeOffset("AppointmentDate"),//e
                           StatusId = reader.GetInt32("StatusId"),//e
                           PatientName = reader.GetString("PatientName"),//e
                           DoctorFullName = reader.GetString("DoctorFullName"),//e
                           MedicalRecordID = recId,//e
                           DocFullNameReferall =  refDoctorName,
                           Status = reader.GetString("Status")
                           
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

    
    public bool AddAppointment(Appointments appointment)
    {
        string sql = @"INSERT INTO appointments
                   (PatientId, DoctorId, AppointmentDate, MedicalRecordId, StatusId)
                   VALUES
                   (@PatientId, @DoctorId, @AppointmentDate, @MedicalRecordId, @StatusId)";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cm.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cm.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cm.Parameters.AddWithValue("@MedicalRecordId", appointment.MedicalRecordID);
                cm.Parameters.AddWithValue("@StatusId", appointment.StatusId);

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
    
    public bool UpdateAppointment(Appointments appointment)
    {
        string sql = @"UPDATE appointments
                   SET PatientId = @PatientId,
                       DoctorId = @DoctorId,
                       AppointmentDate = @AppointmentDate,
                       StatusId = @StatusId
                   WHERE Id = @Id";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Id", appointment.Id);
                cm.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cm.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cm.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cm.Parameters.AddWithValue("@StatusId", appointment.StatusId);

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
    public bool DeleteAppointment(int id)
    {
        string sql = @"UPDATE appointments
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
        base.Dispose();
        CloseConnection();
    }
}