using Tutorial3.Enums;

namespace Tutorial3.Models;

public class Employee : User
{
    public string Department { get; set; }
    public override int MaxActiveRentals => 5;

    public Employee(string firstName, string lastName, string department)
        : base(firstName, lastName, UserType.Employee)
    {
        Department = department;
    }
}
