using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Data;
using ApexVolley.Models;

[Authorize(Roles = "Staff,Admin")]
public class OrdersAdminController : Controller
{
    private readonly ApexVolleyContext _context;

    public OrdersAdminController(ApexVolleyContext context)
    {
        _context = context;
    }

    // Lista ordini
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return View(orders);
    }

    // Modifica stato ordine
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            TempData["ErrorMessage"] = "Ordine non trovato.";
            return RedirectToAction(nameof(Index));
        }

        order.Status = newStatus;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Stato ordine #{orderId} aggiornato a '{newStatus}'";
        return RedirectToAction(nameof(Index));
    }
}
