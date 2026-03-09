using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

namespace WorkoutTracker;

public partial class AddWorkoutWindow : Window
{
    public bool IsSaved { get; private set; }

    public AddWorkoutWindow()
    {
        InitializeComponent();
        DateInput.SelectedDate = DateTimeOffset.Now;
    }

    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (AuthService.CurrentUser == null) return;

        var type = (TypeInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Jiné";
        var date = DateInput.SelectedDate?.DateTime ?? DateTime.Now;
        
        if (!int.TryParse(DurationInput.Text, out int duration))
        {
            // Simple validation could be added here
        }

        var intensity = (IntensityInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Střední";

        var newWorkout = new Workout
        {
            UserUsername = AuthService.CurrentUser.Username,
            ExerciseType = type,
            Date = date,
            DurationMinutes = duration,
            Intensity = intensity
        };

        WorkoutService.SaveWorkout(newWorkout);
        IsSaved = true;
        this.Close();
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        IsSaved = false;
        this.Close();
    }
}
