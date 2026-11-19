using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.DAO;

public class PosetilacDao : IPosetilacDao
{
    private readonly string _putanja;
    private List<Posetilac> _posetioci;

    public PosetilacDao()
    {
        _putanja = Serijalizacija.Putanja("posetioci.json");
        _posetioci = Serijalizacija.Ucitaj<Posetilac>(_putanja);
    }

    public Posetilac? GetById(int id)
    {
        foreach (Posetilac posetilac in _posetioci)
        {
            if (posetilac.Id == id)
            {
                return posetilac;
            }
        }
        return null;
    }

    public List<Posetilac> GetAll()
    {
        return new List<Posetilac>(_posetioci);
    }

    public void Add(Posetilac posetilac)
    {
        posetilac.Id = SledeciId();
        _posetioci.Add(posetilac);
        Sacuvaj();
    }

    public void Update(Posetilac posetilac)
    {
        for (int i = 0; i < _posetioci.Count; i++)
        {
            if (_posetioci[i].Id == posetilac.Id)
            {
                _posetioci[i] = posetilac;
                Sacuvaj();
                return;
            }
        }
    }

    public void Delete(int id)
    {
        Posetilac? posetilac = GetById(id);
        if (posetilac != null)
        {
            _posetioci.Remove(posetilac);
            Sacuvaj();
        }
    }

    public void Sacuvaj()
    {
        Serijalizacija.Sacuvaj(_posetioci, _putanja);
    }

    private int SledeciId()
    {
        int max = 0;
        foreach (Posetilac posetilac in _posetioci)
        {
            if (posetilac.Id > max)
            {
                max = posetilac.Id;
            }
        }
        return max + 1;
    }
}
