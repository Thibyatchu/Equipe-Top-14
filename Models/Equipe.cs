namespace EquipeTop14.Models;
public class Equipe
{
    /// <summary>
    /// Identifiant unique à l'équipe
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom de l'équipe, peut retourner null en cas de non saisie
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Nom de la ville où est situé l'équipe
    /// </summary>
    public string? Ville { get; set; }
    
    /// <summary>
    /// Liste des Joueurs présents dans cette équipe
    /// </summary>
    public List<Joueur> Joueurs { get; set; } = new List<Joueur>();
}