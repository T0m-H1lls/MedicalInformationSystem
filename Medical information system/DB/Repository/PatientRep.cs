using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Tmds.DBus.Protocol;

namespace Medical_information_system.DB.Repository;

public class PatientRep:Base
{
    public PatientRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Patient> GetAllPatient(int doctorId)
    {
        List<Patient> patients = new();
        string sql = @"Select * from `patients` where `doctorId` = @doctorId";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@doctorId", doctorId);
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient()
                        {
                             Id = reader.GetInt32("id"),
                             FullName = reader.GetString("FullName"),
                             DateOfBirth = reader.GetDateOnly("BirthDate"),
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


    public void AddPatient(Patient patient)
    {
        string sql = @"insert into `patients` values(0,@FullName,@DateOfBirth,@Gender,@PhoneNumber,@Address,@InsuranceNumber,@Passport,@Snils)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("FullName", patient.FullName);
                mc.Parameters.AddWithValue("DateOfBirth", patient.DateOfBirth);
                mc.Parameters.AddWithValue("Gender", patient.Gender);
                mc.Parameters.AddWithValue("PhoneNumber", patient.PhoneNumber);
                mc.Parameters.AddWithValue("Address", patient.Address);
                mc.Parameters.AddWithValue("InsuranceNumber", patient.InsuranceNumber);
                mc.Parameters.AddWithValue("Passport", patient.Passport);
                mc.Parameters.AddWithValue("Snils", patient.Snils);
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
        string sql = @"update `patients` set Fullname = @FullName, DateOfBirth=@DateOfBirth,
                       Gender=@Gender,PhoneNumber=@PhoneNumber,Address=@Address,
                       InsuranceNumber=@InsuranceNumber
                       where id=@id";
        try
        {
             using (var command = new MySqlCommand(sql, connection))
             {
                 command.Parameters.AddWithValue("@FullName",patient.FullName);
                 command.Parameters.AddWithValue("@DateOfBirth",patient.DateOfBirth);
                 command.Parameters.AddWithValue("@Gender",patient.Gender);
                 command.Parameters.AddWithValue("@PhoneNumber",patient.PhoneNumber);
                 command.Parameters.AddWithValue("@Address",patient.Address);
                 command.Parameters.AddWithValue("@InsuranceNumber",patient.InsuranceNumber);
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
    

    public bool DeletePatient(int Id)
    {
        string sql = @"delete from `patients` where `Id` = @Id";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id",Id);
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
        base.Dispose();
        CloseConnection();
    }
}