public static class MenuService
{
    public static void DisplayHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================================");
        Console.WriteLine($"  {title}");
        Console.WriteLine("====================================");
        Console.ResetColor();
    }
}
