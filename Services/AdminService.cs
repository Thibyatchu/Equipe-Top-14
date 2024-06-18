using Newtonsoft.Json;

public interface IAdminService
{
    AdminModel CreateAdmin(AdminModel admin);
    AdminModel GetAdminByUsername(string username);
    bool Authenticate(string username, string password);
}


public class AdminService : IAdminService
{
    private readonly string _filePath = "admins.json";
    private readonly List<AdminModel> _admins;

    public AdminService()
    {
        if (File.Exists(_filePath))
        {
            var jsonData = File.ReadAllText(_filePath);
            _admins = JsonConvert.DeserializeObject<List<AdminModel>>(jsonData) ?? new List<AdminModel>();
        }
        else
        {
            _admins = new List<AdminModel>();
        }
    }

    public AdminModel CreateAdmin(AdminModel admin)
    {
        if (_admins.Any(a => a.Username == admin.Username))
        {
            throw new InvalidOperationException("Un administrateur avec ce nom d'utilisateur existe déjà.");
        }

        _admins.Add(admin);
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(_admins));
        return admin;
    }

    public AdminModel GetAdminByUsername(string username)
    {
        return _admins.FirstOrDefault(a => a.Username == username);
    }

    public bool Authenticate(string username, string password)
    {
        var admin = _admins.FirstOrDefault(a => a.Username == username);
        return admin != null && admin.Password == password;
    }
}
