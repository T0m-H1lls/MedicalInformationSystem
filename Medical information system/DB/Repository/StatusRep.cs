using System;
using System.Collections.Generic;
using Medical_information_system.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Medical_information_system.DB.Repository;

public class StatusRep:Base
{
    public StatusRep(IOptions<DataBaseConnection> dataBaseConnection) : base(dataBaseConnection)
    {
        OpenConnection();
    }

    public List<Status> StatusList()
    {
        List<Status> statusList = new();
        string sql =@"select * from `status`";
        try
        {
            using (var cm = new MySqlCommand(sql, connection))
            {
                using (var rdr = cm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        statusList.Add(new Status()
                        {
                            Id = rdr.GetInt32("Id"),
                            Name =  rdr.GetString("Name")
                            
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return statusList;
    }
}