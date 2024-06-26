using EquipeTop14.Models;
using EquipeTop14.EquipeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace EquipeTop14.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EquipeController : ControllerBase
{
    private readonly ILogger<EquipeController> _logger;

    public EquipeController(ILogger<EquipeController> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// C'est la méthode utilisée pour afficher toutes les listes
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Equipe>>> GetAll()
    {
        var equipes = await EquipeServices.GetAllAsync();
        return Ok(equipes);
    }
    /// <summary>
    /// La méthode affiche une seule équipe selon l'Id écrit
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var equipes = await EquipeServices.GetAllAsync();
        var equipe = equipes.FirstOrDefault(e => e.Id == id);

        if (equipe == null)
        {
            return NotFound();
        }

        return Ok(equipe);
    }
    /// <summary>
    /// Tout comme la méthode GetAll, cette méthode n'utilise pas d'Id car à chaques ajout de nouvelle liste
    /// l'Id est auto-incrémenté dans la nouvelle liste et augmente au fur et a mesure
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Equipe equipe)
    {
        await EquipeServices.AddEquipeAsync(equipe);
        return CreatedAtAction(nameof(Get), new { id = equipe.Id }, equipe);
    }
    /// <summary>
    /// La méthode sert à modifier les paramètre de la liste équipe et de la liste joueur, présente a l'intérieur
    /// Elle utilise l'id de l'équipe pour etrouver la liste
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Equipe equipe)
    {
        if (id != equipe.Id)
            return BadRequest();

        var equipes = await EquipeServices.GetAllAsync();
        var index = equipes.FindIndex(e => e.Id == id);
        if (index == -1)
            return NotFound();

        equipes[index] = equipe;
        await EquipeServices.SaveAllAsync(equipes);
        return NoContent();
    }
    /// <summary>
    /// Cette méthode permet de supprimmer une liste en utilisant son Id pour spécifier quelle
    /// liste on souhaite supprimmer
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await EquipeServices.DeleteEquipeAsync(id);

        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
    /// <summary>
    /// Affiche les détails d'un joueur spécifique d'une équipe.
    /// </summary>
    [HttpGet("{id}/joueurs/{joueurId}")]
    public async Task<ActionResult<Joueur>> GetPlayer(int id, int joueurId)
    {
        var equipes = await EquipeServices.GetAllAsync();
        var equipe = equipes.FirstOrDefault(e => e.Id == id);
        if (equipe == null)
            return NotFound();

        var joueur = equipe.Joueurs.FirstOrDefault(j => j.Id == joueurId);
        if (joueur == null)
            return NotFound();

        return Ok(joueur);
    }
    /// <summary>
    /// Ajoute un joueur à une équipe spécifiée par son Id.
    /// </summary>
    [HttpPost("{id}/joueurs")]
    public async Task<IActionResult> AddPlayer(int id, [FromBody] Joueur joueur)
    {
        var equipes = await EquipeServices.GetAllAsync();
        var equipe = equipes.FirstOrDefault(e => e.Id == id);

        if (equipe == null)
            return NotFound();

        // Assignation d'un nouvel ID au joueur
        joueur.Id = equipe.Joueurs.Any() ? equipe.Joueurs.Max(j => j.Id) + 1 : 1;
        equipe.Joueurs.Add(joueur);

        // Sauvegarder les modifications
        await EquipeServices.SaveAllAsync(equipes);

        // Retourner l'équipe mise à jour
        return Ok(equipe);
    }

    /// <summary>
    /// Met à jour un joueur d'une équipe spécifiée par son Id.
    /// </summary>
[HttpPut("{id}/joueurs/{joueurId}")]
public async Task<IActionResult> UpdatePlayer(int id, int joueurId, Joueur joueur)
{
    if (joueurId != joueur.Id)
    {
        Console.WriteLine("ID mismatch: joueurId != joueur.Id");
        return BadRequest("ID mismatch.");
    }

    var equipes = await EquipeServices.GetAllAsync();
    var equipe = equipes.FirstOrDefault(e => e.Id == id);
    if (equipe == null)
    {
        Console.WriteLine($"Equipe not found: {id}");
        return NotFound("Equipe not found.");
    }

    var index = equipe.Joueurs.FindIndex(j => j.Id == joueurId);
    if (index == -1)
    {
        Console.WriteLine($"Player not found: {joueurId}");
        return NotFound("Player not found.");
    }

    equipe.Joueurs[index] = joueur;
    await EquipeServices.SaveAllAsync(equipes);
    Console.WriteLine($"Player updated: {joueurId}");
    return NoContent();
}


    /// <summary>
    /// Supprime un joueur d'une équipe spécifiée par son Id.
    /// </summary>
    [HttpDelete("{id}/joueurs/{joueurId}")]
    public async Task<IActionResult> DeletePlayer(int id, int joueurId)
    {
        var equipes = await EquipeServices.GetAllAsync();
        var equipe = equipes.FirstOrDefault(e => e.Id == id);
        if (equipe == null)
            return NotFound();

        var joueur = equipe.Joueurs.FirstOrDefault(j => j.Id == joueurId);
        if (joueur == null)
            return NotFound();

        equipe.Joueurs.Remove(joueur);

        // Réaffecter les IDs des joueurs restants
        for (int i = 0; i < equipe.Joueurs.Count; i++)
        {
            equipe.Joueurs[i].Id = i + 1;
        }

        await EquipeServices.SaveAllAsync(equipes);
        return NoContent();
    }
}
