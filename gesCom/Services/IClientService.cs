using gesCom.Models;

namespace gesCom.Services;

public interface IClientService
{
    Task<IEnumerable<Commande>> GetCommandesClientAsync(string clientName);
    Task<IEnumerable<Produit>> GetProduitsAsync();
    Task<Commande> CreerCommandeAsync(string clientName, Dictionary<int, int> produits);
    Task<Commande> GetCommandeAsync(int id);
    Task<bool> DeclarerReceptionAsync(int commandeId);
    Task<bool> EnregistrerPaiementAsync(int commandeId, double montant, string reference, TypePaiement type);
}