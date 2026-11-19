using System.Text.Encodings.Web;
using System.Text.Json;

namespace Core.Utils;

// Cuvanje i ucitavanje listi entiteta u JSON datoteke.
// Koristi se System.Text.Json iz .NET-a:
// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to
public static class Serijalizacija
{
    // Vraca punu putanju do datoteke u folderu "Podaci" koji se nalazi u korenu resenja.
    // Program se pokrece iz <Projekat>/bin/Debug/<verzija>/ pa se penjemo 4 nivoa gore.
    public static string Putanja(string imeDatoteke)
    {
        char sep = Path.DirectorySeparatorChar;

        string folder = AppDomain.CurrentDomain.BaseDirectory
                        + ".." + sep + ".." + sep + ".." + sep + ".." + sep + "Podaci";
        folder = Path.GetFullPath(folder);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        return folder + sep + imeDatoteke;
    }

    // Upisuje listu u datoteku (T je tip entiteta, npr. Posetilac)
    public static void Sacuvaj<T>(List<T> lista, string putanja)
    {
        JsonSerializerOptions opcije = new JsonSerializerOptions();
        opcije.WriteIndented = true;
        // da se slova č, ć, š, ž, đ upisuju normalno, a ne kao š
        opcije.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

        string json = JsonSerializer.Serialize(lista, opcije);
        File.WriteAllText(putanja, json);
    }

    // Cita listu iz datoteke; ako datoteka ne postoji vraca praznu listu
    public static List<T> Ucitaj<T>(string putanja)
    {
        if (!File.Exists(putanja))
        {
            return new List<T>();
        }

        string json = File.ReadAllText(putanja);
        if (json.Trim() == "")
        {
            return new List<T>();
        }

        List<T>? lista = JsonSerializer.Deserialize<List<T>>(json);
        if (lista == null)
        {
            return new List<T>();
        }

        return lista;
    }
}
