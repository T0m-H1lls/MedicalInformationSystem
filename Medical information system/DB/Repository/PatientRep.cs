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

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}