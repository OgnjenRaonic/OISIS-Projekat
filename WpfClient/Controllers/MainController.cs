using Core.Services;

namespace WpfClient.Controllers;

public class MainController
{
    private readonly UserService _userService;

    public MainController(UserService userService)
    {
        _userService = userService;
    }

    public bool Login(string username, string password)
    {
        return _userService.Login(username, password);
    }
}
