using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public class Paiement
{
    public int Id { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Le montant doit être un nombre positif")]
    public double Montant { get; set; }

    [Required(ErrorMessage = "La référence est requise.")]
    public string? Reference { get; set; }
    public TypePaiement Type { get; set; }
    public int CommandeId { get; set; }
    public Commande? Commande { get; set; }
    public ICollection<Versement>? Versements { get; set; }
}

public enum TypePaiement
{
    OM,
    Wave,
    Cheque
}