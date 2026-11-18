namespace Core.Models;

public class Adresa
{
    public int Id { get; set; }

    public string Ulica { get; set; } = string.Empty;

    public string Broj { get; set; } = string.Empty;

    public string Grad { get; set; } = string.Empty;

    public string Drzava { get; set; } = string.Empty;
}
