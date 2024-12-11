using System;
using System.Collections.Generic;

public static class CarService
{
    public static void DisplayAvailableCars(List<Car> cars)
    {
        Console.Clear();
        MenuService.DisplayHeader("Dostępne Samochody");

        var availableCars = cars.FindAll(c => c.IsAvailable);
        DisplayCarList(availableCars, "Brak dostępnych samochodów.");

        WaitForUserInput();
    }

    public static void DisplayAllCars(List<Car> cars)
    {
        Console.Clear();
        MenuService.DisplayHeader("Wszystkie Samochody");

        DisplayCarList(cars, "Brak samochodów w bazie.");

        WaitForUserInput();
    }

    public static void ReserveCar(List<Car> cars, CarDatabase carDatabase, User loggedInUser)
    {
        Console.Clear();
        MenuService.DisplayHeader("Rezerwacja Samochodu");

        var availableCars = cars.FindAll(c => c.IsAvailable);
        if (availableCars.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Brak dostępnych samochodów do rezerwacji.");
        }
        else
        {
            DisplayCarList(availableCars);
            HandleCarReservation(cars, carDatabase, loggedInUser);
        }

        WaitForUserInput();
    }

    public static void CheckRenterSurname(List<Car> cars)
    {
        Console.Clear();
        MenuService.DisplayHeader("Sprawdzenie Wynajmującego");

        var renters = ExtractRenterSurnames(cars);
        if (renters.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Brak wynajmujących samochodów.");
        }
        else
        {
            DisplayRenterList(renters);
            HandleRenterCheck(cars);
        }

        WaitForUserInput();
    }

    public static void PayForRental(List<Car> cars, CarDatabase carDatabase)
    {
        Console.Clear();
        MenuService.DisplayHeader("Opłata za Wynajem");

        var rentedCars = cars.FindAll(c => !c.IsAvailable);
        if (rentedCars.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Brak wynajętych samochodów do opłacenia.");
        }
        else
        {
            DisplayCarList(rentedCars);
            HandleRentalPayment(cars, carDatabase);
        }

        WaitForUserInput();
    }


    private static void DisplayCarList(List<Car> cars, string emptyMessage = "")
    {
        if (cars.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(emptyMessage);
        }
        else
        {
            foreach (var car in cars)
            {
                Console.WriteLine(car);
            }
        }
        Console.ResetColor();
    }

    private static void WaitForUserInput()
    {
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    private static void HandleCarReservation(List<Car> cars, CarDatabase carDatabase, User loggedInUser)
    {
        while (true)
        {
            Console.Write("\nPodaj ID samochodu do rezerwacji (lub wpisz 0, aby wrócić): ");
            if (int.TryParse(Console.ReadLine(), out int carId) && carId != 0)
            {
                var carToReserve = cars.Find(c => c.Id == carId && c.IsAvailable);
                if (carToReserve != null)
                {
                    CollectReservationDetails(carToReserve, loggedInUser);
                    carDatabase.SaveCars(cars);
                    break;
                }
                else
                {
                    ShowError("Samochód nie jest dostępny lub nie istnieje.");
                }
            }
            else if (carId == 0)
            {
                break;
            }
            else
            {
                ShowError("Nieprawidłowe ID.");
            }
        }
    }

    private static void CollectReservationDetails(Car car, User loggedInUser)
    {
        Console.Write("Podaj nazwisko wynajmującego: ");
        car.RenterSurname = Console.ReadLine();

        Console.Write("Podaj liczbę dni wynajmu: ");
        if (int.TryParse(Console.ReadLine(), out int rentalDays))
        {
            car.RentalDays = rentalDays;
            car.IsAvailable = false;
            car.AssignedEmployee = $"{loggedInUser.FirstName} {loggedInUser.LastName}";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nRezerwacja samochodu {car.Model} została pomyślnie dokonana przez {car.RenterSurname}.");
            Console.WriteLine($"Rezerwacja dokonała: {car.AssignedEmployee}");
            Console.ResetColor();
        }
        else
        {
            ShowError("Nieprawidłowa liczba dni.");
        }
    }

    private static HashSet<string> ExtractRenterSurnames(List<Car> cars)
    {
        var renters = new HashSet<string>();
        foreach (var car in cars)
        {
            if (!string.IsNullOrEmpty(car.RenterSurname))
            {
                renters.Add(car.RenterSurname);
            }
        }
        return renters;
    }

    private static void DisplayRenterList(HashSet<string> renters)
    {
        Console.WriteLine("Lista wynajmujących:");
        foreach (var renter in renters)
        {
            Console.WriteLine($"- {renter}");
        }
    }

    private static void HandleRenterCheck(List<Car> cars)
    {
        Console.Write("\nPodaj nazwisko wynajmującego (lub wpisz 0, aby wrócić): ");
        string surnameToCheck = Console.ReadLine();

        if (surnameToCheck == "0")
        {
            return;
        }

        var matchedCars = cars.FindAll(c => c.RenterSurname.Equals(surnameToCheck, StringComparison.OrdinalIgnoreCase));
        if (matchedCars.Count > 0)
        {
            foreach (var car in matchedCars)
            {
                Console.WriteLine($"Samochód {car.Model} ({car.Brand}) jest wynajęty przez {car.RenterSurname} na {car.RentalDays} dni.");
                Console.WriteLine($"Koszt wynajmu: {car.TotalCost} PLN\nRezerwacja wykonana przez: {car.AssignedEmployee}");
            }
        }
        else
        {
            ShowError("Nie znaleziono wynajmującego o podanym nazwisku.");
        }
    }

    private static void HandleRentalPayment(List<Car> cars, CarDatabase carDatabase)
    {
        while (true)
        {
            Console.Write("\nPodaj ID samochodu do opłacenia (lub wpisz 0, aby wrócić): ");
            if (int.TryParse(Console.ReadLine(), out int carId) && carId != 0)
            {
                var carToPay = cars.Find(c => c.Id == carId);
                if (carToPay != null && !carToPay.IsAvailable)
                {
                    carToPay.IsAvailable = true;
                    carToPay.RenterSurname = "";
                    carToPay.RentalDays = 0;
                    carToPay.AssignedEmployee = "";

                    carDatabase.SaveCars(cars);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Wynajem opłacony i samochód zwolniony.");
                    Console.ResetColor();
                    break;
                }
                else
                {
                    ShowError("Nie znaleziono wynajętego samochodu o podanym ID.");
                }
            }
            else if (carId == 0)
            {
                break;
            }
            else
            {
                ShowError("Nieprawidłowe ID.");
            }
        }
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
