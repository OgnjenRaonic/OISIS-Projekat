namespace Core.Models;

public class Posetilac
{
    public int Id { get; set; }

    public string Ime { get; set; } = "";

    public string Prezime { get; set; } = "";

    public DateTime DatumRodjenja { get; set; }

    public Adresa Adresa { get; set; } = new Adresa();

    public string KontaktTelefon { get; set; } = "";

    public string Email { get; set; } = "";

    // npr. SK-1-2023
    public string BrojClanskeKarte { get; set; } = "";

    public int TrenutnaGodinaClanstva { get; set; }

    public StatusPosetioca Status { get; set; }

    // racuna se automatski iz ocena u kupovinama posetioca
    public double ProsecnaOcena { get; set; }

    // ID-jevi kupovina koje je posetilac napravio
    public List<int> KupovinaIds { get; set; } = new List<int>();

    // ID-jevi knjiga koje posetilac ima na listi zelja
    public List<int> ListaZeljaIds { get; set; } = new List<int>();
}
