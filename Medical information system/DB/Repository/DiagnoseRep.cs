using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class DiagnoseRep:Base
{
    public DiagnoseRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }


    public List<Diagnose> GetDiagnoses()
    {
        List<Diagnose> diagnoses = new();
        string sql = @"select * from `diagnoses`";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                using (var reader = cm.ExecuteReader())
                while (reader.Read())
                {
                    diagnoses.Add(new Diagnose()
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("Name"),
                        Description = reader.GetString("Description"),
                    });
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return diagnoses;
    }

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}