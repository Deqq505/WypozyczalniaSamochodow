using System;
using System.Collections.Generic;

public static class CarService
{
    public static void DisplayAvailableCars(List<Car> cars)
    {
        Console.Clear();
        MenuService.DisplayHeader("Dostępne Samochody");

        var availableCars = cars.FindAll(c => c.IsAvailable);
        if (availableCars.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Brak dostępnych samochodów.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Dostępne samochody:");
            foreach (var car in availableCars)
            {
                Console.WriteLine(car);
            }
        }
        Console.ResetColor();
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static void DisplayAllCars(List<Car> cars)
    {
        Console.Clear();
        MenuService.DisplayHeader("Wszystkie Samochody");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Wszystkie samochody:");
        foreach (var car in cars)
        {
            Console.WriteLine(car);
        }
        Console.ResetColor();
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static void ReserveCar(List<Car> cars, CarDatabase carDatabase)
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
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Dostępne samochody do rezerwacji:");
            foreach (var car in availableCars)
            {
                Console.WriteLine(car);
            }

            int carId = -1;
            while (carId == -1)
            {
                Console.Write("\nPodaj ID samochodu do rezerwacji (lub wpisz 0, aby wrócić): ");
                if (int.TryParse(Console.ReadLine(), out carId) && carId != 0)
                {
                    var carToReserve = cars.Find(c => c.Id == carId && c.IsAvailable);
                    if (carToReserve != null)
                    {
                        Console.Write("Podaj nazwisko wynajmującego: ");
                        carToReserve.RenterSurname = Console.ReadLine();

                        Console.Write("Podaj liczbę dni wynajmu: ");
                        if (int.TryParse(Console.ReadLine(), out int rentalDays))
                        {
                            carToReserve.RentalDays = rentalDays;
                            carToReserve.IsAvailable = false;

                            carDatabase.SaveCars(cars);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nRezerwacja samochodu {carToReserve.Model} została pomyślnie dokonana przez {carToReserve.RenterSurname}.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nNieprawidłowa liczba dni.");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Samochód nie jest dostępny lub nie istnieje.");
                        Console.ResetColor();
                        carId = -1;
                    }
                }
                else if (carId == 0)
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nieprawidłowe ID.");
                    Console.ResetColor();
                }
            }
        }
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static void CheckRenterSurname(List<Car> cars)
    {
        Console.Clear();
        MenuService.DisplayHeader("Sprawdzenie Wynajmującego");

        while (true)
        {
            var renters = new HashSet<string>();
            foreach (var car in cars)
            {
                if (!string.IsNullOrEmpty(car.RenterSurname))
                {
                    renters.Add(car.RenterSurname);
                }
            }

            if (renters.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Brak wynajmujących samochodów.");
                Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Lista wynajmujących:");
            foreach (var renter in renters)
            {
                Console.WriteLine($"- {renter}");
            }

            Console.ResetColor();
            Console.Write("\nPodaj nazwisko wynajmującego (lub wpisz 0, aby wrócić): ");
            string surnameToCheck = Console.ReadLine();

            if (surnameToCheck == "0")
            {
                return;
            }

            if (renters.Contains(surnameToCheck))
            {
                bool found = false;
                foreach (var car in cars)
                {
                    if (car.RenterSurname.Equals(surnameToCheck, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Samochód {car.Model} ({car.Brand}) jest wynajęty przez {car.RenterSurname} na {car.RentalDays} dni.");
                        Console.WriteLine($"Koszt wynajmu: {car.TotalCost} PLN");
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Brak wynajmowanych samochodów dla podanego nazwiska.");
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nie znaleziono wynajmującego o podanym nazwisku.");
                Console.WriteLine("Spróbuj ponownie.");
                Console.ResetColor();
            }
        }
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
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wynajęte samochody:");
            foreach (var car in rentedCars)
            {
                Console.WriteLine(car);
            }

            int carId = -1;
            while (carId == -1)
            {
                Console.Write("\nPodaj ID samochodu do opłacenia (lub wpisz 0, aby wrócić): ");
                if (int.TryParse(Console.ReadLine(), out carId) && carId != 0)
                {
                    var carToPay = rentedCars.Find(c => c.Id == carId);
                    if (carToPay != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Koszt wynajmu samochodu {carToPay.Model}: {carToPay.TotalCost} PLN");
                        carToPay.IsAvailable = true;
                        carToPay.RenterSurname = "";
                        carToPay.RentalDays = 0;

                        carDatabase.SaveCars(cars);

                        Console.WriteLine("Wynajem opłacony i samochód zwolniony.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nie znaleziono wynajętego samochodu o podanym ID.");
                        Console.ResetColor();
                        carId = -1;
                    }
                }
                else if (carId == 0)
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nieprawidłowe ID.");
                    Console.ResetColor();
                }
            }
        }
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }
}
