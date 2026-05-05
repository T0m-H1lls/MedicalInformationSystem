using Medical_information_system.DB;
using Microsoft.Extensions.Options;

namespace Medical_information_system.Models;

public class Departments
{
    public int Id {get; set;}
    public string Name {get; set;}
    public int Floor {get; set;}

}