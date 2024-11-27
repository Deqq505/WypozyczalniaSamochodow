using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class CarDatabase
{
    private const string FilePath = "..\\..\\..\\cars.json";

    public List<Car> LoadCars()
    {
        if (!File.Exists(FilePath))
        {
            return new List<Car>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Car>>(json) ?? new List<Car>();
    }

    public void SaveCars(List<Car> cars)
    {
        try
        {
            var json = JsonSerializer.Serialize(cars, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd przy zapisie do pliku: {ex.Message}");
        }
    }
}
