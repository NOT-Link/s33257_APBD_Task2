using Tutorial3.Enums;

namespace Tutorial3.Models;

public class Student : User
{
    public string StudentNumber { get; set; }
    public override int MaxActiveRentals => 2;

    public Student(string firstName, string lastName, string studentNumber)
        : base(firstName, lastName, UserType.Student)
    {
        StudentNumber = studentNumber;
    }
}
