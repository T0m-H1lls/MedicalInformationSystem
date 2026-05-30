using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class DepartmentRep:Base, IDisposable
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
    
    public bool AddDepartment(Departments department)
    {
        string sql = @"INSERT INTO departments(Name, Floor)
                   VALUES(@Name, @Floor)";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Name", department.Name);
                cm.Parameters.AddWithValue("@Floor", department.Floor);

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
    
    public bool UpdateDepartment(Departments department)
    {
        string sql = @"UPDATE departments
                   SET Name = @Name,
                       Floor = @Floor
                   WHERE Id = @Id";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Id", department.Id);
                cm.Parameters.AddWithValue("@Name", department.Name);
                cm.Parameters.AddWithValue("@Floor", department.Floor);

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
    public bool DeleteDepartment(int id)
    {
        string sql = @"UPDATE departments
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
       connection.Close();
        base.Dispose();
       
    }
}