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


    public List<Statistic> GetPatientColor()
    {
        List<Statistic> patientCount = new();
        string sql = @"SELECT d.FullName AS DoctorName, COUNT(p.Id) AS PatientCount
                       FROM doctors d
                       JOIN patients p ON d.Id = p.doctorId
                       GROUP BY d.Id";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
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

    public List<Statistic> GetActiveDoctorsCount()
    {
        List<Statistic> activeDoctorsCount = new();
        string sql = @"SELECT sp.Name AS SpecializationName, COUNT(d.Id) AS ActiveDoctorsCount
                      FROM doctors d
                      JOIN Specialization sp ON d.SpecializationId = sp.Id
                      WHERE d.IsActive = 1
                      GROUP BY d.SpecializationId";
        try
        {
            using (var  cm = new MySqlCommand(sql, connection))
            {
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
    public List<Statistic> GetPatientsByGender()
    {
        List<Statistic> patientsByGender = new();
        string sql = @"SELECT Gender, COUNT(Id) AS PatientCount
                        FROM patients
                        GROUP BY Gender;";
        try
        {
            using (var  cm = new MySqlCommand(sql, connection))
            {
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