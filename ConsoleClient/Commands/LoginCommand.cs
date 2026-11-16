using Core.Services;

namespace ConsoleClient.Commands;

public class LoginCommand
{
    private readonly UserService _userService;

    public LoginCommand(UserService userService)
    {
        _userService = userService;
    }

    public void Execute()
    {
        Console.Write("Korisničko ime: ");
        string user = Console.ReadLine() ?? string.Empty;

        Console.Write("Lozinka: ");
        string pass = Console.ReadLine() ?? string.Empty;

        Console.WriteLine(_userService.Login(user, pass) ? "Uspešna prijava." : "Neuspešna prijava.");
    }
}
