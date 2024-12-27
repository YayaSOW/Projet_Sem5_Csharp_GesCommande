using gesCom.Models;

namespace gesCom.Services;

public interface IResponsableStockService
{
    Task<IEnumerable<Commande>> GetCommandesEnAttenteAsync();
    Task<IEnumerable<Produit>> GetProduitsAsync();
    Task<Produit> GetProduitAsync(int id);
    Task AjouterProduitAsync(Produit produit);
    Task MettreAJourStockAsync(int produitId, int quantite);
    Task<Commande> GetCommandeAsync(int id);
    Task ConfirmerPreparationAsync(int commandeId);
    Task<IEnumerable<Livreur>> GetLivreursDisponiblesAsync();
    Task PlanifierLivraisonAsync(int commandeId, int livreurId, DateTime dateLivraison, string adresse);
}