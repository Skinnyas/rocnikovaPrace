using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

namespace WorkoutTracker;

public partial class MainWindow : Window
{
    public ObservableCollection<Workout> UserWorkouts { get; set; } = new ObservableCollection<Workout>();

    public MainWindow()
    {
        InitializeComponent();
        LoadUserData();
        LoadWorkouts();
        LoadStats();
        RecentWorkoutsList.ItemsSource = UserWorkouts;
    }

    private void LoadUserData()
    {
        if (AuthService.CurrentUser != null)
        {
            var fullName = AuthService.CurrentUser.FullName;
            WelcomeTextBlock.Text = $"Vítej zpět, {fullName.Split(' ')[0]}! 👋";
            SidebarUserName.Text = fullName;
            
            // Nastavení iniciál
            var names = fullName.Split(' ');
            string initials = names[0][0].ToString();
            if (names.Length > 1) initials += names[^1][0];
            UserInitials.Text = initials.ToUpper();
        }
    }

    private void LoadStats()
    {
        if (AuthService.CurrentUser == null) return;

        var stats = WorkoutService.GetUserStats(AuthService.CurrentUser.Username);
        
        StatCountThisWeek.Text = stats.countThisWeek.ToString();
        StatTotalMinutes.Text = $"{stats.totalMinutes} min";
        StatCalories.Text = stats.estimatedCalories.ToString("N0");
        
        // Změna trendu na základě počtu tréninků
        if (stats.countThisWeek >= 3)
        {
            StatTrend.Text = "Skvělá aktivita! 🔥";
            StatTrend.Foreground = Avalonia.Media.Brush.Parse("#10B981");
        }
        else
        {
            StatTrend.Text = "Zkus přidat trénink";
            StatTrend.Foreground = Avalonia.Media.Brush.Parse("#F59E0B");
        }
    }

    private void LoadWorkouts()
    {
        if (AuthService.CurrentUser == null) return;

        UserWorkouts.Clear();
        var workouts = WorkoutService.GetUserWorkouts(AuthService.CurrentUser.Username);
        
        // Only show last 3 workouts in the main dashboard
        foreach (var workout in workouts.Take(3))
        {
            UserWorkouts.Add(workout);
        }
    }

    private async void OnAddNewWorkoutClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddWorkoutWindow();
        await dialog.ShowDialog(this);
        
        if (dialog.IsSaved)
        {
            LoadWorkouts();
            LoadStats();
        }
    }

    private void OnShowAllWorkoutsClick(object? sender, RoutedEventArgs e)
    {
        OpenHistoryDialog();
    }

    private void OnHistoryNavTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        OpenHistoryDialog();
        // Reset selection to Overview if needed, or just let it be
    }

    private void OpenHistoryDialog()
    {
        if (AuthService.CurrentUser == null) return;

        var allWorkouts = WorkoutService.GetUserWorkouts(AuthService.CurrentUser.Username);
        var historyWindow = new WorkoutHistoryWindow(allWorkouts);
        historyWindow.ShowDialog(this);
    }
}
