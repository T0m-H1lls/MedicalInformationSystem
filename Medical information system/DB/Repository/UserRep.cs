using System;
using System.Collections.Generic;
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
        string sql = @"insert into `users`values(0,@Name,@Surname,@Patronymic,@Login,@Password,@Role)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("Name", user.Name);
                mc.Parameters.AddWithValue("Surname", user.Surname);
                mc.Parameters.AddWithValue("Patronymic", user.Patronymic);
                mc.Parameters.AddWithValue("Login", user.Login);
                mc.Parameters.AddWithValue("Password", user.Password);
                mc.Parameters.AddWithValue("Role", user.Role);
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
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            Patronymic = reader.GetString("Patronymic"),
                            Login = reader.GetString("Login"),
                            Password = reader.GetString("Password"),
                            Role = reader.GetString("Role")
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

    public void Dispose()
    {
        base.Dispose();
        CloseConnection();
    }
}