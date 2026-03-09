using Avalonia.Controls;
using Avalonia.Interactivity;
using WorkoutTracker.Services;

namespace WorkoutTracker;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void ToggleView(object? sender, RoutedEventArgs e)
    {
        LoginPanel.IsVisible = !LoginPanel.IsVisible;
        RegisterPanel.IsVisible = !RegisterPanel.IsVisible;
    }

    private void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        string username = LoginUsername.Text ?? "";
        string password = LoginPassword.Text ?? "";

        if (AuthService.Login(username, password))
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        else
        {
            LoginError.Text = "Neplatné jméno nebo heslo.";
            LoginError.IsVisible = true;
        }
    }

    private void OnRegisterClick(object? sender, RoutedEventArgs e)
    {
        string fullName = RegFullName.Text ?? "";
        string username = RegUsername.Text ?? "";
        string password = RegPassword.Text ?? "";

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            RegError.Text = "Vyplňte všechna pole.";
            RegError.IsVisible = true;
            return;
        }

        if (AuthService.Register(username, password, fullName))
        {
            ToggleView(null, new RoutedEventArgs());
        }
        else
        {
            RegError.Text = "Uživatelské jméno již existuje.";
            RegError.IsVisible = true;
        }
    }
}
