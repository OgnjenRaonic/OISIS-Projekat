using Core.Models;
namespace Core.Interfaces;

public interface IAutorDao
{
    Autor? GetById(int id);

    List<Autor> GetAll();

    void Add(Autor autor);

    void Update(Autor autor);

    void Delete(int id);
}
