using Core.Models;

namespace Core.Interfaces;

public interface IPosetilacDao
{
    Posetilac? GetById(int id);

    List<Posetilac> GetAll();

    void Add(Posetilac posetilac);

    void Update(Posetilac posetilac);

    void Delete(int id);

    void Sacuvaj();
}
