public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public double RentalPrice { get; set; }
    public bool IsAvailable { get; set; }
    public string RenterSurname { get; set; }
    public double Mileage { get; set; }
    public int RentalDays { get; set; }
    public double TotalCost => RentalDays * RentalPrice;

    public override string ToString()
    {
        return $"{Id} | {Brand} | {Model} | {Year} | {Color} | {RentalPrice} PLN/dzień | " +
               $"{(IsAvailable ? "Dostępny" : $"Zarezerwowany przez: {RenterSurname}, na {RentalDays} dni")}";
    }
}
