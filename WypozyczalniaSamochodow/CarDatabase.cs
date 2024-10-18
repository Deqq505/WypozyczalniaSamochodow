using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class CarDatabase
{
    private const string FilePath = "C:\\Users\\godzi\\OneDrive\\Studia informatyka- Rok 2, sem 3\\Programowanie III\\Wypozyczalnia samochodow\\Wypozyczalnia samochodow\\cars.json";

    public List<Car> LoadCars()
    {
        if (!File.Exists(FilePath))
        {
            // Zwracamy pustą listę, jeśli plik nie istnieje
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
            Console.WriteLine("Dane zostały zapisane do pliku JSON.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd przy zapisie do pliku: {ex.Message}");
        }


    }




}
