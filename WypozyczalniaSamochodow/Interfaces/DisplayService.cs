using System;

class DisplayService : IDisplayService
{
    public void DisplayHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n=== {title.ToUpper()} ===\n");
        Console.ResetColor();
    }

    public void DisplayUserData(User user)
    {
        Console.WriteLine($"Imię: {user.FirstName}");
        Console.WriteLine($"Nazwisko: {user.LastName}");
        Console.WriteLine($"Płeć: {user.Gender}");
        Console.WriteLine($"Wiek: {user.Age}");
        Console.WriteLine($"Login: {user.Login}");
        Console.WriteLine($"Administrator: {(user.IsAdmin ? "Tak" : "Nie")}");
    }
}
