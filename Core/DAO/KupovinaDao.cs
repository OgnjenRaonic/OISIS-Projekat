using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.DAO;

public class KupovinaDao : IKupovinaDao
{
    private readonly string _putanja;
    private List<Kupovina> _kupovine;

    public KupovinaDao()
    {
        _putanja = Serijalizacija.Putanja("kupovine.json");
        _kupovine = Serijalizacija.Ucitaj<Kupovina>(_putanja);
    }

    public Kupovina? GetById(int id)
    {
        foreach (Kupovina kupovina in _kupovine)
        {
            if (kupovina.Id == id)
            {
                return kupovina;
            }
        }
        return null;
    }

    public List<Kupovina> GetAll()
    {
        return new List<Kupovina>(_kupovine);
    }

    public void Add(Kupovina kupovina)
    {
        kupovina.Id = SledeciId();
        _kupovine.Add(kupovina);
        Sacuvaj();
    }

    public void Update(Kupovina kupovina)
    {
        for (int i = 0; i < _kupovine.Count; i++)
        {
            if (_kupovine[i].Id == kupovina.Id)
            {
                _kupovine[i] = kupovina;
                Sacuvaj();
                return;
            }
        }
    }

    public void Delete(int id)
    {
        Kupovina? kupovina = GetById(id);
        if (kupovina != null)
        {
            _kupovine.Remove(kupovina);
            Sacuvaj();
        }
    }

    public void Sacuvaj()
    {
        Serijalizacija.Sacuvaj(_kupovine, _putanja);
    }

    private int SledeciId()
    {
        int max = 0;
        foreach (Kupovina kupovina in _kupovine)
        {
            if (kupovina.Id > max)
            {
                max = kupovina.Id;
            }
        }
        return max + 1;
    }
}
