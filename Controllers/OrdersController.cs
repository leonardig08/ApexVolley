using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Data;
using ApexVolley.Models;
using System.Security.Claims;

[Authorize]
public class OrdersController : Controller
{
    private readonly ApexVolleyContext _context;

    public OrdersController(ApexVolleyContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

    // Mostra tutti gli ordini dell'utente
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return View(orders);
    }

    // Dettagli di un singolo ordine
    public async Task<IActionResult> Details(int id)
    {
        var userId = GetUserId();
        var order = await _context.Orders
            .Where(o => o.UserId == userId && o.Id == id)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            TempData["ErrorMessage"] = "Ordine non trovato.";
            return RedirectToAction(nameof(Index));
        }

        return View(order);
    }
}
