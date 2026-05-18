using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class UserRep:Base
{
    public UserRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public void AddUser(User user)
    {
        string sql = @"insert into `users`values(0,@Login,@Password,@RoleId,@doctorId)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
             mc.Parameters.AddWithValue("@Login", user.Login);
             mc.Parameters.AddWithValue("@Password", user.Password);
             mc.Parameters.AddWithValue("@RoleId", user.RoleId);
             mc.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
    }

    public List<User> CheckUser(string Login,string Password)
    {
        
        List<User> users = new();
        string sql =  @"select * from `users` where `Login`= @Login and `Password`= @Password";
        try
        {
            using (var mc = new MySqlCommand(sql, connection)) 
            {
                mc.Parameters.AddWithValue("@Login", Login);
                mc.Parameters.AddWithValue("@Password", Password);
                using ( var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User()
                        {
                            Id = reader.GetInt32("Id"),
                            Login = reader.GetString("Login"),
                            Password = reader.GetString("Password"),
                            RoleId = reader.GetInt32("RoleId")
                           
                        });
                    }   
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return users;
    }

    public List<User> GetFullNameAndRole(string Login, string Password)
    {
        List<User> usersList = new();
        string sql = @"select u.Id ,u.doctorId ,u.RoleId,s.Name as Role,u.Name,u.Surname,u.Patronymic
                        from users u 
                        join Specialization s on u.RoleId  = s.Id
                        where `Login`= @Login and `Password`= @Password";   
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Login", Login);
                mc.Parameters.AddWithValue("@Password", Password);
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usersList.Add(new User()
                        {
                            Id = reader.GetInt32("Id"),
                            RoleId = reader.GetInt32("RoleId"),
                            Role = reader.GetString("Role"),
                            DoctorId = reader.GetInt32("doctorId"),
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            Patronymic = reader.GetString("Patronymic")
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return usersList;
    }
    

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}