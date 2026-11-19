using Core.Models;

namespace ConsoleClient.Utils;

// Pomocne metode za lepsi ispis podataka u konzoli
public static class Ispis
{
    public static void Separator()
    {
        Console.WriteLine("----------------------------------------");
    }

    public static string TekstDatuma(DateTime datum)
    {
        return datum.ToString("dd.MM.yyyy.");
    }

    public static string TekstAdrese(Adresa adresa)
    {
        return adresa.Ulica + " " + adresa.Broj + ", " + adresa.Grad + ", " + adresa.Drzava;
    }

    public static string TekstListe(List<int> lista)
    {
        if (lista.Count == 0)
        {
            return "nema";
        }

        string tekst = "";
        for (int i = 0; i < lista.Count; i++)
        {
            if (i > 0)
            {
                tekst = tekst + ", ";
            }
            tekst = tekst + lista[i];
        }
        return tekst;
    }
}
