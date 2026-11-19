using Core.Interfaces;
using Core.Models;

namespace Core.DAO;

public class AutorDao : IAutorDao
{
    private readonly List<Autor> _autori = new();

    public Autor? GetById(int id)
    {
        return _autori.FirstOrDefault(a => a.Id == id);
    }

    public List<Autor> GetAll()
    {
        return _autori.ToList();
    }

    public void Add(Autor autor)
    {
        if (autor.Id <= 0 ||
            _autori.Any(a => a.Id == autor.Id))
        {
            autor.Id = _autori.Count == 0
                ? 1
                : _autori.Max(a => a.Id) + 1;
        }

        _autori.Add(autor);
    }

    public void Update(Autor autor)
    {
        int index = _autori.FindIndex(a => a.Id == autor.Id);

        if (index >= 0)
        {
            _autori[index] = autor;
        }
    }

    public void Delete(int id)
    {
        Autor? autor = GetById(id);

        if (autor != null)
        {
            _autori.Remove(autor);
        }
    }
}
