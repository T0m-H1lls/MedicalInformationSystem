using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Tmds.DBus.Protocol;

namespace Medical_information_system.DB.Repository;

public class PatientRep:Base, IDisposable
{
    public PatientRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Patient> GetAllPatient(int doctorId,int? pageNumber = null, int? pageSize = null)
    {
        List<Patient> patients = new();
        string sql = @"SELECT * 
               FROM patients 
               WHERE doctorId = @doctorId AND IsActive = 1";
        if (pageNumber != null && pageSize != null)
        {
            sql+= " limit @limit offset @offset";
        }
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@doctorId", doctorId);
                if (pageNumber != null && pageSize != null)
                {
                    mc.Parameters.AddWithValue("@limit", pageSize);
                    mc.Parameters.AddWithValue("@offset", (pageNumber.Value - 1) * pageSize.Value);
                }
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient()
                        {
                             Id = reader.GetInt32("id"),
                             FullName = reader.GetString("FullName"),
                             BirthDate = reader.GetDateTimeOffset("BirthDate"),
                             Gender = reader.GetString("Gender"),
                             PhoneNumber = reader.GetString("Phone"),
                             Address = reader.GetString("Address"),
                             InsuranceNumber =  reader.GetString("InsuranceNumber"),
                             Passport = reader.GetString("Passport"),
                             Snils = reader.GetString("SNILS")
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return patients;
    }

    public int GetRowsCount(int doctorId)
    {
        string sql = @"SELECT COUNT(Id)
                        FROM patients
                        WHERE DoctorId = @doctorId AND IsActive = 1";

        try
        {
            using var mc = new MySqlCommand(sql, connection);
            mc.Parameters.AddWithValue("@doctorId", doctorId);

            return Convert.ToInt32(mc.ExecuteScalar());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }


    public void AddPatient(Patient patient)
    {
        string sql = @"insert into `patients` values(0,@FullName,@BirthDate,@Gender,@Phone,@Address,@InsuranceNumber,@Passport,@Snils,@doctorId,Null,1,@CreatedAt)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("FullName", patient.FullName);
                mc.Parameters.AddWithValue("BirthDate", patient.BirthDate);
                mc.Parameters.AddWithValue("Gender", patient.Gender);
                mc.Parameters.AddWithValue("Phone", patient.PhoneNumber);
                mc.Parameters.AddWithValue("Address", patient.Address);
                mc.Parameters.AddWithValue("InsuranceNumber", patient.InsuranceNumber);
                mc.Parameters.AddWithValue("Passport", patient.Passport);
                mc.Parameters.AddWithValue("Snils", patient.Snils);
                mc.Parameters.AddWithValue("doctorId",patient.DoctorId);
                mc.Parameters.AddWithValue("CreatedAt", DateTimeOffset.Now);
                mc.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
    }

    public bool UpdatePatient(Patient patient)
    {
        string sql = @"UPDATE patients 
                   SET FullName = @FullName,
                       BirthDate = @BirthDate,
                       Gender = @Gender,
                       Phone = @Phone,
                       Address = @Address,
                       InsuranceNumber = @InsuranceNumber,
                       Passport = @Passport,
                       SNILS = @Snils
                   WHERE Id = @Id";

        try
        {
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", patient.Id);
                command.Parameters.AddWithValue("@FullName", patient.FullName);
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Phone", patient.PhoneNumber);
                command.Parameters.AddWithValue("@Address", patient.Address);
                command.Parameters.AddWithValue("@InsuranceNumber", patient.InsuranceNumber);
                command.Parameters.AddWithValue("@Passport", patient.Passport);
                command.Parameters.AddWithValue("@Snils", patient.Snils);

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
    

    public bool DeletePatient(int id)
    {
        string sql = @"UPDATE patients
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