using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        CarDatabase carDatabase = new CarDatabase();
        List<Car> cars = carDatabase.LoadCars();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Wyświetl dostępne samochody");
            Console.WriteLine("2. Wyświetl wszystkie samochody");
            Console.WriteLine("3. Zarezerwuj samochód");
            Console.WriteLine("4. Sprawdź nazwisko wynajmującego");
            Console.WriteLine("5. Wyjście");
            Console.Write("Wybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1":
                    DisplayAvailableCars(cars);
                    break;
                case "2":
                    DisplayAllCars(cars);
                    break;
                case "3":
                    ReserveCar(cars, carDatabase);
                    break;
                case "4":
                    CheckRenterSurname(cars);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Niepoprawna opcja.");
                    break;
            }
        }
    }



    static void DisplayAvailableCars(List<Car> cars)
    {
        Console.Clear();
        Console.WriteLine("Dostępne samochody:");
        foreach (var car in cars)
        {
            if (car.IsAvailable)
            {
                Console.WriteLine(car);
            }
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }




    static void DisplayAllCars(List<Car> cars)
    {
        Console.Clear();
        Console.WriteLine("Wszystkie samochody:");
        foreach (var car in cars)
        {
            Console.WriteLine(car);
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    static void ReserveCar(List<Car> cars, CarDatabase carDatabase)
    {
        Console.Clear();
        Console.WriteLine("Dostępne samochody do rezerwacji:");
        foreach (var car in cars)
        {
            if (car.IsAvailable)
            {
                Console.WriteLine(car);
            }
        }

        Console.Write("Podaj ID samochodu do rezerwacji: ");
        if (int.TryParse(Console.ReadLine(), out int carId))
        {
            var carToReserve = cars.Find(c => c.Id == carId && c.IsAvailable);
            if (carToReserve != null)
            {
                Console.Write("Podaj nazwisko wynajmującego: ");
                carToReserve.RenterSurname = Console.ReadLine(); // Ustaw nazwisko wynajmującego
                carToReserve.IsAvailable = false; // Zmień dostępność na false



                // Zapisz zmiany w pliku JSON
                carDatabase.SaveCars(cars); // Zapisz zaktualizowaną listę samochodów

                Console.WriteLine($"Rezerwacja samochodu {carToReserve.Model} została pomyślnie dokonana przez {carToReserve.RenterSurname}.");
            }
            else
            {
                Console.WriteLine("Samochód nie jest dostępny lub nie istnieje.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID.");
        }

        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }


    static void CheckRenterSurname(List<Car> cars)
    {
        Console.Clear();
        Console.Write("Podaj nazwisko wynajmującego: ");
        string surnameToCheck = Console.ReadLine();

        bool found = false;
        foreach (var car in cars)
        {
            if (car.RenterSurname.Equals(surnameToCheck, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Samochód {car.Model} ({car.Brand}) jest wynajęty przez {car.RenterSurname}.");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Nie znaleziono samochodu wynajętego przez to nazwisko.");
        }

        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }
}
