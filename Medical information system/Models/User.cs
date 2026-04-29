namespace Medical_information_system.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Password { get; set; }
    public string Login { get; set; }
    public string Role { get; set; }
}