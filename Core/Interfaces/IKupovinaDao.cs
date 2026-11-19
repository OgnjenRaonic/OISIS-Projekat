using Core.Models;

namespace Core.Interfaces;

public interface IKupovinaDao
{
    Kupovina? GetById(int id);

    List<Kupovina> GetAll();

    void Add(Kupovina kupovina);

    void Update(Kupovina kupovina);

    void Delete(int id);

    void Sacuvaj();
}
