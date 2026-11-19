using Core.Interfaces;
using Core.Models;

namespace Core.DAO;

public class AdresaDao : IAdresaDao
{
    private readonly List<Adresa> _adrese = new();

    public Adresa? GetById(int id)
    {
        return _adrese.FirstOrDefault(a => a.Id == id);
    }

    public List<Adresa> GetAll()
    {
        return _adrese.ToList();
    }

    public void Add(Adresa adresa)
    {
        if (adresa.Id <= 0 ||
            _adrese.Any(a => a.Id == adresa.Id))
        {
            adresa.Id = _adrese.Count == 0
                ? 1
                : _adrese.Max(a => a.Id) + 1;
        }

        _adrese.Add(adresa);
    }

    public void Update(Adresa adresa)
    {
        int index = _adrese.FindIndex(a => a.Id == adresa.Id);

        if (index >= 0)
        {
            _adrese[index] = adresa;
        }
    }

    public void Delete(int id)
    {
        Adresa? adresa = GetById(id);

        if (adresa != null)
        {
            _adrese.Remove(adresa);
        }
    }
}
