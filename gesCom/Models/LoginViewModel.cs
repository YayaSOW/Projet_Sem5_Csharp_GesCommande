using System.ComponentModel.DataAnnotations;

namespace gesCom.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}