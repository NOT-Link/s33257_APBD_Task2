using Tutorial3.Models;

namespace Tutorial3.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public IReadOnlyList<User> GetAll()
    {
        return _users.AsReadOnly();
    }
}
