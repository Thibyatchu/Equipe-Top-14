namespace EquipeTop14.Models;
public class Joueur
{
    /// <summary>
    /// Identifiant unique au joueur
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom du joueur
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Le nom de la position du joueur dans l'équipe, son poste
    /// </summary>
    public string Position { get; set; }
    
    /// <summary>
    /// Le numéro joué par le joueur (du numéro 1 à 15 le numéro correspond au poste joué)
    /// Exemple : 14 => Ailier droit, 9 => Demi de mêlé
    /// </summary>
    public int Numero { get; set; }
}