using gesCom.Models;

namespace gesCom.Services;

public interface IComptableService
{
    Task<IEnumerable<Commande>> GetCommandesALivrerAsync();
    Task<Commande> GetCommandeAsync(int id);
}