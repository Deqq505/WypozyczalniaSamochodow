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
            MenuService.DisplayHeader("Wypożyczalnia Samochodów");

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
                    CarService.DisplayAvailableCars(cars);
                    break;
                case "2":
                    CarService.DisplayAllCars(cars);
                    break;
                case "3":
                    CarService.ReserveCar(cars, carDatabase);
                    break;
                case "4":
                    CarService.CheckRenterSurname(cars);
                    break;
                case "5":
                    CarService.PayForRental(cars, carDatabase);
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
}
