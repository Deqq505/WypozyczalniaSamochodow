using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Title = "Wypożyczalnia Samochodów";

        CarDatabase carDatabase = new CarDatabase();
        List<Car> cars = carDatabase.LoadCars();

        while (true)
        {
            Console.Clear();
            DisplayHeader("Wypożyczalnia Samochodów");

            Console.WriteLine("Menu:");
            Console.WriteLine("1. Wyświetl dostępne samochody");
            Console.WriteLine("2. Wyświetl wszystkie samochody");
            Console.WriteLine("3. Zarezerwuj samochód");
            Console.WriteLine("4. Sprawdź nazwisko wynajmującego");
            Console.WriteLine("5. Opłać wynajem");
            Console.WriteLine("6. Wyjście");
            Console.Write("\nWybierz opcję: ");

            string choice = Console.ReadLine();
            switch (choice)
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
                    PayForRental(cars, carDatabase);
                    break;
                case "6":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNiepoprawna opcja. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================================");
        Console.WriteLine($"  {title}");
        Console.WriteLine("====================================");
        Console.ResetColor();
    }

    static void DisplayAvailableCars(List<Car> cars)
    {
        Console.Clear();
        DisplayHeader("Dostępne Samochody");

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

    static void DisplayAllCars(List<Car> cars)
    {
        Console.Clear();
        DisplayHeader("Wszystkie Samochody");

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

    static void ReserveCar(List<Car> cars, CarDatabase carDatabase)
    {
        Console.Clear();
        DisplayHeader("Rezerwacja Samochodu");

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

    static void CheckRenterSurname(List<Car> cars)
    {
        Console.Clear();
        DisplayHeader("Sprawdzenie Wynajmującego");

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
                return;  // Wracamy do menu głównego
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


    static void PayForRental(List<Car> cars, CarDatabase carDatabase)
    {
        Console.Clear();
        DisplayHeader("Opłata za Wynajem");

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
