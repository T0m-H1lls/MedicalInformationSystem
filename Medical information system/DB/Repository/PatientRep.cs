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

    public List<Patient> GetAllPatient()
    {
        List<Patient> patients = new();
        string sql = @"Select * from `patients`";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
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
        string sql = @"insert into `patient` values(0,@FullName,@DateOfBirth,@Gender,@PhoneNumber,@Address,@InsuranceNumber)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("Id", patient.Id);
                mc.Parameters.AddWithValue("FullName", patient.FullName);
                mc.Parameters.AddWithValue("DateOfBirth", patient.DateOfBirth);
                mc.Parameters.AddWithValue("Gender", patient.Gender);
                mc.Parameters.AddWithValue("PhoneNumber", patient.PhoneNumber);
                mc.Parameters.AddWithValue("Address", patient.Address);
                mc.Parameters.AddWithValue("InsuranceNumber", patient.InsuranceNumber);
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
        string sql = @"update `product` set Fullname = @FullName, DateOfBirth=@DateOfBirth,
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
                 command.Parameters.AddWithValue("@id", patient.Id);
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
        string sql = @"delete from `patient` where `id` = @id";
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

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}