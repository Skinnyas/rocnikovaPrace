using System;

namespace WorkoutTracker.Models;

public class Workout
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserUsername { get; set; } = string.Empty;
    public string ExerciseType { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int DurationMinutes { get; set; }
    public string Intensity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
