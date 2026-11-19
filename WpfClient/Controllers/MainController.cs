using Core.Services;
using Core.Models;

namespace WpfClient.Controllers;

public class MainController
{
    private readonly UserService _userService;
    public UserService UserService { get; }
    public AdresaService AdresaService { get; } 
    public AutorService AutorService { get; }
    public KnjigaService KnjigaService { get; }

    public MainController(UserService userService, AdresaService adresaService, AutorService autorService, KnjigaService knjigaService)
    {
        _userService = userService;
        AdresaService = adresaService;
        AutorService = autorService;
        KnjigaService = knjigaService;  
    }

    public List<Autor> GetAllAutore() => AutorService.GetAll();
    public List<Knjiga> GetAllKnjige() => KnjigaService.GetAll();
    public List<Adresa> GetAllAdrese() => AdresaService.GetAll();

    public bool Login(string username, string password)
    {
        return _userService.Login(username, password);
    }
}
