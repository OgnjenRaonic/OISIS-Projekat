using ConsoleClient.Commands;
using Core.Services;

namespace ConsoleClient.Controllers;

public class ConsoleController
{
    private readonly UserService _userService;

    public ConsoleController(UserService userService)
    {
        _userService = userService;
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1) Prijava");
            Console.WriteLine("0) Izlaz");
            Console.Write("> ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    new LoginCommand(_userService).Execute();
                    break;
                case "0":
                case null:
                    return;
                default:
                    Console.WriteLine("Nepoznata opcija.");
                    break;
            }
        }
    }
}
