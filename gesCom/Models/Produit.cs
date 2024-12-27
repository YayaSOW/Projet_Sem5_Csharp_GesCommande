using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public class Produit
{
    public int Id { get; set; }
    public string? Libelle { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif")]
    public double Prix { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La qteStock doit être un nombre positif.")]  
    public int QteStock { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La qteSeuil doit être un nombre positif.")]  
    public int QteSeuil { get; set; }
    public string? Image { get; set; }
    public virtual ICollection<DetailCommande>? DetailCommandes { get; set; }
}