using Core.Models;
namespace Core.Interfaces;

public interface IAdresaDao
{
    Adresa? GetById(int id);

    List<Adresa> GetAll();

    void Add(Adresa adresa);

    void Update(Adresa adresa);

    void Delete(int id);
}
