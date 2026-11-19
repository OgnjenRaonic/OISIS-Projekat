using ConsoleClient.Commands;
using ConsoleClient.Utils;
using Core.Services;

namespace ConsoleClient.Controllers;

public class ConsoleController
{
    private readonly UserService _userService;
    private readonly PosetilacController _posetilacController;
    private readonly KupovinaController _kupovinaController;
    private readonly IzdavacController _izdavacController;

    public ConsoleController(UserService userService,
                             PosetilacService posetilacService,
                             KupovinaService kupovinaService,
                             IzdavacService izdavacService)
    {
        _userService = userService;
        _posetilacController = new PosetilacController(posetilacService);
        _kupovinaController = new KupovinaController(kupovinaService, posetilacService);
        _izdavacController = new IzdavacController(izdavacService);
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== SAJAM KNJIGA =====");
            Console.WriteLine("1) Posetioci");
            Console.WriteLine("2) Kupovine");
            Console.WriteLine("3) Izdavači");
            Console.WriteLine("4) Prijava");
            Console.WriteLine("0) Izlaz");
            string izbor = Unos.Linija("> ");

            switch (izbor)
            {
                case "1":
                    _posetilacController.Pokreni();
                    break;
                case "2":
                    _kupovinaController.Pokreni();
                    break;
                case "3":
                    _izdavacController.Pokreni();
                    break;
                case "4":
                    new LoginCommand(_userService).Execute();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Nepoznata opcija.");
                    break;
            }
        }
    }
}
