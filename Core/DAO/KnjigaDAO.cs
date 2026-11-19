using Core.Interfaces;
using Core.Models;

namespace Core.DAO;

public class KnjigaDao : IKnjigaDao
{
    private readonly List<Knjiga> _knjige = new();

    public Knjiga? GetById(int id)
    {
        return _knjige.FirstOrDefault(k => k.Id == id);
    }

    public List<Knjiga> GetAll()
    {
        return _knjige.ToList();
    }

    public void Add(Knjiga knjiga)
    {
        if (knjiga.Id <= 0 ||
            _knjige.Any(k => k.Id == knjiga.Id))
        {
            knjiga.Id = _knjige.Count == 0
                ? 1
                : _knjige.Max(k => k.Id) + 1;
        }

        _knjige.Add(knjiga);
    }

    public void Update(Knjiga knjiga)
    {
        int index = _knjige.FindIndex(k => k.Id == knjiga.Id);

        if (index >= 0)
        {
            _knjige[index] = knjiga;
        }
    }

    public void Delete(int id)
    {
        Knjiga? knjiga = GetById(id);

        if (knjiga != null)
        {
            _knjige.Remove(knjiga);
        }
    }
}
