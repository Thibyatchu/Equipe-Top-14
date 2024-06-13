namespace EquipeTop14.Models;
public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
}