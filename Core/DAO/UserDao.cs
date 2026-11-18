using Core.Interfaces;
using Core.Models;
using System.IO;

namespace Core.DAO;

public class UserDao : IUserDao
{
    // TODO: zameniti čitanjem iz fajla / baze
    private readonly List<User> _users =
    [
        new User { Id = 1, Name = "admin" },
    ];

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
}
