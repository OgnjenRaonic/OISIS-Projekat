using Core.Models;
namespace Core.Interfaces;

public interface IKnjigaDao
{
    Knjiga? GetById(int id);

    List<Knjiga> GetAll();

    void Add(Knjiga knjiga);

    void Update(Knjiga knjiga);

    void Delete(int id);
}
