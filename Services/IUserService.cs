namespace EquipeTop14.Service;
public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
}