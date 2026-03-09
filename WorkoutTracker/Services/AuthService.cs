using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public static class AuthService
{
    private static readonly string UsersFilePath = "users.json";
    public static User? CurrentUser { get; private set; }

    public static bool Register(string username, string password, string fullName)
    {
        var users = LoadUsers();
        if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            return false;

        var newUser = new User 
        { 
            Username = username, 
            Password = password, 
            FullName = fullName 
        };
        
        users.Add(newUser);
        SaveUsers(users);
        return true;
    }

    public static bool Login(string username, string password)
    {
        var users = LoadUsers();
        var user = users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
            u.Password == password);

        if (user != null)
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }

    private static List<User> LoadUsers()
    {
        if (!File.Exists(UsersFilePath))
            return new List<User>();

        try
        {
            var json = File.ReadAllText(UsersFilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch
        {
            return new List<User>();
        }
    }

    private static void SaveUsers(List<User> users)
    {
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(UsersFilePath, json);
    }
}
