using Microsoft.EntityFrameworkCore;
using gesCom.Data;
using gesCom.Models;

namespace gesCom.Services.Impl;

public class ClientService : IClientService
{
    private readonly ApplicationDbContext _context;

    public ClientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Commande>> GetCommandesClientAsync(string clientName)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Nom == clientName);
        if (client == null)
        {
            return new List<Commande>();
        }
        return await _context.Commandes
            .Where(c => c.ClientId == client.Id)
            .Include(c => c.DetailCommandes)
            .ThenInclude(dc => dc.Produit)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produit>> GetProduitsAsync()
    {
        return await _context.Produits.ToListAsync();
    }

    public async Task<Commande> CreerCommandeAsync(string clientName, Dictionary<int, int> produits)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Nom == clientName);
        if (client == null)
        {
            return null;
        }

        var commande = new Commande
        {
            ClientId = client.Id,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Etat = Etat.EnCours
        };

        foreach (var produit in produits)
        {
            var detailCommande = new DetailCommande
            {
                ProduitId = produit.Key,
                QteProduit = produit.Value,
                Prix = (await _context.Produits.FindAsync(produit.Key)).Prix * produit.Value
            };
            commande.DetailCommandes.Add(detailCommande);
        }

        commande.Montant = commande.DetailCommandes.Sum(dc => dc.Prix);

        _context.Commandes.Add(commande);
        await _context.SaveChangesAsync();

        return commande;
    }

    public async Task<Commande> GetCommandeAsync(int id)
    {
        return await _context.Commandes
            .Include(c => c.DetailCommandes)
            .ThenInclude(dc => dc.Produit)
            .Include(c => c.Livraison)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> DeclarerReceptionAsync(int commandeId)
    {
        var commande = await _context.Commandes.FindAsync(commandeId);
        if (commande == null || commande.Etat != Etat.Livrer)
        {
            return false;
        }

        commande.Etat = Etat.Payer;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EnregistrerPaiementAsync(int commandeId, double montant, string reference, TypePaiement type)
    {
        var commande = await _context.Commandes.FindAsync(commandeId);
        if (commande == null || commande.Etat != Etat.Payer)
        {
            return false;
        }

        var paiement = new Paiement
        {
            CommandeId = commandeId,
            Montant = montant,
            Reference = reference,
            Type = type
        };

        var versement = new Versement
        {
            Numero = Guid.NewGuid().ToString(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            Montant = montant,
            Paiement = paiement
        };

        _context.Paiements.Add(paiement);
        _context.Versements.Add(versement);
        await _context.SaveChangesAsync();

        return true;
    }
}