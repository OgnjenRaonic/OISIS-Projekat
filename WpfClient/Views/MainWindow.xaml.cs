using System.Windows;
using WpfClient.Controllers;

namespace WpfClient.Views;

public partial class MainWindow : Window
{
    private readonly MainController _controller;

    public MainWindow(MainController controller)
    {
        InitializeComponent();
        _controller = controller;
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        bool success = _controller.Login(UsernameInput.Text, PasswordInput.Password);
        StatusText.Text = success ? "Uspešna prijava." : "Neuspešna prijava.";
    }
}
