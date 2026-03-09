using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public static class WorkoutService
{
    private static readonly string WorkoutsFilePath = "workouts.json";

    public static void SaveWorkout(Workout workout)
    {
        var workouts = LoadAllWorkouts();
        workouts.Add(workout);
        
        var json = JsonSerializer.Serialize(workouts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(WorkoutsFilePath, json);
    }

    public static List<Workout> GetUserWorkouts(string username)
    {
        var all = LoadAllWorkouts();
        return all
            .Where(w => w.UserUsername == username)
            .OrderByDescending(w => w.Date)
            .ToList();
    }

    public static (int countThisWeek, int totalMinutes, int estimatedCalories) GetUserStats(string username)
    {
        var userWorkouts = GetUserWorkouts(username);
        var now = DateTime.Now;
        // Výpočet začátku aktuálního týdne (pondělí)
        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
        var startOfWeek = now.AddDays(-1 * diff).Date;

        var countThisWeek = userWorkouts.Count(w => w.Date >= startOfWeek);
        var totalMinutes = userWorkouts.Sum(w => w.DurationMinutes);
        
        // Jednoduchý odhad kalorií: 7 kalorií za minutu (průměrná hodnota)
        var estimatedCalories = totalMinutes * 7;

        return (countThisWeek, totalMinutes, estimatedCalories);
    }

    private static List<Workout> LoadAllWorkouts()
    {
        if (!File.Exists(WorkoutsFilePath))
            return new List<Workout>();

        try
        {
            var json = File.ReadAllText(WorkoutsFilePath);
            return JsonSerializer.Deserialize<List<Workout>>(json) ?? new List<Workout>();
        }
        catch
        {
            return new List<Workout>();
        }
    }
}
