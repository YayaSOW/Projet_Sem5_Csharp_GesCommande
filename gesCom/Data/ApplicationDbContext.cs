using gesCom.Models;
using Microsoft.EntityFrameworkCore;

namespace gesCom.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ResponsableStock> ResponsableStocks { get; set; }
    public DbSet<Comptable> Comptables { get; set; }
    public DbSet<Livreur> Livreurs { get; set; }
    public DbSet<Produit> Produits { get; set; }
    public DbSet<Commande> Commandes { get; set; }
    public DbSet<DetailCommande> DetailCommandes { get; set; }
    public DbSet<Livraison> Livraisons { get; set; }
    public DbSet<Paiement> Paiements { get; set; }
    public DbSet<Versement> Versements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
        .HasMany(c => c.Commandes)
        .WithOne(cd => cd.Client)
        .HasForeignKey(cd => cd.ClientId);

        modelBuilder.Entity<Commande>()
        .HasMany(c => c.DetailCommandes)
        .WithOne(d => d.Commande)
        .HasForeignKey(d => d.CommandeId);

        modelBuilder.Entity<Commande>()
        .HasMany(c => c.Paiements)
        .WithOne(p => p.Commande)
        .HasForeignKey(p => p.CommandeId);

        modelBuilder.Entity<Commande>()
        .HasOne(c => c.Livraison)
        .WithOne(l => l.Commande)
        .HasForeignKey<Livraison>(l => l.CommandeId);

        modelBuilder.Entity<Livreur>()
        .HasMany(l => l.Livraisons)
        .WithOne(li => li.Livreur)
        .HasForeignKey(li => li.LivreurId);

        modelBuilder.Entity<Paiement>()
        .HasMany(p => p.Versements)
        .WithOne(v => v.Paiement)
        .HasForeignKey(v => v.PaiementId);

        modelBuilder.Entity<Produit>()
        .HasMany(p => p.DetailCommandes)
        .WithOne(dc => dc.Produit)
        .HasForeignKey(dc => dc.ProduitId);
    }

}