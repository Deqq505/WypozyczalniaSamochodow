using System.Collections.Generic;

interface IUserManager
{
    void AddUser(List<User> users, UserDatabase userDatabase);
    void RemoveUser(List<User> users, UserDatabase userDatabase);
    void DisplayAllUsers(List<User> users);
}
