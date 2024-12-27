using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public class DetailCommande
{
    public int Id { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "La qteProduit doit être un nombre positif.")]
    public int QteProduit { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif")]
    public double Prix { get; set; }

    public int ProduitId { get; set; }
    public Produit? Produit { get; set; }

    public int CommandeId { get; set; }
    public Commande? Commande { get; set; }
}