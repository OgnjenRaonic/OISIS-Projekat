using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class KupovinaService
{
    private readonly IKupovinaDao _kupovinaDao;
    private readonly IPosetilacDao _posetilacDao;

    public KupovinaService(IKupovinaDao kupovinaDao, IPosetilacDao posetilacDao)
    {
        _kupovinaDao = kupovinaDao;
        _posetilacDao = posetilacDao;
    }

    public Kupovina? GetById(int id)
    {
        return _kupovinaDao.GetById(id);
    }

    public List<Kupovina> GetAll()
    {
        return _kupovinaDao.GetAll();
    }

    // Dodaje kupovinu i povezuje je sa posetiocem
    public void Add(Kupovina kupovina)
    {
        _kupovinaDao.Add(kupovina);

        Posetilac? posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
        if (posetilac != null)
        {
            posetilac.KupovinaIds.Add(kupovina.Id);
            // kupljena knjiga vise nije na listi zelja
            posetilac.ListaZeljaIds.Remove(kupovina.KnjigaId);
            posetilac.ProsecnaOcena = IzracunajProsecnuOcenu(posetilac.Id);
            _posetilacDao.Update(posetilac);
        }
    }

    public void Update(Kupovina kupovina)
    {
        _kupovinaDao.Update(kupovina);

        // ocena je mozda promenjena, pa se ponovo racuna prosek
        Posetilac? posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
        if (posetilac != null)
        {
            posetilac.ProsecnaOcena = IzracunajProsecnuOcenu(posetilac.Id);
            _posetilacDao.Update(posetilac);
        }
    }

    // Brise kupovinu i uklanja je iz liste kupovina posetioca
    public void Delete(int id)
    {
        Kupovina? kupovina = _kupovinaDao.GetById(id);
        if (kupovina == null)
        {
            return;
        }

        _kupovinaDao.Delete(id);

        Posetilac? posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
        if (posetilac != null)
        {
            posetilac.KupovinaIds.Remove(id);
            posetilac.ProsecnaOcena = IzracunajProsecnuOcenu(posetilac.Id);
            _posetilacDao.Update(posetilac);
        }
    }

    // Proverava da li je posetilac vec kupio datu knjigu
    public bool PosetilacVecKupio(int posetilacId, int knjigaId)
    {
        foreach (Kupovina kupovina in _kupovinaDao.GetAll())
        {
            if (kupovina.PosetilacId == posetilacId && kupovina.KnjigaId == knjigaId)
            {
                return true;
            }
        }
        return false;
    }

    private double IzracunajProsecnuOcenu(int posetilacId)
    {
        int zbir = 0;
        int broj = 0;

        foreach (Kupovina kupovina in _kupovinaDao.GetAll())
        {
            if (kupovina.PosetilacId == posetilacId)
            {
                zbir = zbir + kupovina.Ocena;
                broj = broj + 1;
            }
        }

        if (broj == 0)
        {
            return 0;
        }
        return (double)zbir / broj;
    }
}
