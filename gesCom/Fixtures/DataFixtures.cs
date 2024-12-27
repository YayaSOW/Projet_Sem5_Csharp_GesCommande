using gesCom.Data;
using gesCom.Models;

namespace gesCom.Fixtures;

public class DataFixtures
{
    private readonly ApplicationDbContext _context;

    public DataFixtures(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Load()
    {
        if (!_context.Produits.Any())
        {
            _context.Produits.AddRange(
                new Produit { Libelle = "Ordinateur", Prix = 500000, QteStock = 20, QteSeuil = 5, Image = "/images/arg.jpg" },
                new Produit { Libelle = "Téléphone", Prix = 300000, QteStock = 50, QteSeuil = 10, Image = "/images/or.jpg" },
                new Produit { Libelle = "Imprimante", Prix = 150000, QteStock = 15, QteSeuil = 3, Image = "/images/one.jpg" }
            );
        }

        if (!_context.Clients.Any())
        {
            _context.Clients.AddRange(
                new Client
                {
                    Nom = "Doe",
                    Prenom = "John",
                    Telephone = "771234567",
                    Adresse = "Dakar",
                    Solde = 10000,
                    Email = "bb@bb.com",
                    MotDePasse = "password",
                    Role = "Client",
                    Commandes =
                    [
                        new Commande
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                            Montant = 20000,
                            Etat = Etat.EnCours,
                            DetailCommandes =
                            [
                                new DetailCommande { ProduitId = 1, QteProduit = 2, Prix = 100000 },
                                new DetailCommande { ProduitId = 2, QteProduit = 1, Prix = 300000 }
                            ],
                            Livraison = new Livraison
                            {
                                Adresse = "123 Rue des Bleus",
                                Livreur = new Livreur { Nom = "Smith", Prenom = "Jane", Telephone = "781234567"},
                            }
                        }
                    ]
                }
            );
        }

        if (!_context.Paiements.Any())
        {
            _context.Paiements.AddRange(
                new Paiement
                {
                    Montant = 20000,
                    Reference = "PAY001",
                    Type = TypePaiement.OM,
                    CommandeId = 1,
                    Versements =
                    [
                        new() { Numero = "V001", Montant = 10000, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)) },
                        new() { Numero = "V002", Montant = 10000, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) }
                    ]
                }
            );
        }

        _context.SaveChanges();
    }
}
