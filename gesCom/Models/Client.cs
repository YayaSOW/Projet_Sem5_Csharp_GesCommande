using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public class Client : Personne
{
    [Required(ErrorMessage = "L'adresse est obligatoire")]
    public string Adresse { get; set; } = null!;

    [Range(0, double.MaxValue, ErrorMessage = "Le solde doit être un nombre positif")]
    public double Solde { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]

    public virtual ICollection<Commande>? Commandes { get; set; }
}