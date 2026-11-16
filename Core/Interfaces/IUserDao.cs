using Core.Models;

namespace Core.Interfaces;

public interface IUserDao
{
    User? GetById(int id);
}
