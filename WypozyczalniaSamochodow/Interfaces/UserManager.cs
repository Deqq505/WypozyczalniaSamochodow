using System;
using System.Collections.Generic;

class UserManager : IUserManager
{
    private readonly IDisplayService _displayService;
    private readonly IAuthenticationService _authService;

    public UserManager(IDisplayService displayService, IAuthenticationService authService)
    {
        _displayService = displayService;
        _authService = authService;
    }

    public void AddUser(List<User> users, UserDatabase userDatabase)
    {
        Console.Clear();
        _displayService.DisplayHeader("Dodaj Użytkownika");

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

        Console.Write("Podaj hasło: ");
        string password = _authService.ReadPassword();
        newUser.Password = _authService.HashPassword(password);

        Console.Write("Czy to administrator (tak/nie): ");
        string isAdminInput = Console.ReadLine().ToLower();
        newUser.IsAdmin = isAdminInput == "tak";

        users.Add(newUser);
        userDatabase.SaveUsers(users);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nUżytkownik został dodany pomyślnie.");
        Console.ResetColor();
        Console.ReadKey();
    }

    public void RemoveUser(List<User> users, UserDatabase userDatabase)
    {
        Console.Clear();
        _displayService.DisplayHeader("Usuń Użytkownika");

        Console.Write("Podaj login użytkownika, którego chcesz usunąć: ");
        string loginToRemove = Console.ReadLine();

        User userToRemove = users.Find(u => u.Login == loginToRemove);

        if (userToRemove != null)
        {
            users.Remove(userToRemove);
            userDatabase.SaveUsers(users);
            Console.WriteLine("\nUżytkownik usunięty.");
        }
        else
        {
            Console.WriteLine("\nNie znaleziono użytkownika.");
        }
        Console.ReadKey();
    }

    public void DisplayAllUsers(List<User> users)
    {
        Console.Clear();
        _displayService.DisplayHeader("Lista Użytkowników");

        foreach (var user in users)
        {
            Console.WriteLine($"{(user.IsAdmin ? "Admin" : "Pracownik")}: {user.FirstName} {user.LastName}");
        }
        Console.ReadKey();
    }
}
