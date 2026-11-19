using ConsoleClient.Utils;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers;

public class PosetilacController
{
    private readonly PosetilacService _posetilacService;

    public PosetilacController(PosetilacService posetilacService)
    {
        _posetilacService = posetilacService;
    }

    public void Pokreni()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("----- POSETIOCI -----");
            Console.WriteLine("1) Prikaz svih posetilaca");
            Console.WriteLine("2) Dodavanje posetioca");
            Console.WriteLine("3) Izmena posetioca");
            Console.WriteLine("4) Brisanje posetioca");
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
        List<Posetilac> posetioci = _posetilacService.GetAll();
        if (posetioci.Count == 0)
        {
            Console.WriteLine("Nema posetilaca u sistemu.");
            return;
        }

        foreach (Posetilac posetilac in posetioci)
        {
            Prikazi(posetilac);
        }
        Ispis.Separator();
        Console.WriteLine("Ukupno posetilaca: " + posetioci.Count);
    }

    private void Prikazi(Posetilac posetilac)
    {
        Ispis.Separator();
        Console.WriteLine("ID:                   " + posetilac.Id);
        Console.WriteLine("Broj članske karte:   " + posetilac.BrojClanskeKarte);
        Console.WriteLine("Ime i prezime:        " + posetilac.Ime + " " + posetilac.Prezime);
        Console.WriteLine("Datum rođenja:        " + Ispis.TekstDatuma(posetilac.DatumRodjenja));
        Console.WriteLine("Adresa:               " + Ispis.TekstAdrese(posetilac.Adresa));
        Console.WriteLine("Kontakt telefon:      " + posetilac.KontaktTelefon);
        Console.WriteLine("E-mejl:               " + posetilac.Email);
        Console.WriteLine("Godina članstva:      " + posetilac.TrenutnaGodinaClanstva);
        Console.WriteLine("Status:               " + TekstStatusa(posetilac.Status));
        Console.WriteLine("Prosečna ocena:       " + posetilac.ProsecnaOcena.ToString("0.00"));
        Console.WriteLine("Kupovine (ID):        " + Ispis.TekstListe(posetilac.KupovinaIds));
        Console.WriteLine("Lista želja (ID knj): " + Ispis.TekstListe(posetilac.ListaZeljaIds));
    }

    // 8.1 Dodavanje
    private void Dodaj()
    {
        Console.WriteLine();
        Console.WriteLine("--- Dodavanje posetioca ---");
        Posetilac posetilac = new Posetilac();

        string broj = Unos.Tekst("Broj članske karte (npr. SK-1-2023)");
        while (_posetilacService.PostojiBrojClanskeKarte(broj, 0))
        {
            Console.WriteLine("Posetilac sa tim brojem članske karte već postoji.");
            broj = Unos.Tekst("Broj članske karte (npr. SK-1-2023)");
        }
        posetilac.BrojClanskeKarte = broj;

        posetilac.Ime = Unos.Tekst("Ime");
        posetilac.Prezime = Unos.Tekst("Prezime");
        posetilac.DatumRodjenja = Unos.Datum("Datum rođenja");
        posetilac.KontaktTelefon = Unos.Telefon("Kontakt telefon");
        posetilac.Email = Unos.Email("E-mejl");

        Console.WriteLine("Adresa:");
        posetilac.Adresa.Ulica = Unos.Tekst("  Ulica");
        posetilac.Adresa.Broj = Unos.Tekst("  Broj");
        posetilac.Adresa.Grad = Unos.Tekst("  Grad");
        posetilac.Adresa.Drzava = Unos.Tekst("  Država");

        posetilac.TrenutnaGodinaClanstva = Unos.CeoBroj("Trenutna godina članstva", 1900, DateTime.Today.Year);
        posetilac.Status = UnesiStatus();
        posetilac.ListaZeljaIds = Unos.ListaBrojeva("Lista želja - ID knjiga");

        _posetilacService.Add(posetilac);
        Console.WriteLine("Posetilac je uspešno dodat (ID = " + posetilac.Id + ").");
    }

    // 8.2 Izmena
    private void Izmeni()
    {
        Posetilac? posetilac = IzaberiPosetioca("ID posetioca za izmenu");
        if (posetilac == null)
        {
            return;
        }

        Console.WriteLine("Pritisnite Enter da zadržite postojeću vrednost.");

        string broj = Unos.TekstIzmena("Broj članske karte", posetilac.BrojClanskeKarte);
        while (_posetilacService.PostojiBrojClanskeKarte(broj, posetilac.Id))
        {
            Console.WriteLine("Posetilac sa tim brojem članske karte već postoji.");
            broj = Unos.TekstIzmena("Broj članske karte", posetilac.BrojClanskeKarte);
        }
        posetilac.BrojClanskeKarte = broj;

        posetilac.Ime = Unos.TekstIzmena("Ime", posetilac.Ime);
        posetilac.Prezime = Unos.TekstIzmena("Prezime", posetilac.Prezime);
        posetilac.DatumRodjenja = Unos.DatumIzmena("Datum rođenja", posetilac.DatumRodjenja);
        posetilac.KontaktTelefon = Unos.TelefonIzmena("Kontakt telefon", posetilac.KontaktTelefon);
        posetilac.Email = Unos.EmailIzmena("E-mejl", posetilac.Email);

        Console.WriteLine("Adresa:");
        posetilac.Adresa.Ulica = Unos.TekstIzmena("  Ulica", posetilac.Adresa.Ulica);
        posetilac.Adresa.Broj = Unos.TekstIzmena("  Broj", posetilac.Adresa.Broj);
        posetilac.Adresa.Grad = Unos.TekstIzmena("  Grad", posetilac.Adresa.Grad);
        posetilac.Adresa.Drzava = Unos.TekstIzmena("  Država", posetilac.Adresa.Drzava);

        posetilac.TrenutnaGodinaClanstva = Unos.CeoBrojIzmena("Trenutna godina članstva", posetilac.TrenutnaGodinaClanstva, 1900, DateTime.Today.Year);
        posetilac.Status = UnesiStatusIzmena(posetilac.Status);
        posetilac.ListaZeljaIds = Unos.ListaBrojevaIzmena("Lista želja - ID knjiga", posetilac.ListaZeljaIds);

        _posetilacService.Update(posetilac);
        Console.WriteLine("Posetilac je uspešno izmenjen.");
    }

    // 8.3 Brisanje
    private void Obrisi()
    {
        Posetilac? posetilac = IzaberiPosetioca("ID posetioca za brisanje");
        if (posetilac == null)
        {
            return;
        }

        Prikazi(posetilac);
        Ispis.Separator();
        if (posetilac.KupovinaIds.Count > 0)
        {
            Console.WriteLine("Napomena: biće obrisane i sve kupovine ovog posetioca (" + posetilac.KupovinaIds.Count + ").");
        }

        if (Unos.Potvrda("Da li ste sigurni da želite da obrišete posetioca?"))
        {
            _posetilacService.Delete(posetilac.Id);
            Console.WriteLine("Posetilac je obrisan.");
        }
        else
        {
            Console.WriteLine("Brisanje je otkazano.");
        }
    }

    // Ispisuje kratak spisak posetilaca i trazi da se unese ID
    private Posetilac? IzaberiPosetioca(string poruka)
    {
        List<Posetilac> posetioci = _posetilacService.GetAll();
        if (posetioci.Count == 0)
        {
            Console.WriteLine("Nema posetilaca u sistemu.");
            return null;
        }

        Console.WriteLine();
        foreach (Posetilac p in posetioci)
        {
            Console.WriteLine(p.Id + ") " + p.BrojClanskeKarte + " - " + p.Ime + " " + p.Prezime);
        }

        int id = Unos.CeoBroj(poruka, 1, int.MaxValue);
        Posetilac? posetilac = _posetilacService.GetById(id);
        if (posetilac == null)
        {
            Console.WriteLine("Posetilac sa ID " + id + " ne postoji.");
        }
        return posetilac;
    }

    private StatusPosetioca UnesiStatus()
    {
        while (true)
        {
            string unos = Unos.Linija("Status (R - redovan, V - VIP): ").ToUpper();
            if (unos == "R")
            {
                return StatusPosetioca.R;
            }
            if (unos == "V")
            {
                return StatusPosetioca.V;
            }
            Console.WriteLine("Unesite R ili V.");
        }
    }

    private StatusPosetioca UnesiStatusIzmena(StatusPosetioca stari)
    {
        while (true)
        {
            string unos = Unos.Linija("Status (R - redovan, V - VIP) [" + stari + "]: ").ToUpper();
            if (unos == "")
            {
                return stari;
            }
            if (unos == "R")
            {
                return StatusPosetioca.R;
            }
            if (unos == "V")
            {
                return StatusPosetioca.V;
            }
            Console.WriteLine("Unesite R ili V.");
        }
    }

    private string TekstStatusa(StatusPosetioca status)
    {
        if (status == StatusPosetioca.V)
        {
            return "VIP";
        }
        return "Redovan";
    }
}
