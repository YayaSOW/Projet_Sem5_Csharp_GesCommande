using gesCom.Models;

namespace gesCom.Services;

public interface IAuthService
{
    Task<Personne> AuthenticateAsync(string email, string password);
}