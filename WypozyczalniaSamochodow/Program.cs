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
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Wyświetl dostępne samochody");
            Console.WriteLine("2. Wyświetl wszystkie samochody");
            Console.WriteLine("3. Zarezerwuj samochód");
            Console.WriteLine("4. Sprawdź nazwisko wynajmującego");
            Console.WriteLine("5. Opłać wynajem");
            Console.WriteLine("6. Wyjście");
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
                    PayForRental(cars, carDatabase);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Niepoprawna opcja. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayAvailableCars(List<Car> cars)
    {
        Console.Clear();
        var availableCars = cars.FindAll(c => c.IsAvailable);
        if (availableCars.Count == 0)
        {
            Console.WriteLine("Brak dostępnych samochodów.");
        }
        else
        {
            Console.WriteLine("Dostępne samochody:");
            foreach (var car in availableCars)
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
        var availableCars = cars.FindAll(c => c.IsAvailable);
        if (availableCars.Count == 0)
        {
            Console.WriteLine("Brak dostępnych samochodów do rezerwacji.");
        }
        else
        {
            Console.WriteLine("Dostępne samochody do rezerwacji:");
            foreach (var car in availableCars)
            {
                Console.WriteLine(car);
            }

            int carId = -1;
            while (carId == -1)
            {
                Console.Write("Podaj ID samochodu do rezerwacji (lub wpisz 0, aby wrócić): ");
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

                            Console.WriteLine($"Rezerwacja samochodu {carToReserve.Model} została pomyślnie dokonana przez {carToReserve.RenterSurname}.");
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowa liczba dni.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Samochód nie jest dostępny lub nie istnieje.");
                        carId = -1;
                    }
                }
                else if (carId == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowe ID.");
                }
            }
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    static void CheckRenterSurname(List<Car> cars)
    {
        Console.Clear();
        var rentedCars = cars.FindAll(c => !c.IsAvailable);
        if (rentedCars.Count == 0)
        {
            Console.WriteLine("Brak wynajętych samochodów.");
            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Lista wynajmujących samochody:");
        HashSet<string> renters = new HashSet<string>();
        foreach (var car in rentedCars)
        {
            renters.Add(car.RenterSurname);
        }

        foreach (var renter in renters)
        {
            Console.WriteLine(renter);
        }

        while (true)
        {
            Console.Write("\nPodaj nazwisko wynajmującego (lub wpisz 0, aby wrócić): ");
            string surnameToCheck = Console.ReadLine();

            if (surnameToCheck == "0")
            {
                break;
            }

            bool found = false;
            foreach (var car in rentedCars)
            {
                if (car.RenterSurname.Equals(surnameToCheck, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Samochód {car.Model} ({car.Brand}) jest wynajęty przez {car.RenterSurname} na {car.RentalDays} dni.");
                    Console.WriteLine($"Koszt wynajmu: {car.TotalCost} PLN");
                    found = true;
                }
            }

            if (!found)
            {
                Console.Clear();
                Console.WriteLine("Nie znaleziono wynajmującego o podanym nazwisku.");
                Console.WriteLine("Lista wynajmujących samochody:");
                foreach (var renter in renters)
                {
                    Console.WriteLine(renter);
                }
            }
            else
            {
                Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
                Console.ReadKey();
                break;
            }
        }
    }



    static void PayForRental(List<Car> cars, CarDatabase carDatabase)
    {
        Console.Clear();
        var rentedCars = cars.FindAll(c => !c.IsAvailable);
        if (rentedCars.Count == 0)
        {
            Console.WriteLine("Brak wynajętych samochodów do opłacenia.");
        }
        else
        {
            Console.WriteLine("Wynajęte samochody:");
            foreach (var car in rentedCars)
            {
                Console.WriteLine(car);
            }

            int carId = -1;
            while (carId == -1)
            {
                Console.Write("Podaj ID samochodu do opłacenia (lub wpisz 0, aby wrócić): ");
                if (int.TryParse(Console.ReadLine(), out carId) && carId != 0)
                {
                    var carToPay = rentedCars.Find(c => c.Id == carId);
                    if (carToPay != null)
                    {
                        Console.WriteLine($"Koszt wynajmu samochodu {carToPay.Model}: {carToPay.TotalCost} PLN");

                        carToPay.IsAvailable = true;
                        carToPay.RenterSurname = "";
                        carToPay.RentalDays = 0;

                        carDatabase.SaveCars(cars);

                        Console.WriteLine("Wynajem opłacony i samochód zwolniony.");
                    }
                    else
                    {
                        Console.WriteLine("Nie znaleziono wynajętego samochodu o podanym ID.");
                        carId = -1;
                    }
                }
                else if (carId == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowe ID.");
                }
            }
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }
}
