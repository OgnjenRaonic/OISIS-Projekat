using Core.Interfaces;
using Core.Utils;

namespace Core.Services;

public class UserService
{
    private readonly IUserDao _userDao;

    public UserService(IUserDao userDao)
    {
        _userDao = userDao;
    }

    public bool Login(string user, string pass)
    {
        // TODO: prava provera kada User dobije lozinku, a IUserDao pretragu po imenu
        bool success = !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass);
        Logger.Log($"Login '{user}': {(success ? "uspešan" : "neuspešan")}");
        return success;
    }
}
