using EquipeTop14.Service;

public class UserService : IUserService
{
    private readonly List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "Thib", Password = "Thib220305" },
    };

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        return await Task.FromResult(user);
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}