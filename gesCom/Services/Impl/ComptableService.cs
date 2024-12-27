using Microsoft.EntityFrameworkCore;
using gesCom.Data;
using gesCom.Models;

namespace gesCom.Services.Impl;

public class ComptableService : IComptableService
{
    private readonly ApplicationDbContext _context;

    public ComptableService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Commande>> GetCommandesALivrerAsync()
    {
        return await _context.Commandes
            .Where(c => c.Etat == Etat.Livrer)
            .Include(c => c.Client)
            .ToListAsync();
    }

    public async Task<Commande> GetCommandeAsync(int id)
    {
        return await _context.Commandes
            .Include(c => c.Client)
            .Include(c => c.DetailCommandes)
            .ThenInclude(dc => dc.Produit)
            .Include(c => c.Paiements)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

}