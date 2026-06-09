using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class StatisticRep:Base,IDisposable
{
    public StatisticRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }


    public List<Statistic> GetPatientColor(int? month = null, int? year = null)
    {
        List<Statistic> patientCount = new();
        string sql = @"SELECT d.FullName AS DoctorName, COUNT(p.Id) AS PatientCount
                       FROM doctors d
                       JOIN patients p ON d.Id = p.doctorId";
        if (month.HasValue)
        {
            sql += @" WHERE MONTH(p.CreatedAt) = @month";
            if (year.HasValue)
            {
                sql += @" AND YEAR(p.CreatedAt) = @year";
            }
        }
        sql += @" GROUP BY d.Id";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                if (month.HasValue)
                {
                    cm.Parameters.AddWithValue("@month", month.Value);
                    if (year.HasValue)
                    {
                        cm.Parameters.AddWithValue("@year", year.Value);
                    }
                }

                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patientCount.Add(new Statistic()
                        {
                            CountPatient = reader.GetInt32("PatientCount"),
                            DocFullName =  reader.GetString("DoctorName"),
                        });
                    }
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return patientCount;
    }

    public List<Statistic> GetActiveDoctorsCount(int? month = null, int? year = null)
    {
        List<Statistic> activeDoctorsCount = new();
        string sql = @"SELECT sp.Name AS SpecializationName, COUNT(d.Id) AS ActiveDoctorsCount
                      FROM doctors d
                      JOIN specialization sp ON d.SpecializationId = sp.Id
                      WHERE d.IsActive = 1";
        
        if (month.HasValue)
        {
            sql += @" AND MONTH(d.CreatedAt) = @month";
            if (year.HasValue)
            {
                sql += @" AND YEAR(d.CreatedAt) = @year";
            }
        }
    
        sql += @" GROUP BY d.SpecializationId";
        try
        {
            using (var  cm = new MySqlCommand(sql, connection))
            {
                if (month.HasValue)
                {
                    cm.Parameters.AddWithValue("@month", month.Value);
                    if (year.HasValue)
                    {
                        cm.Parameters.AddWithValue("@year", year.Value);
                    }
                }
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activeDoctorsCount.Add(new Statistic()
                        {
                            SpecializationName = reader.GetString("SpecializationName"),
                            ActiveDoctorsCount = reader.GetInt32("ActiveDoctorsCount")
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return activeDoctorsCount;
    }
    public List<Statistic> GetPatientsByGender(int? month = null, int? year = null)
    {
        List<Statistic> patientsByGender = new();
        string sql = @"SELECT `Gender`, COUNT(Id) AS PatientCount
                   FROM patients";
    
        if (month.HasValue)
        {
            sql += @" WHERE MONTH(CreatedAt) = @month";
            if (year.HasValue)
            {
                sql += @" AND YEAR(CreatedAt) = @year";
            }
        }
    
        sql += @" GROUP BY Gender;";
        try
        {
            using (var  cm = new MySqlCommand(sql, connection))
            {
                if (month.HasValue)
                {
                    cm.Parameters.AddWithValue("@month", month.Value);
                    if (year.HasValue)
                    {
                        cm.Parameters.AddWithValue("@year", year.Value);
                    }
                }
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patientsByGender.Add(new Statistic()
                        {
                            Gender = reader.GetString("Gender"),
                            PatientCount = reader.GetInt32("PatientCount")
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return patientsByGender;
    }
    

    public void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}