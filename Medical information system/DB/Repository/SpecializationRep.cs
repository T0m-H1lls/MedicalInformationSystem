using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class SpecializationRep:Base
{
    public SpecializationRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Specialization> GetSpec()
    {
        List<Specialization> specializationsList = new();

        string sql = @"select * from Specialization";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        specializationsList.Add(new Specialization()
                        {
                            Id =  reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return specializationsList;
    }

    public void Dispose()
    {
        CloseConnection();
        base.Dispose();
        
    }
}