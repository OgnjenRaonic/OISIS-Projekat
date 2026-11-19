using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.DAO;

public class IzdavacDao : IIzdavacDao
{
    private readonly string _putanja;
    private List<Izdavac> _izdavaci;

    public IzdavacDao()
    {
        _putanja = Serijalizacija.Putanja("izdavaci.json");
        _izdavaci = Serijalizacija.Ucitaj<Izdavac>(_putanja);
    }

    public Izdavac? GetById(int id)
    {
        foreach (Izdavac izdavac in _izdavaci)
        {
            if (izdavac.Id == id)
            {
                return izdavac;
            }
        }
        return null;
    }

    public List<Izdavac> GetAll()
    {
        return new List<Izdavac>(_izdavaci);
    }

    public void Add(Izdavac izdavac)
    {
        izdavac.Id = SledeciId();
        _izdavaci.Add(izdavac);
        Sacuvaj();
    }

    public void Update(Izdavac izdavac)
    {
        for (int i = 0; i < _izdavaci.Count; i++)
        {
            if (_izdavaci[i].Id == izdavac.Id)
            {
                _izdavaci[i] = izdavac;
                Sacuvaj();
                return;
            }
        }
    }

    public void Delete(int id)
    {
        Izdavac? izdavac = GetById(id);
        if (izdavac != null)
        {
            _izdavaci.Remove(izdavac);
            Sacuvaj();
        }
    }

    public void Sacuvaj()
    {
        Serijalizacija.Sacuvaj(_izdavaci, _putanja);
    }

    private int SledeciId()
    {
        int max = 0;
        foreach (Izdavac izdavac in _izdavaci)
        {
            if (izdavac.Id > max)
            {
                max = izdavac.Id;
            }
        }
        return max + 1;
    }
}
