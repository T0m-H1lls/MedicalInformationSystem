using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class DoctorRep:Base, IDisposable
{
    public DoctorRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Doctor> GetDoctors(int? pageNumber = null, int? pageSize = null )
    {
        List<Doctor> doctors = new();
        string sql =
            @"SELECT d.Id ,d.FullName,d.Phone,d.SpecializationId,d.Room,d.DepartmentId, d2.Name as DepartmentName,s.Name as Spec
                       from doctors d 
                       JOIN departments d2 ON d.DepartmentId = d2.Id
                       join specialization s on d.SpecializationId =  s.Id
                       Where d.IsActive = 1";
            if (pageNumber != null && pageSize != null)
            {
                sql+= " limit @limit offset @offset";
            }
            
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                if (pageNumber != null && pageSize != null)
                {
                    cm.Parameters.AddWithValue("@limit", pageSize);
                    cm.Parameters.AddWithValue("@offset", (pageNumber.Value - 1) * pageSize.Value);
                }
                using (var reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(new Doctor()
                        {
                            Id = reader.GetInt32("id"),
                            PhoneNumber = reader.GetString("Phone"),
                            Speciality = reader.GetString("Spec"),
                            Room = reader.GetString("Room"),
                            DepartmentId = reader.GetInt32("DepartmentId"),
                            DepartmentName = reader.GetString("DepartmentName"),
                            SpecialtyId = reader.GetInt32("SpecializationId"),
                            FullName = reader.GetString("FullName"),
                           
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        return doctors;
    }
    public int GetRowsCount()
    {
        string sql = @"SELECT COUNT(Id)
                        FROM `doctors`
                        Where IsActive = 1";

        try
        {
            using var mc = new MySqlCommand(sql, connection);
            return Convert.ToInt32(mc.ExecuteScalar());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }

    public bool AddDoctor(Doctor doctor)
    {
        string sql = @"INSERT INTO `doctors` VALUES(0,@FullName, @SpecializationId,@Phone, @Room, @DepartmentId,null,1,@CreatedAt)";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@FullName", doctor.FullName);
                cm.Parameters.AddWithValue("@Phone", doctor.PhoneNumber);
                cm.Parameters.AddWithValue("@SpecializationId", doctor.SpecialtyId);
                cm.Parameters.AddWithValue("@Room", doctor.Room);
                cm.Parameters.AddWithValue("@DepartmentId", doctor.DepartmentId);
                cm.Parameters.AddWithValue("@CreatedAt", DateTimeOffset.Now);

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
    public bool UpdateDoctor(Doctor doctor)
    {
        string sql = @"UPDATE doctors
                   SET FullName = @FullName,
                       Phone = @Phone,
                       SpecializationId = @SpecializationId,
                       Room = @Room,
                       DepartmentId = @DepartmentId
                   WHERE Id = @Id";

        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                cm.Parameters.AddWithValue("@Id", doctor.Id);
                cm.Parameters.AddWithValue("@FullName", doctor.FullName);
                cm.Parameters.AddWithValue("@Phone", doctor.PhoneNumber);
                cm.Parameters.AddWithValue("@SpecializationId", doctor.SpecialtyId);
                cm.Parameters.AddWithValue("@Room", doctor.Room);
                cm.Parameters.AddWithValue("@DepartmentId", doctor.DepartmentId);

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
    public bool DeleteDoctor(int id)
    {
        string sql = @"UPDATE doctors
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