namespace Core.Models;

public class Knjiga
{
    public int Id { get; set; }

    public string ISBN { get; set; } = string.Empty;

    public string Naziv { get; set; } = string.Empty;

    public string Zanr { get; set; } = string.Empty;

    public int GodinaIzdanja { get; set; }

    public decimal Cena { get; set; }

    public int BrojStrana { get; set; }

    public List<int> AutorIds { get; set; } = new();

    public int? IzdavacId { get; set; }

    public List<int> PosetilacIdsKupili { get; set; } = new();

    public List<int> PosetilacIdsListaZelja { get; set; } = new();
}
