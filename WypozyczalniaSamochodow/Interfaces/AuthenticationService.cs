using System;
using System.Collections.Generic;

class AuthenticationService : IAuthenticationService
{
    public User LoginUser(List<User> users)
    {
        Console.Clear();
        Console.WriteLine("Logowanie\n");

        Console.Write("Podaj login: ");
        string login = Console.ReadLine();

        Console.Write("Podaj hasło: ");
        string password = ReadPassword();
        string hashedPassword = HashPassword(password);

        User user = users.Find(u => u.Login == login && u.Password == hashedPassword);

        if (user != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLogowanie pomyślne!");
            Console.ResetColor();
            return user;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nNiepoprawny login lub hasło.");
            Console.ResetColor();
            Console.ReadKey();
            return null;
        }
    }

    public string HashPassword(string password)
    {
        int hash = password.GetHashCode();
        return hash.ToString();
    }

    public string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, (password.Length - 1));
                Console.Write("\b \b");
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}
