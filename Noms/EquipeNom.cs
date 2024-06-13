using EquipeTop14.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EquipeTop14.Noms
{
    public static class EquipeNom
    {
        private static readonly string FilePathEquipes = "equipes.json";
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        public static async Task<List<Equipe>> GetAllAsync()
        {
            if (!File.Exists(FilePathEquipes))
            {
                return new List<Equipe>();
            }

            var json = await File.ReadAllTextAsync(FilePathEquipes);
            return JsonSerializer.Deserialize<List<Equipe>>(json);
        }

        public static async Task SaveAllAsync(List<Equipe> equipes)
        {
            var json = JsonSerializer.Serialize(equipes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePathEquipes, json);
        }

        public static async Task AddEquipeAsync(Equipe equipe)
        {
            await Semaphore.WaitAsync();
            try
            {
                var equipes = await GetAllAsync();

                equipe.Id = equipes.Any() ? equipes.Max(e => e.Id) + 1 : 1;
                equipes.Add(equipe);

                await SaveAllAsync(equipes);
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public static async Task<bool> DeleteEquipeAsync(int id)
        {
            await Semaphore.WaitAsync();
            try
            {
                var equipes = await GetAllAsync();
                var equipeToRemove = equipes.FirstOrDefault(e => e.Id == id);

                if (equipeToRemove == null)
                {
                    return false;
                }

                equipes.Remove(equipeToRemove);

                // Réaffecter les IDs
                for (int i = 0; i < equipes.Count; i++)
                {
                    equipes[i].Id = i + 1;
                }

                await SaveAllAsync(equipes);
                return true;
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}