using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public class Versement
{
    public int Id { get; set; }

    [Required(ErrorMessage = "L'adresse est obligatoire")]
    public string? Numero { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Range(0, double.MaxValue, ErrorMessage = "Le montant doit être un nombre positif.")]
    public double Montant { get; set; }
    public int PaiementId { get; set; }
    public Paiement? Paiement { get; set; }
}