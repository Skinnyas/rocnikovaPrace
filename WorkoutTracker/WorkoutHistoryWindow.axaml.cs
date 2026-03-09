using Avalonia.Controls;
using System.Collections.Generic;
using WorkoutTracker.Models;

namespace WorkoutTracker;

public partial class WorkoutHistoryWindow : Window
{
    public WorkoutHistoryWindow()
    {
        InitializeComponent();
    }

    public WorkoutHistoryWindow(List<Workout> workouts) : this()
    {
        AllWorkoutsItemsControl.ItemsSource = workouts;
        CountBadge.Text = workouts.Count.ToString();
    }
}
