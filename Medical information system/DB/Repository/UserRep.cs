using Medical_information_system.Models;
using Microsoft.Extensions.Options;

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
        
    }
}