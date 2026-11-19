using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class KnjigaService
{
    private readonly IKnjigaDao _knjigaDao;

    public KnjigaService(IKnjigaDao knjigaDao)
    {
        _knjigaDao = knjigaDao;
    }

    public Knjiga? GetById(int id) => _knjigaDao.GetById(id);

    public List<Knjiga> GetAll() => _knjigaDao.GetAll();

    public void Add(Knjiga knjiga)
    {
        _knjigaDao.Add(knjiga);
    }

    public void Update(Knjiga knjiga)
    {
        _knjigaDao.Update(knjiga);
    }

    public void Delete(int id)
    {
        _knjigaDao.Delete(id);
    }
}