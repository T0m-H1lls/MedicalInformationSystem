using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class DoctorRep:Base
{
    public DoctorRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Doctor> GetDoctors()
    {
        List<Doctor> doctors = new();
        string sql = @"SELECT d.Id ,d.FullName,d.Phone,d.Specialization,d.Room,d.DepartmentId, d2.Name as DepartmentName
                       from doctors d 
                       JOIN departments d2 ON d.DepartmentId = d2.Id ";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                var reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    doctors.Add(new Doctor()
                    {
                        Id = reader.GetInt32("id"),
                        FullName = reader.GetString("FullName"),
                        PhoneNumber = reader.GetString("Phone"),
                        Speciality =  reader.GetString("Specialization"),
                        Room = reader.GetString("Room"),
                        DepartmentId = reader.GetInt32("DepartmentId"),
                        DepartmentName = reader.GetString("DepartmentName"),
                    });
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return doctors;
    }

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}