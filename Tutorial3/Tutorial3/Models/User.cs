using Tutorial3.Enums;

namespace Tutorial3.Models;

public abstract class User
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType Type { get; }

    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName, UserType type)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Type = type;
    }

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"[{Id}] {FullName} ({Type}) - Max rentals: {MaxActiveRentals}";
    }
}
