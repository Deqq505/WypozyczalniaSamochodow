using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class UserDatabase
{
    private const string FilePath = "..\\..\\..\\Entities\\users.json";

    public List<User> LoadUsers()
    {
        if (!File.Exists(FilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    public void SaveUsers(List<User> users)
    {
        try
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd przy zapisie do pliku: {ex.Message}");
        }
    }
}
