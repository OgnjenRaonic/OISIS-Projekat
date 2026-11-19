using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class IzdavacService
{
    private readonly IIzdavacDao _izdavacDao;

    public IzdavacService(IIzdavacDao izdavacDao)
    {
        _izdavacDao = izdavacDao;
    }

    public Izdavac? GetById(int id)
    {
        return _izdavacDao.GetById(id);
    }

    public List<Izdavac> GetAll()
    {
        return _izdavacDao.GetAll();
    }

    public void Add(Izdavac izdavac)
    {
        DodajSefaUAutore(izdavac);
        _izdavacDao.Add(izdavac);
    }

    public void Update(Izdavac izdavac)
    {
        DodajSefaUAutore(izdavac);
        _izdavacDao.Update(izdavac);
    }

    public void Delete(int id)
    {
        _izdavacDao.Delete(id);
    }

    // Proverava da li vec postoji izdavac sa datom sifrom.
    // Izdavac sa ID-jem "preskociId" se ne racuna (koristi se kod izmene).
    public bool PostojiSifra(string sifra, int preskociId)
    {
        foreach (Izdavac izdavac in _izdavacDao.GetAll())
        {
            if (izdavac.Id != preskociId && izdavac.Sifra.ToLower() == sifra.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    // Sef izdavaca je jedan od autora tog izdavaca, pa mora biti i u spisku autora
    private void DodajSefaUAutore(Izdavac izdavac)
    {
        if (izdavac.SefId != 0 && !izdavac.AutorIds.Contains(izdavac.SefId))
        {
            izdavac.AutorIds.Add(izdavac.SefId);
        }
    }
}
