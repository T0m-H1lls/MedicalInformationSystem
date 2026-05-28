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

    public List<Appointments> GetAppointments(int id)
    {
        List<Appointments> appointmentsList = new();
        string sql = @"SELECT a.id,a.PatientId,a.DoctorId,a.AppointmentDate,a.StatusId,a.ReferralDoctorId,p.FullName as PatientName,d.FullName as DoctorFullName,d2.FullName as DocFullNameReferal,s.Name  as Status
                       from appointments a
                       Left join patients p ON a.PatientId = p.Id 
                       Left join doctors d ON a.DoctorId = d.Id
                       Left join doctors d2 on a.ReferralDoctorId =d2.Id 
                       Left join status s  on a.StatusId  = s.id 
                       Where a.DoctorId  = @id AND a.IsActive = 1";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("id", id);
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int recId = 0;
                        if (!reader.IsDBNull(5))
                            recId = reader.GetInt32("ReferralDoctorId");
                        
                        string refDoctorName = null;
                        if (!reader.IsDBNull(8))
                            refDoctorName = reader.GetString("DocFullNameReferal");
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
                           ReferralDoctorId = recId,
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
                   (PatientId,DoctorId,AppointmentDate,StatusId,ReferralDoctorId,IsActive,DeletedAt)
                   VALUES
                   (@PatientId,@DoctorId,@AppointmentDate,@StatusId,@ReferralDoctorId,1,null)";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cm.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cm.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cm.Parameters.AddWithValue("@StatusId", appointment.StatusId);
                cm.Parameters.AddWithValue("@ReferralDoctorId", appointment.ReferralDoctorId);

                
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
                       StatusId = @StatusId,
                       ReferralDoctorId = @ReferralDoctorId
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
                if (appointment.ReferralDoctorId == null)
                    cm.Parameters.AddWithValue("@ReferralDoctorId", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReferralDoctorId", appointment.ReferralDoctorId);


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