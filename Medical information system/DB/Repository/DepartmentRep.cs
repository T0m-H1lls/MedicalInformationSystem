using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class DepartmentRep:Base
{
    public DepartmentRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Departments> GetDepartments()
    {
        List<Departments> departmentsList = new();
        string sql = "select * from `departments`";
        try
        {
            using (var cm = new MySqlCommand(sql,connection))
            {
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departmentsList.Add(new Departments()
                        {
                           Id = reader.GetInt32("Id"),
                           Name = reader.GetString("Name"),
                           Floor = reader.GetInt32("Floor")
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return departmentsList;
    }

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}