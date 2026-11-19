using Core.Models;

namespace Core.Interfaces;

public interface IIzdavacDao
{
    Izdavac? GetById(int id);

    List<Izdavac> GetAll();

    void Add(Izdavac izdavac);

    void Update(Izdavac izdavac);

    void Delete(int id);

    void Sacuvaj();
}
