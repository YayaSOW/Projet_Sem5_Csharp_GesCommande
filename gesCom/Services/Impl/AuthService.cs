using Microsoft.EntityFrameworkCore;
using gesCom.Data;
using gesCom.Models;

namespace gesCom.Services.Impl;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Personne> AuthenticateAsync(string email, string password)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(p => p.Email == email && p.MotDePasse == password);
        if (client != null) return client;

        var rs = await _context.ResponsableStocks.FirstOrDefaultAsync(p => p.Email == email && p.MotDePasse == password);
        if (rs != null) return rs;

        var comptable = await _context.Comptables.FirstOrDefaultAsync(p => p.Email == email && p.MotDePasse == password);
        if (comptable != null) return comptable;

        return null;
    }

    private bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        return enteredPassword == storedPassword;
    }
}