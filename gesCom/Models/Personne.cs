using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public abstract class Personne
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string? Nom { get; set; }

    [Required(ErrorMessage = "Le prénom est obligatoire")]
    public string? Prenom { get; set; }

    [Required(ErrorMessage = "Le téléphone est obligatoire")]
    [RegularExpression(@"^(77|78|76)[0-9]{7}$", ErrorMessage = "Le téléphone doit commencer par 77 ou 78 ou 76 et doit avoir 9 chiffres")]
    public string? Telephone { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string? MotDePasse { get; set; }

    public string? Role { get; set; }
}