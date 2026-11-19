using ConsoleClient.Utils;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers;

public class KupovinaController
{
    private readonly KupovinaService _kupovinaService;
    private readonly PosetilacService _posetilacService;

    public KupovinaController(KupovinaService kupovinaService, PosetilacService posetilacService)
    {
        _kupovinaService = kupovinaService;
        _posetilacService = posetilacService;
    }

    public void Pokreni()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("----- KUPOVINE -----");
            Console.WriteLine("1) Prikaz svih kupovina");
            Console.WriteLine("2) Dodavanje kupovine");
            Console.WriteLine("3) Izmena kupovine");
            Console.WriteLine("4) Brisanje kupovine");
            Console.WriteLine("0) Nazad");
            string izbor = Unos.Linija("> ");

            switch (izbor)
            {
                case "1":
                    PrikaziSve();
                    break;
                case "2":
                    Dodaj();
                    break;
                case "3":
                    Izmeni();
                    break;
                case "4":
                    Obrisi();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Nepoznata opcija.");
                    break;
            }
        }
    }

    // 8.4 Prikaz
    private void PrikaziSve()
    {
        List<Kupovina> kupovine = _kupovinaService.GetAll();
        if (kupovine.Count == 0)
        {
            Console.WriteLine("Nema kupovina u sistemu.");
            return;
        }

        foreach (Kupovina kupovina in kupovine)
        {
            Prikazi(kupovina);
        }
        Ispis.Separator();
        Console.WriteLine("Ukupno kupovina: " + kupovine.Count);
    }

    private void Prikazi(Kupovina kupovina)
    {
        Ispis.Separator();
        Console.WriteLine("ID:               " + kupovina.Id);
        Console.WriteLine("Posetilac:        " + TekstPosetioca(kupovina.PosetilacId));
        Console.WriteLine("Knjiga (ID):      " + kupovina.KnjigaId);
        Console.WriteLine("Datum kupovine:   " + Ispis.TekstDatuma(kupovina.DatumKupovine));
        Console.WriteLine("Ocena:            " + kupovina.Ocena);
        Console.WriteLine("Komentar:         " + kupovina.Komentar);
    }

    // 8.1 Dodavanje
    private void Dodaj()
    {
        List<Posetilac> posetioci = _posetilacService.GetAll();
        if (posetioci.Count == 0)
        {
            Console.WriteLine("Prvo dodajte bar jednog posetioca.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("--- Dodavanje kupovine ---");
        foreach (Posetilac p in posetioci)
        {
            Console.WriteLine(p.Id + ") " + p.BrojClanskeKarte + " - " + p.Ime + " " + p.Prezime);
        }

        Kupovina kupovina = new Kupovina();

        int posetilacId = Unos.CeoBroj("ID posetioca", 1, int.MaxValue);
        while (_posetilacService.GetById(posetilacId) == null)
        {
            Console.WriteLine("Posetilac sa ID " + posetilacId + " ne postoji.");
            posetilacId = Unos.CeoBroj("ID posetioca", 1, int.MaxValue);
        }
        kupovina.PosetilacId = posetilacId;

        int knjigaId = Unos.CeoBroj("ID knjige", 1, int.MaxValue);
        while (_kupovinaService.PosetilacVecKupio(posetilacId, knjigaId))
        {
            Console.WriteLine("Ovaj posetilac je već kupio tu knjigu.");
            knjigaId = Unos.CeoBroj("ID knjige", 1, int.MaxValue);
        }
        kupovina.KnjigaId = knjigaId;

        kupovina.DatumKupovine = Unos.Datum("Datum kupovine");
        kupovina.Ocena = Unos.CeoBroj("Ocena (1-5)", 1, 5);
        kupovina.Komentar = Unos.Linija("Komentar (može ostati prazno): ");

        _kupovinaService.Add(kupovina);
        Console.WriteLine("Kupovina je uspešno dodata (ID = " + kupovina.Id + ").");
    }

    // 8.2 Izmena (posetilac i knjiga se ne menjaju, samo datum, ocena i komentar)
    private void Izmeni()
    {
        Kupovina? kupovina = IzaberiKupovinu("ID kupovine za izmenu");
        if (kupovina == null)
        {
            return;
        }

        Console.WriteLine("Pritisnite Enter da zadržite postojeću vrednost.");
        kupovina.DatumKupovine = Unos.DatumIzmena("Datum kupovine", kupovina.DatumKupovine);
        kupovina.Ocena = Unos.CeoBrojIzmena("Ocena (1-5)", kupovina.Ocena, 1, 5);
        kupovina.Komentar = Unos.TekstIzmena("Komentar", kupovina.Komentar);

        _kupovinaService.Update(kupovina);
        Console.WriteLine("Kupovina je uspešno izmenjena.");
    }

    // 8.3 Brisanje
    private void Obrisi()
    {
        Kupovina? kupovina = IzaberiKupovinu("ID kupovine za brisanje");
        if (kupovina == null)
        {
            return;
        }

        Prikazi(kupovina);
        Ispis.Separator();

        if (Unos.Potvrda("Da li ste sigurni da želite da obrišete kupovinu?"))
        {
            _kupovinaService.Delete(kupovina.Id);
            Console.WriteLine("Kupovina je obrisana.");
        }
        else
        {
            Console.WriteLine("Brisanje je otkazano.");
        }
    }

    // Ispisuje kratak spisak kupovina i trazi da se unese ID
    private Kupovina? IzaberiKupovinu(string poruka)
    {
        List<Kupovina> kupovine = _kupovinaService.GetAll();
        if (kupovine.Count == 0)
        {
            Console.WriteLine("Nema kupovina u sistemu.");
            return null;
        }

        Console.WriteLine();
        foreach (Kupovina k in kupovine)
        {
            Console.WriteLine(k.Id + ") " + TekstPosetioca(k.PosetilacId) + " - knjiga " + k.KnjigaId
                              + " - " + Ispis.TekstDatuma(k.DatumKupovine));
        }

        int id = Unos.CeoBroj(poruka, 1, int.MaxValue);
        Kupovina? kupovina = _kupovinaService.GetById(id);
        if (kupovina == null)
        {
            Console.WriteLine("Kupovina sa ID " + id + " ne postoji.");
        }
        return kupovina;
    }

    private string TekstPosetioca(int posetilacId)
    {
        Posetilac? posetilac = _posetilacService.GetById(posetilacId);
        if (posetilac == null)
        {
            return "nepoznat (ID " + posetilacId + ")";
        }
        return posetilac.Ime + " " + posetilac.Prezime + " (" + posetilac.BrojClanskeKarte + ")";
    }
}
