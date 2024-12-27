using Microsoft.EntityFrameworkCore;
using gesCom.Data;
using gesCom.Models;

namespace gesCom.Services.Impl;

public class ResponsableStockService : IResponsableStockService
{
    private readonly ApplicationDbContext _context;

    public ResponsableStockService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Commande>> GetCommandesEnAttenteAsync()
    {
        return await _context.Commandes
            .Where(c => c.Etat == Etat.EnAttente)
            .Include(c => c.Client)
            .Include(c => c.DetailCommandes)
            .ThenInclude(dc => dc.Produit)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produit>> GetProduitsAsync()
    {
        return await _context.Produits.ToListAsync();
    }

    public async Task<Produit> GetProduitAsync(int id)
    {
        return await _context.Produits.FindAsync(id);
    }

    public async Task AjouterProduitAsync(Produit produit)
    {
        _context.Produits.Add(produit);
        await _context.SaveChangesAsync();
    }

    public async Task MettreAJourStockAsync(int produitId, int quantite)
    {
        var produit = await _context.Produits.FindAsync(produitId);
        if (produit != null)
        {
            produit.QteStock += quantite;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Commande> GetCommandeAsync(int id)
    {
        return await _context.Commandes
            .Include(c => c.Client)
            .Include(c => c.DetailCommandes)
            .ThenInclude(dc => dc.Produit)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task ConfirmerPreparationAsync(int commandeId)
    {
        var commande = await _context.Commandes.FindAsync(commandeId);
        if (commande != null)
        {
            commande.Etat = Etat.EnCours;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Livreur>> GetLivreursDisponiblesAsync()
    {
        return await _context.Livreurs.ToListAsync();
    }

    public async Task PlanifierLivraisonAsync(int commandeId, int livreurId, DateTime dateLivraison, string adresse)
    {
        var commande = await _context.Commandes.FindAsync(commandeId);
        var livreur = await _context.Livreurs.FindAsync(livreurId);

        if (commande != null && livreur != null)
        {
            var livraison = new Livraison
            {
                CommandeId = commandeId,
                LivreurId = livreurId,
                Date = DateOnly.FromDateTime(dateLivraison),
                Adresse = adresse
            };

            _context.Livraisons.Add(livraison);
            commande.Etat = Etat.Livrer;
            await _context.SaveChangesAsync();
        }
    }
}