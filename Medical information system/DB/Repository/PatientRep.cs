using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;

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
        string sql = @"";
        try
        {

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