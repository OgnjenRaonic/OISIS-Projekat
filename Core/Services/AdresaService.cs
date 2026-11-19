using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class AdresaService
{
    private readonly IAdresaDao _adresaDao;

    public AdresaService(IAdresaDao adresaDao)
    {
        _adresaDao = adresaDao;
    }

    public Adresa? GetById(int id) => _adresaDao.GetById(id);

    public List<Adresa> GetAll() => _adresaDao.GetAll();

    public void Add(Adresa adresa)
    {
        _adresaDao.Add(adresa);
    }

    public void Update(Adresa adresa)
    {
        _adresaDao.Update(adresa);
    }

    public void Delete(int id)
    {
        _adresaDao.Delete(id);
    }
}