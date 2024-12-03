public static class LoginService
{
    public static User Login(List<User> users)
    {
        Console.Clear();
        MenuService.DisplayHeader("Logowanie");

        Console.Write("Podaj login: ");
        string login = Console.ReadLine();

        Console.Write("Podaj hasło: ");
        string password = Console.ReadLine();

        User user = users.Find(u => u.Login == login && u.Password == password);

        if (user != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nZalogowano pomyślnie!");
            Console.ResetColor();
            return user;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nNiepoprawny login lub hasło.");
            Console.ResetColor();
            return null;
        }
    }
}
