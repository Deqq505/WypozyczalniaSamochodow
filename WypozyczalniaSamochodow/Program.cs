using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Title = "Wypożyczalnia Samochodów";

        CarDatabase carDatabase = new CarDatabase();
        UserDatabase userDatabase = new UserDatabase();

        List<Car> cars = carDatabase.LoadCars();
        List<User> users = userDatabase.LoadUsers();

        User currentUser = UserService.LoginUser(users);

        if (currentUser.IsAdmin)
        {
            UserService.AdminPanel(cars, carDatabase, users, userDatabase);
        }
        else
        {
            UserService.EmployeePanel(cars, carDatabase, currentUser);
        }
    }
}
