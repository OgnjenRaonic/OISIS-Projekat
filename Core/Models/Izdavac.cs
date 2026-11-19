namespace Core.Models;

public class Izdavac
{
    public int Id { get; set; }

    public string Sifra { get; set; } = "";

    public string Naziv { get; set; } = "";

    // ID autora koji je sef izdavaca (autor sa najmanje 5 godina iskustva)
    // 0 znaci da sef nije postavljen
    public int SefId { get; set; }

    public List<int> AutorIds { get; set; } = new List<int>();

    public List<int> KnjigaIds { get; set; } = new List<int>();
}
