interface IAuthenticationService
{
    User LoginUser(List<User> users);
    string HashPassword(string password);
    string ReadPassword();
}
