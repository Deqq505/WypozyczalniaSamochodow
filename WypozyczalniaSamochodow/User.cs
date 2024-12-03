public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}, Płeć: {Gender}, Wiek: {Age}, Login: {Login}, " +
               (IsAdmin ? "Administrator" : "Pracownik");
    }
}
