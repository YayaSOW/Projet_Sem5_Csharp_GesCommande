using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;
public class Commande
{
    public int Id { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Range(0, double.MaxValue, ErrorMessage = "Le montant doit être un nombre positif")]
    public double Montant { get; set; }
    public Etat Etat { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; } 
    public ICollection<DetailCommande>? DetailCommandes { get; set; }
    public ICollection<Paiement>? Paiements { get; set; }
    public Livraison? Livraison { get; set; }
}