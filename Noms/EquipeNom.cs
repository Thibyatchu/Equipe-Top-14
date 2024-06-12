using EquipeTop14.Models;
using System.Text.Json;

namespace EquipeTop14.Noms
{
    public static class EquipeNom
    {
        private static readonly string FilePath = "equipes.json";

        public static async Task<List<Equipe>> GetAllAsync()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Equipe>();
            }

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Equipe>>(json);
        }

        public static async Task SaveAllAsync(List<Equipe> equipes)
        {
            var json = JsonSerializer.Serialize(equipes);
            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}

