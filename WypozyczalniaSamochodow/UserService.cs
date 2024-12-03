using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class UserService
{
    public static User LoginUser(List<User> users)
    {
        User currentUser = null;

        while (currentUser == null)
        {
            Console.Clear();
            MenuService.DisplayHeader("Logowanie");

            Console.Write("Podaj login: ");
            string login = Console.ReadLine();

            Console.Write("Podaj hasło: ");
            string password = ReadPassword();

            currentUser = users.Find(u => u.Login == login);

            if (currentUser != null)
            {
                if (VerifyPassword(currentUser.Password, password))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nZalogowano pomyślnie!");
                    Console.ResetColor();
                    return currentUser;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Błędne hasło.");
                    Console.ResetColor();
                    currentUser = null;
                    Console.WriteLine("\nNaciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Błędny login.");
                Console.ResetColor();
                currentUser = null;
                Console.WriteLine("\nNaciśnij dowolny klawisz, aby spróbować ponownie.");
                Console.ReadKey();
            }
        }

        return currentUser;
    }

    public static void AdminPanel(List<Car> cars, CarDatabase carDatabase, List<User> users, UserDatabase userDatabase)
    {
        while (true)
        {
            Console.Clear();
            MenuService.DisplayHeader("Panel Administratora");

            Console.WriteLine("1. Dodaj użytkownika");
            Console.WriteLine("2. Usuń użytkownika");
            Console.WriteLine("3. Wyświetl wszystkich użytkowników");
            Console.WriteLine("4. Wyjście");
            Console.Write("\nWybierz opcję: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddUser(users, userDatabase);
                    break;
                case "2":
                    RemoveUser(users, userDatabase);
                    break;
                case "3":
                    DisplayAllUsers(users);
                    break;
                case "4":
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

    public static void EmployeePanel(List<Car> cars, CarDatabase carDatabase, User currentUser)
    {
        while (true)
        {
            Console.Clear();
            MenuService.DisplayHeader("Panel Pracownika");

            Console.WriteLine("1. Wyświetl dostępne samochody");
            Console.WriteLine("2. Wyświetl wszystkie samochody");
            Console.WriteLine("3. Zarezerwuj samochód");
            Console.WriteLine("4. Sprawdź nazwisko wynajmującego");
            Console.WriteLine("5. Opłać wynajem");
            Console.WriteLine("6. Wyświetl swoje dane");
            Console.WriteLine("7. Wyjście");
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
                    CarService.ReserveCar(cars, carDatabase, currentUser);
                    break;
                case "4":
                    CarService.CheckRenterSurname(cars);
                    break;
                case "5":
                    CarService.PayForRental(cars, carDatabase);
                    break;
                case "6":
                    DisplayUserData(currentUser);
                    break;
                case "7":
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

    public static void AddUser(List<User> users, UserDatabase userDatabase)
    {
        Console.Clear();
        MenuService.DisplayHeader("Dodaj Użytkownika");

        User newUser = new User();

        Console.Write("Podaj imię: ");
        newUser.FirstName = Console.ReadLine();

        Console.Write("Podaj nazwisko: ");
        newUser.LastName = Console.ReadLine();

        Console.Write("Podaj płeć: ");
        newUser.Gender = Console.ReadLine();

        Console.Write("Podaj wiek: ");
        if (int.TryParse(Console.ReadLine(), out int age))
        {
            newUser.Age = age;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Niepoprawny wiek.");
            Console.ResetColor();
            return;
        }

        Console.Write("Podaj login: ");
        newUser.Login = Console.ReadLine();

        string password;
        string confirmPassword;

        do
        {
            Console.Write("Podaj hasło: ");
            password = ReadPassword();

            Console.Write("Potwierdź hasło: ");
            confirmPassword = ReadPassword();

            if (password != confirmPassword)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hasła nie są identyczne. Spróbuj ponownie.");
                Console.ResetColor();
            }
        } while (password != confirmPassword);

        newUser.Password = HashPassword(password);

        Console.Write("Czy to administrator (tak/nie): ");
        string isAdminInput = Console.ReadLine().ToLower();
        newUser.IsAdmin = isAdminInput == "tak";

        users.Add(newUser);
        userDatabase.SaveUsers(users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nUżytkownik został dodany pomyślnie.");
        Console.ResetColor();
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static void RemoveUser(List<User> users, UserDatabase userDatabase)
    {
        Console.Clear();
        MenuService.DisplayHeader("Usuń Użytkownika");

        Console.Write("Podaj login użytkownika, którego chcesz usunąć: ");
        string loginToRemove = Console.ReadLine();

        User userToRemove = users.Find(u => u.Login == loginToRemove);

        if (userToRemove != null)
        {
            users.Remove(userToRemove);
            userDatabase.SaveUsers(users);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUżytkownik został usunięty.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nUżytkownik o podanym loginie nie istnieje.");
            Console.ResetColor();
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static void DisplayAllUsers(List<User> users)
    {
        Console.Clear();
        MenuService.DisplayHeader("Wszyscy Użytkownicy");

        if (users.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Brak użytkowników.");
            Console.ResetColor();
        }
        else
        {
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static void DisplayUserData(User user)
    {
        Console.Clear();
        MenuService.DisplayHeader("Twoje Dane");

        Console.WriteLine($"Imię: {user.FirstName}");
        Console.WriteLine($"Nazwisko: {user.LastName}");
        Console.WriteLine($"Płeć: {user.Gender}");
        Console.WriteLine($"Wiek: {user.Age}");
        Console.WriteLine($"Login: {user.Login}");
        Console.WriteLine($"Typ użytkownika: {(user.IsAdmin ? "Administrator" : "Pracownik")}");

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
        Console.ReadKey();
    }

    public static string ReadPassword()
    {
        string password = string.Empty;
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            else
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        Console.WriteLine();
        return password;
    }

    private static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }
    }

    private static bool VerifyPassword(string storedHashedPassword, string passwordToCheck)
    {
        string hashedPassword = HashPassword(passwordToCheck);
        return storedHashedPassword == hashedPassword;
    }

    public static void UpdateAdminPassword(List<User> users)
    {
        var admin = users.Find(u => u.IsAdmin == true);

        if (admin != null)
        {
            Console.WriteLine("Podaj nowe hasło dla administratora:");
            string newPassword = ReadPassword();
            admin.Password = HashPassword(newPassword);

            UserDatabase userDatabase = new UserDatabase();
            userDatabase.SaveUsers(users);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hasło administratora zostało zaktualizowane.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Nie znaleziono administratora.");
            Console.ResetColor();
        }
    }
}
