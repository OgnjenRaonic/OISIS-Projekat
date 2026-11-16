using Core.DAO;
using Core.Interfaces;
using Core.Services;
using WpfClient.Controllers;
using WpfClient.Views;

namespace WpfClient;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        // Ovde se povezuju zavisnosti: DAO -> Service -> Controller -> View
        IUserDao userDao = new UserDao();
        var userService = new UserService(userDao);
        var controller = new MainController(userService);

        var app = new App();
        // Kada u App.xaml dodate resurse (stilove i sl.), ovde pozovite app.InitializeComponent();
        app.Run(new MainWindow(controller));
    }
}
