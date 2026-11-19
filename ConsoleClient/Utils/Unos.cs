using System.Globalization;

namespace ConsoleClient.Utils;

// Pomocne metode za ucitavanje podataka sa konzole.
// Metode "...Izmena" prikazuju staru vrednost u [ ] i zadrzavaju je ako se pritisne samo Enter.
public static class Unos
{
    public static string Linija(string poruka)
    {
        Console.Write(poruka);
        string? linija = Console.ReadLine();
        if (linija == null)
        {
            // ulaz je zavrsen (npr. Ctrl+Z), zatvaramo program
            Environment.Exit(0);
        }
        return linija.Trim();
    }

    // ---------- tekst ----------

    public static string Tekst(string naziv)
    {
        while (true)
        {
            string unos = Linija(naziv + ": ");
            if (unos != "")
            {
                return unos;
            }
            Console.WriteLine("Polje je obavezno.");
        }
    }

    public static string TekstIzmena(string naziv, string stara)
    {
        string unos = Linija(naziv + " [" + stara + "]: ");
        if (unos == "")
        {
            return stara;
        }
        return unos;
    }

    // ---------- ceo broj ----------

    public static int CeoBroj(string naziv, int min, int max)
    {
        while (true)
        {
            string unos = Linija(naziv + ": ");
            int broj;
            if (int.TryParse(unos, out broj) && broj >= min && broj <= max)
            {
                return broj;
            }
            Console.WriteLine("Unesite ceo broj od " + min + " do " + max + ".");
        }
    }

    public static int CeoBrojIzmena(string naziv, int stara, int min, int max)
    {
        while (true)
        {
            string unos = Linija(naziv + " [" + stara + "]: ");
            if (unos == "")
            {
                return stara;
            }
            int broj;
            if (int.TryParse(unos, out broj) && broj >= min && broj <= max)
            {
                return broj;
            }
            Console.WriteLine("Unesite ceo broj od " + min + " do " + max + ".");
        }
    }

    // ---------- datum ----------

    public static DateTime Datum(string naziv)
    {
        while (true)
        {
            string unos = Linija(naziv + " (dd.mm.gggg): ");
            DateTime datum;
            if (ProveriDatum(unos, out datum))
            {
                return datum;
            }
        }
    }

    public static DateTime DatumIzmena(string naziv, DateTime stara)
    {
        while (true)
        {
            string unos = Linija(naziv + " [" + stara.ToString("dd.MM.yyyy") + "]: ");
            if (unos == "")
            {
                return stara;
            }
            DateTime datum;
            if (ProveriDatum(unos, out datum))
            {
                return datum;
            }
        }
    }

    // Pretvara tekst u datum i proverava da datum nije u buducnosti
    private static bool ProveriDatum(string unos, out DateTime datum)
    {
        // dozvoljavamo i tacku na kraju, npr. 12.12.2002.
        if (unos.EndsWith("."))
        {
            unos = unos.Substring(0, unos.Length - 1);
        }

        if (!DateTime.TryParseExact(unos, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum))
        {
            Console.WriteLine("Neispravan datum. Primer: 12.05.2001");
            return false;
        }

        if (datum > DateTime.Today)
        {
            Console.WriteLine("Datum ne može biti u budućnosti.");
            return false;
        }

        return true;
    }

    // ---------- e-mejl ----------

    public static string Email(string naziv)
    {
        while (true)
        {
            string unos = Linija(naziv + ": ");
            if (ProveriEmail(unos))
            {
                return unos;
            }
        }
    }

    public static string EmailIzmena(string naziv, string stara)
    {
        while (true)
        {
            string unos = Linija(naziv + " [" + stara + "]: ");
            if (unos == "")
            {
                return stara;
            }
            if (ProveriEmail(unos))
            {
                return unos;
            }
        }
    }

    // E-mejl mora da ima nesto pre @, a posle @ mora da postoji tacka (npr. marko@gmail.com)
    private static bool ProveriEmail(string email)
    {
        int pozicijaEt = email.IndexOf('@');
        int pozicijaTacke = email.LastIndexOf('.');

        if (pozicijaEt > 0 &&
            pozicijaTacke > pozicijaEt + 1 &&
            pozicijaTacke < email.Length - 1 &&
            !email.Contains(' '))
        {
            return true;
        }

        Console.WriteLine("Neispravan e-mejl. Primer: marko@gmail.com");
        return false;
    }

    // ---------- telefon ----------

    public static string Telefon(string naziv)
    {
        while (true)
        {
            string unos = Linija(naziv + ": ");
            if (ProveriTelefon(unos))
            {
                return unos;
            }
        }
    }

    public static string TelefonIzmena(string naziv, string stara)
    {
        while (true)
        {
            string unos = Linija(naziv + " [" + stara + "]: ");
            if (unos == "")
            {
                return stara;
            }
            if (ProveriTelefon(unos))
            {
                return unos;
            }
        }
    }

    // Telefon sme da sadrzi cifre i znakove + - / i razmak, a mora imati bar 6 cifara
    private static bool ProveriTelefon(string telefon)
    {
        int brojCifara = 0;

        foreach (char znak in telefon)
        {
            if (char.IsDigit(znak))
            {
                brojCifara++;
            }
            else if (znak != '+' && znak != '-' && znak != '/' && znak != ' ')
            {
                Console.WriteLine("Telefon sme da sadrži samo cifre i znakove + - /");
                return false;
            }
        }

        if (brojCifara < 6)
        {
            Console.WriteLine("Telefon mora imati bar 6 cifara.");
            return false;
        }

        return true;
    }

    // ---------- lista ID-jeva ----------

    public static List<int> ListaBrojeva(string naziv)
    {
        while (true)
        {
            string unos = Linija(naziv + " (ID-jevi odvojeni zarezom, Enter za praznu listu): ");
            List<int> lista = new List<int>();
            if (ProveriListu(unos, lista))
            {
                return lista;
            }
        }
    }

    public static List<int> ListaBrojevaIzmena(string naziv, List<int> stara)
    {
        while (true)
        {
            string unos = Linija(naziv + " [" + Ispis.TekstListe(stara) + "] (\"-\" za praznu listu): ");
            if (unos == "")
            {
                return stara;
            }

            List<int> lista = new List<int>();
            if (unos == "-")
            {
                return lista;
            }
            if (ProveriListu(unos, lista))
            {
                return lista;
            }
        }
    }

    // Pretvara tekst "1, 2, 5" u brojeve i dodaje ih u listu.
    // Vraca false ako unos nije ispravan.
    private static bool ProveriListu(string unos, List<int> lista)
    {
        if (unos == "")
        {
            return true;
        }

        string[] delovi = unos.Split(',');
        foreach (string deo in delovi)
        {
            int broj;
            if (!int.TryParse(deo.Trim(), out broj) || broj < 1)
            {
                Console.WriteLine("Neispravan unos. Primer: 1, 2, 5");
                lista.Clear();
                return false;
            }

            if (!lista.Contains(broj))
            {
                lista.Add(broj);
            }
        }

        return true;
    }

    // ---------- potvrda ----------

    public static bool Potvrda(string pitanje)
    {
        string unos = Linija(pitanje + " (da/ne): ").ToLower();
        return unos == "da" || unos == "d";
    }
}
