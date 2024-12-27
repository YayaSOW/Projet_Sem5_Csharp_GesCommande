using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gesCom.Models;
using gesCom.Services;
using Microsoft.AspNetCore.Authorization;

namespace gesCom.Controllers;

[Authorize(Roles = "Client")]
public class ClientController : Controller
{
    private readonly ILogger<ClientController> _logger;
    private readonly IClientService _clientService;
    private const int PageSize = 3;

    public ClientController(ILogger<ClientController> logger, IClientService clientService)
    {
        _logger = logger;
        _clientService = clientService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var commandes = await _clientService.GetCommandesClientAsync(User.Identity.Name);
        int totalCommandes = commandes.Count();
        int totalPages = (int)Math.Ceiling(totalCommandes / (double)PageSize);

        var commandesForPage = commandes
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        return View(commandesForPage);
    }

    public async Task<IActionResult> NouvelleCommande()
    {
        var produits = await _clientService.GetProduitsAsync();
        return View(produits);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NouvelleCommande(Dictionary<int, int> produits)
    {
        if (ModelState.IsValid)
        {
            var commande = await _clientService.CreerCommandeAsync(User.Identity.Name, produits);
            if (commande != null)
            {
                TempData["Message"] = "Commande créée avec succès!";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(await _clientService.GetProduitsAsync());
    }

    public async Task<IActionResult> SuivreCommande(int id)
    {
        var commande = await _clientService.GetCommandeAsync(id);
        if (commande == null)
        {
            return NotFound();
        }
        return View(commande);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclarerReception(int id)
    {
        var success = await _clientService.DeclarerReceptionAsync(id);
        if (success)
        {
            TempData["Message"] = "Réception de la commande déclarée avec succès!";
        }
        else
        {
            TempData["Error"] = "Erreur lors de la déclaration de réception de la commande.";
        }
        return RedirectToAction(nameof(SuivreCommande), new { id = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnregistrerPaiement(int commandeId, double montant, string reference, TypePaiement type)
    {
        if (ModelState.IsValid)
        {
            var success = await _clientService.EnregistrerPaiementAsync(commandeId, montant, reference, type);
            if (success)
            {
                TempData["Message"] = "Paiement enregistré avec succès!";
            }
            else
            {
                TempData["Error"] = "Erreur lors de l'enregistrement du paiement.";
            }
        }
        return RedirectToAction(nameof(SuivreCommande), new { id = commandeId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}