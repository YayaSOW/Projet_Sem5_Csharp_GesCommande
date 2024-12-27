using Microsoft.AspNetCore.Mvc;
using gesCom.Models;
using gesCom.Services;
using Microsoft.AspNetCore.Authorization;

namespace gesCom.Controllers;

[Authorize(Roles = "ResponsableStock")]
public class ResponsableStockController : Controller
{
    private readonly ILogger<ResponsableStockController> _logger;
    private readonly IResponsableStockService _rsService;
    private const int PageSize = 10;

    public ResponsableStockController(ILogger<ResponsableStockController> logger, IResponsableStockService rsService)
    {
        _logger = logger;
        _rsService = rsService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var commandes = await _rsService.GetCommandesEnAttenteAsync();
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

    public async Task<IActionResult> GererProduits(int page = 1)
    {
        var produits = await _rsService.GetProduitsAsync();
        int totalProduits = produits.Count();
        int totalPages = (int)Math.Ceiling(totalProduits / (double)PageSize);

        var produitsForPage = produits
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        return View(produitsForPage);
    }

    public IActionResult NouveauProduit()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NouveauProduit(Produit produit, IFormFile image)
    {
        if (ModelState.IsValid)
        {
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "produits", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                produit.Image = "/images/produits/" + fileName;
            }

            await _rsService.AjouterProduitAsync(produit);
            TempData["Message"] = "Produit ajouté avec succès!";
            return RedirectToAction(nameof(GererProduits));
        }
        return View(produit);
    }

    public async Task<IActionResult> MettreAJourStock(int id)
    {
        var produit = await _rsService.GetProduitAsync(id);
        if (produit == null)
        {
            return NotFound();
        }
        return View(produit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MettreAJourStock(int id, int quantite)
    {
        await _rsService.MettreAJourStockAsync(id, quantite);
        TempData["Message"] = "Stock mis à jour avec succès!";
        return RedirectToAction(nameof(GererProduits));
    }

    public async Task<IActionResult> PreparerCommande(int id)
    {
        var commande = await _rsService.GetCommandeAsync(id);
        if (commande == null)
        {
            return NotFound();
        }
        return View(commande);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmerPreparation(int id)
    {
        await _rsService.ConfirmerPreparationAsync(id);
        TempData["Message"] = "Préparation de la commande confirmée!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> PlanifierLivraison(int id)
    {
        var commande = await _rsService.GetCommandeAsync(id);
        if (commande == null)
        {
            return NotFound();
        }
        ViewBag.Livreurs = await _rsService.GetLivreursDisponiblesAsync();
        return View(commande);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlanifierLivraison(int commandeId, int livreurId, DateTime dateLivraison, string adresse)
    {
        await _rsService.PlanifierLivraisonAsync(commandeId, livreurId, dateLivraison, adresse);
        TempData["Message"] = "Livraison planifiée avec succès!";
        return RedirectToAction(nameof(Index));
    }
}