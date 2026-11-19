using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class AutorService
{
    private readonly IAutorDao _autorDao;

    public AutorService(IAutorDao autorDao)
    {
        _autorDao = autorDao;
    }

    public Autor? GetById(int id) => _autorDao.GetById(id);

    public List<Autor> GetAll() => _autorDao.GetAll();

    public void Add(Autor autor)
    {
        _autorDao.Add(autor);
    }

    public void Update(Autor autor)
    {
        _autorDao.Update(autor);
    }

    public void Delete(int id)
    {
        _autorDao.Delete(id);
    }
}