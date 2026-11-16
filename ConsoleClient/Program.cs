using System.Text;
using ConsoleClient.Controllers;
using Core.DAO;
using Core.Services;

Console.OutputEncoding = Encoding.UTF8;

// Ovde se povezuju zavisnosti: DAO -> Service -> Controller
var userService = new UserService(new UserDao());
new ConsoleController(userService).Start();
