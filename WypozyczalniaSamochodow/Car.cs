public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public double RentalPrice { get; set; }
    public bool IsAvailable { get; set; }
    public string RenterSurname { get; set; } // Nazwisko wynajmującego
    public double Mileage { get; set; }

    public override string ToString()
    {
        return $"{Id} | {Brand} | {Model} | {Year} | {Color} | {RentalPrice} PLN/dzień | " +
               $"{(IsAvailable ? "Dostępny" : $"Zarezerwowany przez: {RenterSurname}")}";
    }
}
