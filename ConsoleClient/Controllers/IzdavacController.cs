using ConsoleClient.Utils;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers;

public class IzdavacController
{
    private readonly IzdavacService _izdavacService;

    public IzdavacController(IzdavacService izdavacService)
    {
        _izdavacService = izdavacService;
    }

    public void Pokreni()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("----- IZDAVAČI -----");
            Console.WriteLine("1) Prikaz svih izdavača");
            Console.WriteLine("2) Dodavanje izdavača");
            Console.WriteLine("3) Izmena izdavača");
            Console.WriteLine("4) Brisanje izdavača");
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
        List<Izdavac> izdavaci = _izdavacService.GetAll();
        if (izdavaci.Count == 0)
        {
            Console.WriteLine("Nema izdavača u sistemu.");
            return;
        }

        foreach (Izdavac izdavac in izdavaci)
        {
            Prikazi(izdavac);
        }
        Ispis.Separator();
        Console.WriteLine("Ukupno izdavača: " + izdavaci.Count);
    }

    private void Prikazi(Izdavac izdavac)
    {
        Ispis.Separator();
        Console.WriteLine("ID:              " + izdavac.Id);
        Console.WriteLine("Šifra:           " + izdavac.Sifra);
        Console.WriteLine("Naziv:           " + izdavac.Naziv);
        Console.WriteLine("Šef (ID autora): " + TekstSefa(izdavac.SefId));
        Console.WriteLine("Autori (ID):     " + Ispis.TekstListe(izdavac.AutorIds));
        Console.WriteLine("Knjige (ID):     " + Ispis.TekstListe(izdavac.KnjigaIds));
    }

    // 8.1 Dodavanje
    private void Dodaj()
    {
        Console.WriteLine();
        Console.WriteLine("--- Dodavanje izdavača ---");
        Izdavac izdavac = new Izdavac();

        string sifra = Unos.Tekst("Šifra");
        while (_izdavacService.PostojiSifra(sifra, 0))
        {
            Console.WriteLine("Izdavač sa tom šifrom već postoji.");
            sifra = Unos.Tekst("Šifra");
        }
        izdavac.Sifra = sifra;

        izdavac.Naziv = Unos.Tekst("Naziv");
        izdavac.SefId = Unos.CeoBroj("ID autora koji je šef (0 ako nema šefa)", 0, int.MaxValue);
        izdavac.AutorIds = Unos.ListaBrojeva("Autori - ID");
        izdavac.KnjigaIds = Unos.ListaBrojeva("Knjige - ID");

        _izdavacService.Add(izdavac);
        Console.WriteLine("Izdavač je uspešno dodat (ID = " + izdavac.Id + ").");
    }

    // 8.2 Izmena
    private void Izmeni()
    {
        Izdavac? izdavac = IzaberiIzdavaca("ID izdavača za izmenu");
        if (izdavac == null)
        {
            return;
        }

        Console.WriteLine("Pritisnite Enter da zadržite postojeću vrednost.");

        string sifra = Unos.TekstIzmena("Šifra", izdavac.Sifra);
        while (_izdavacService.PostojiSifra(sifra, izdavac.Id))
        {
            Console.WriteLine("Izdavač sa tom šifrom već postoji.");
            sifra = Unos.TekstIzmena("Šifra", izdavac.Sifra);
        }
        izdavac.Sifra = sifra;

        izdavac.Naziv = Unos.TekstIzmena("Naziv", izdavac.Naziv);
        izdavac.SefId = Unos.CeoBrojIzmena("ID autora koji je šef (0 ako nema šefa)", izdavac.SefId, 0, int.MaxValue);
        izdavac.AutorIds = Unos.ListaBrojevaIzmena("Autori - ID", izdavac.AutorIds);
        izdavac.KnjigaIds = Unos.ListaBrojevaIzmena("Knjige - ID", izdavac.KnjigaIds);

        _izdavacService.Update(izdavac);
        Console.WriteLine("Izdavač je uspešno izmenjen.");
    }

    // 8.3 Brisanje
    private void Obrisi()
    {
        Izdavac? izdavac = IzaberiIzdavaca("ID izdavača za brisanje");
        if (izdavac == null)
        {
            return;
        }

        Prikazi(izdavac);
        Ispis.Separator();

        if (Unos.Potvrda("Da li ste sigurni da želite da obrišete izdavača?"))
        {
            _izdavacService.Delete(izdavac.Id);
            Console.WriteLine("Izdavač je obrisan.");
        }
        else
        {
            Console.WriteLine("Brisanje je otkazano.");
        }
    }

    // Ispisuje kratak spisak izdavaca i trazi da se unese ID
    private Izdavac? IzaberiIzdavaca(string poruka)
    {
        List<Izdavac> izdavaci = _izdavacService.GetAll();
        if (izdavaci.Count == 0)
        {
            Console.WriteLine("Nema izdavača u sistemu.");
            return null;
        }

        Console.WriteLine();
        foreach (Izdavac i in izdavaci)
        {
            Console.WriteLine(i.Id + ") " + i.Sifra + " - " + i.Naziv);
        }

        int id = Unos.CeoBroj(poruka, 1, int.MaxValue);
        Izdavac? izdavac = _izdavacService.GetById(id);
        if (izdavac == null)
        {
            Console.WriteLine("Izdavač sa ID " + id + " ne postoji.");
        }
        return izdavac;
    }

    private string TekstSefa(int sefId)
    {
        if (sefId == 0)
        {
            return "nije postavljen";
        }
        return sefId.ToString();
    }
}
