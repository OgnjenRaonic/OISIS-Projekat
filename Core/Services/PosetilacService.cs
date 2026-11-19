using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class PosetilacService
{
    private readonly IPosetilacDao _posetilacDao;
    private readonly IKupovinaDao _kupovinaDao;

    public PosetilacService(IPosetilacDao posetilacDao, IKupovinaDao kupovinaDao)
    {
        _posetilacDao = posetilacDao;
        _kupovinaDao = kupovinaDao;
    }

    public Posetilac? GetById(int id)
    {
        return _posetilacDao.GetById(id);
    }

    public List<Posetilac> GetAll()
    {
        return _posetilacDao.GetAll();
    }

    public void Add(Posetilac posetilac)
    {
        // novi posetilac jos nema kupovina
        posetilac.KupovinaIds = new List<int>();
        posetilac.ProsecnaOcena = 0;
        _posetilacDao.Add(posetilac);
    }

    public void Update(Posetilac posetilac)
    {
        _posetilacDao.Update(posetilac);
    }

    // Brise posetioca, a zajedno sa njim i sve njegove kupovine
    public void Delete(int id)
    {
        List<Kupovina> kupovine = _kupovinaDao.GetAll();
        foreach (Kupovina kupovina in kupovine)
        {
            if (kupovina.PosetilacId == id)
            {
                _kupovinaDao.Delete(kupovina.Id);
            }
        }

        _posetilacDao.Delete(id);
    }

    // Proverava da li vec postoji posetilac sa datim brojem clanske karte.
    // Posetilac sa ID-jem "preskociId" se ne racuna (koristi se kod izmene).
    public bool PostojiBrojClanskeKarte(string brojClanskeKarte, int preskociId)
    {
        foreach (Posetilac posetilac in _posetilacDao.GetAll())
        {
            if (posetilac.Id != preskociId &&
                posetilac.BrojClanskeKarte.ToLower() == brojClanskeKarte.ToLower())
            {
                return true;
            }
        }
        return false;
    }
}
