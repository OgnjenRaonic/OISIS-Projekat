namespace Core.Models;

public class Autor
{
    public int Id { get; set; }

    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public DateTime DatumRodjenja { get; set; }

    public Adresa? Adresa { get; set; }

    public string Telefon { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string BrojLicneKarte { get; set; } = string.Empty;

    public int GodineIskustva { get; set; }

    public List<int> KnjigaIds { get; set; } = new();
}
