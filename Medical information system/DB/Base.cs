using System;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB;

public abstract class Base : IDisposable
{
    
    protected MySqlConnection connection;

    public Base(IOptions<DataBaseConnection> dataBaseConnection)
    {
        connection = new MySqlConnection(
            dataBaseConnection.Value.ConnectionString);
    }

    public bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
             return false;
        }
       
    }

    public bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        
    }
    
    public void Dispose()
    {
        connection.Dispose();
    }
}