using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gesCom.Models;
using gesCom.Services;
using Microsoft.AspNetCore.Authorization;

namespace gesCom.Controllers;

[Authorize(Roles = "Comptable")]
public class ComptableController : Controller
{
    private readonly ILogger<ComptableController> _logger;
    private readonly IComptableService _comptableService;
    private const int PageSize = 3;

    public ComptableController(ILogger<ComptableController> logger, IComptableService comptableService)
    {
        _logger = logger;
        _comptableService = comptableService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var commandes = await _comptableService.GetCommandesALivrerAsync();
        int totalCommandes = commandes.Count();
        int totalPages = (int)Math.Ceiling(totalCommandes / (double)PageSize);

        var commandesForPage = commandes
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        return View(commandesForPage);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}